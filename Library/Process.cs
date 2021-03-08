using System;
using System.Threading.Tasks;
using Api.Data;

namespace Api.Library
{
    public class Process
    {
        public static ProcessResult Run(Action action)
        {
            try
            {
                action();
                return ProcessResult.Ok();
            }
            catch (Exception e)
            {
                return ProcessResult.Fail(e.Message);
            }
        }

        public static async Task<ProcessResult> RunAsync(Func<Task> action)
        {
            try
            {
                await action();
                return ProcessResult.Ok();
            }
            catch (Exception e)
            {
                return ProcessResult.Fail(e.Message);
            }
        }

        public static ProcessResult<T> Run<T>(Func<T> action)
        {
            try
            {
                var result = action();
                return ProcessResult<T>.Ok(result);
            }
            catch (Exception e)
            {
                return ProcessResult<T>.Fail(e.Message);
            }
        }

        public static async Task<ProcessResult<T>> RunAsync<T>(Func<Task<T>> action)
        {
            try
            {
                var result = await action();
                return ProcessResult<T>.Ok(result);
            }
            catch (Exception e)
            {
                return ProcessResult<T>.Fail(e.Message);
            }
        }

        public static async Task<ProcessResult> RunInTransactionAsync(Func<Task> action, DataContext context)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    await action();
                    transaction.Commit();
                    return ProcessResult.Ok();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return ProcessResult.Fail(e.Message);
                }
            } 
        }
    }
}
