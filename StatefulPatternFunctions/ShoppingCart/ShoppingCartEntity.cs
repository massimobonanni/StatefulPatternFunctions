using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StatefulPatternFunctions.ShoppingCart
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ShoppingCartEntity
    {
        [JsonProperty("products")]
        public List<Product> Products { get; set; } = new List<Product>();

        public void AddProduct(Product product)
        {
            var innerProduct = Products.FirstOrDefault(p => p.Id == product.Id);
            if (innerProduct == null)
            {
                Products.Add(product);
            }
            else
            {
                innerProduct.Quantity += product.Quantity;
            }
        }

        public void Empty()
        {
            Products.Clear();
        }

        [FunctionName(nameof(ShoppingCartEntity))]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx)
            => ctx.DispatchAsync<ShoppingCartEntity>();
    }

    public static class ShoppingManagement
    {
        [FunctionName("AddProduct")]
        public static async Task<IActionResult> AddProduct(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "carts/{cartId}")] HttpRequest req,
            string cartId,
            [DurableClient] IDurableEntityClient client)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var product = JsonConvert.DeserializeObject<Product>(requestBody);

            var entityId = new EntityId(nameof(ShoppingCartEntity), cartId);

            await client.SignalEntityAsync(entityId,
                nameof(ShoppingCartEntity.AddProduct), product);

            return new OkObjectResult(null);
        }

        [FunctionName("GetCart")]
        public static async Task<IActionResult> GetCart(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "carts/{cartId}")] HttpRequest req,
            string cartId,
            [DurableClient] IDurableEntityClient client)
        {
            var entityId = new EntityId(nameof(ShoppingCartEntity), cartId);

            var cart = await client.ReadEntityStateAsync<JObject>(entityId);
            return new OkObjectResult(cart.EntityState);
        }

        public class ShoppingCartModel
        {
            public string Id { get; set; }
            public IEnumerable<Product> Products { get; set; }
        }

        [FunctionName("GetCarts")]
        public static async Task<IActionResult> GetCarts(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "carts")] HttpRequest req,
            [DurableClient] IDurableEntityClient client)
        {
            var result = new List<ShoppingCartModel>();

            var query = new EntityQuery()
            {
                PageSize = 100,
                FetchState = true
            };

            do
            {
                var cartQuery = await client.ListEntitiesAsync(query, default);
                
                foreach (var cart in cartQuery.Entities)
                {
                    
                    var cartModel = cart.State.ToObject<ShoppingCartModel>();
                    cartModel.Id = cart.EntityId.EntityKey;
                    result.Add(cartModel);
                }
                
                query.ContinuationToken = cartQuery.ContinuationToken;
            } while (query.ContinuationToken != null && query.ContinuationToken != "bnVsbA==");

            return new OkObjectResult(result);
        }

        [FunctionName("EmptyCart")]
        public static async Task<IActionResult> EmptyCart(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "carts/{cartId}")] HttpRequest req,
            string cartId,
            [DurableClient] IDurableEntityClient client)
        {
            var entityId = new EntityId(nameof(ShoppingCartEntity), cartId);

            await client.SignalEntityAsync(entityId,
             nameof(ShoppingCartEntity.Empty), null);

            return new OkObjectResult(null);
        }
    }
}