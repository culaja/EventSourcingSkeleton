using System.Threading.Tasks;
using Framework;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public static class ResultExtensions
    {
        public static async Task<IActionResult> ToActionResult<T>(this Task<Result<T>> resultTask)
        {
            var result = await resultTask;
            return result.IsSuccess
                ? new JsonResult(new {Result = result.Value}) as IActionResult
                : new BadRequestObjectResult(result.Error);
        }
        
        public static async Task<IActionResult> ToActionResult(this Task<Result> resultTask)
        {
            var result = await resultTask;
            return result.IsSuccess
                ? new JsonResult(new {}) as IActionResult
                : new BadRequestObjectResult(result.Error);
        }

        public static async Task<IActionResult> ToActionResult(this Optional<Task<Result>> optionalTaskResult)
        {
            if (optionalTaskResult.HasValue)
            {
                return await optionalTaskResult.Value.ToActionResult();
            }
            
            return new BadRequestResult();
        }
    }
}