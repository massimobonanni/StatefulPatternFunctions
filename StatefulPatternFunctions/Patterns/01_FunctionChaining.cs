using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace StatefulPatternFunctions.Patterns
{
    public static class FunctionChaining
    {
        [FunctionName("FunctionsChainingOrchestrator")]
        public static async Task<int> Run([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            try
            {
                var x = await context.CallActivityAsync<int>("F1", null);
                var y = await context.CallActivityAsync<int>("F2", x);
                return await context.CallActivityAsync<int>("F3", y);
            }
            catch (Exception)
            {
                // Error handling ...
            }
            return 0;
        }

        [FunctionName("F1")]
        public static int F1([ActivityTrigger] int value)
        {
            return 42;
        }

        [FunctionName("F2")]
        public static int F2([ActivityTrigger] int value)
        {
            return value + 1;
        }

        [FunctionName("F3")]
        public static int F3([ActivityTrigger] int value)
        {
            return value + 2;
        }

        [FunctionName("FunctionsChainingClient")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var instanceId = await starter.StartNewAsync("FunctionsChainingOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}