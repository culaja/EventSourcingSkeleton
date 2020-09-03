using System;
using System.Threading.Tasks;
using static Framework.Result;

namespace Framework
{
    public static class ResultExtensions
    {
        public static Result<TK> Map<T, TK>(this Result<T> result, Func<T, TK> onSuccessSupplier)
        {
            if (result.IsSuccess)
                return Ok(onSuccessSupplier(result.Value));

            return Fail<TK>(result.Error);
        }
        
        public static async Task<Result> Map<T>(this Result<T> result, Func<T, Task> func)
        {
            if (result.IsFailure)
                return result;

            await func(result.Value);
            
            return Ok();
        }
        
        public static Result<T> Map<T>(this Result result, Func<T> onSuccessSupplier)
        {
            if (result.IsSuccess)
                return Ok(onSuccessSupplier());

            return Fail<T>(result.Error);
        }
        
        public static async Task<Result> Map(this Result result, Func<Task> func)
        {
            if (result.IsFailure)
                return result;

            await func();
            
            return Ok();
        }

        public static async Task<Result> IgnoreIf(this Task<Result> resultTask, Error error)
        {
            var result = await resultTask;
            if (result.IsSuccess)
                return result;

            if (result.Error == error)
                return Ok();

            return result;
        }

        public static async Task<Result> OnSuccess(this Task<Result> resultTask, Func<Task<Result>> supplier)
        {
            var result = await resultTask;
            if (result.IsFailure)
                return result;

            return await supplier();
        }
        
        public static async Task<Result<T>> OnSuccess<T>(this Task<Result> resultTask, Func<T> supplier)
        {
            var result = await resultTask;
            if (result.IsFailure)
                return Fail<T>(result.Error);

            return Ok(supplier());
        }
        
        public static async Task<Result<T>> OnSuccess<T>(this Task<Result<T>> resultTask, Func<T, Task<Result<T>>> supplier)
        {
            var result = await resultTask;
            if (result.IsFailure)
                return Fail<T>(result.Error);

            return await supplier(result.Value);
        }
        
        public static async Task<Result> OnSuccess(this Task<Result> resultTask, Action supplier)
        {
            var result = await resultTask;
            if (result.IsFailure)
                return Fail(result.Error);

            supplier();
            return Ok();
        }
        
        public static async Task<Result> OnSuccess<T>(this Task<Result<T>> resultTask, Func<T, Task<Result>> supplier)
        {
            var result = await resultTask;
            if (result.IsFailure)
                return Fail(result.Error);

            
            return await supplier(result.Value);
        }

        public static async Task<Result> TransformIfError(
            this Task<Result> resultTask,
            Error errorToMatchWith,
            Error errorToTransformTo)
        {
            var result = await resultTask;
            if (result.IsFailure && result.Error == errorToMatchWith)
            {
                return Fail(errorToTransformTo);
            }

            return result;
        }

        public static async Task<Result> OnFailure(
            this Task<Result> resultTask,
            Error errorToMatchWith,
            Func<Task<Result>> supplier)
        {
            var result = await resultTask;
            if (result.IsFailure && result.Error == errorToMatchWith)
            {
                return await supplier();
            }

            return result;
        }

        public static Task<Result> OnBoth(
            this Task<Result> resultTask,
            Action action)
        {
            action();
            return resultTask;
        }
    }
}