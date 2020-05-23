using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace StatefulPatternFunctions.Patterns
{
    public static class FanOut_FanIn
    {
        [FunctionName("FanOutFanInOrchestrator")]
        public static async Task<int> Run([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var parallelTasks = new List<Task<int>>();

            var workBatch = await context.CallActivityAsync<int[]>("F1", null);
            
            for (var i = 0; i < workBatch.Length; i++)
            {
                Task<int> task = context.CallActivityAsync<int>("F2", workBatch[i]);
                parallelTasks.Add(task);
            }
            await Task.WhenAll(parallelTasks);
            var sum = parallelTasks.Sum(t => t.Result);

            return await context.CallActivityAsync<int>("F3", sum);
        }
        
    }
}