using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StatefulPatternFunctions.Patterns
{
    public static class HumanInteraction
    {
        [FunctionName("HumanInteractionOrchestrator")]
        public static async Task Run([OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            await context.CallActivityAsync("RequestApproval", null);
            using (var timeoutCts = new CancellationTokenSource())
            {
                DateTime dueTime = context.CurrentUtcDateTime.AddHours(72);
                Task durableTimeout = context.CreateTimer(dueTime, timeoutCts.Token);

                Task<bool> approvalEvent = context.WaitForExternalEvent<bool>("ApprovalEvent");

                if (approvalEvent == await Task.WhenAny(approvalEvent, durableTimeout))
                {
                    timeoutCts.Cancel();
                    await context.CallActivityAsync("ProcessApproval", approvalEvent.Result);
                }
                else
                {
                    await context.CallActivityAsync("Escalate", null);
                }
            }
        }

        [FunctionName("RaiseEventToOrchestration")]
        public static async Task Run([HttpTrigger] string instanceId,
            [DurableClient] IDurableOrchestrationClient client)
        {
            bool isApproved = true;
            await client.RaiseEventAsync(instanceId, "ApprovalEvent", isApproved);
        }
    }
}
