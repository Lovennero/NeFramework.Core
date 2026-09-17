using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Core.Utility
{
    public static class TaskUtility
    {
        public static async Task AwaitWithCancellation(Task task, CancellationToken ct)
        {
            if (!ct.CanBeCanceled || task.IsCompleted)
            {
                await task.ConfigureAwait(false);
                return;
            }

            var cancelTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            await using (ct.Register(static s =>
                         {
                             ((TaskCompletionSource<bool>)s).TrySetResult(true);
                         }, cancelTcs))
            {
                var winner = await Task.WhenAny(task, cancelTcs.Task).ConfigureAwait(false);
                if (winner != task)
                {
                    throw new OperationCanceledException(ct);
                }
            }

            await task.ConfigureAwait(false);
        }
    }
}
