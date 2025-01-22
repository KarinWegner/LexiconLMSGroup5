using Domain.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public abstract class ApiBaseResponse
    {
        public bool Success { get; set; }
        protected ApiBaseResponse(bool success) => Success = success;
        public TResultType GetOkResult<TResultType>()
        {
            if (this is ApiOkResponse<TResultType> apiOkResponse)
            {
                return apiOkResponse.Result;
            }
            throw new InvalidOperationException($"Response type {this.GetType().Name} is not ApiOkResponse");
        }
        public TResultType GetCreatedAtResult<TResultType>()
        {
            if (this is ApiCreatedAtResponse<TResultType> apiCreateResponse)
            {
                return apiCreateResponse.Result;
            }
            throw new InvalidOperationException($"Response type {this.GetType().Name} is not ApiCreatedAtResponse");
        }
        

    }
    public sealed class ApiOkResponse<TResult> : ApiBaseResponse
    {
        public TResult Result { get; set; }
        public ApiOkResponse(TResult result) : base(true)
        {
            Result = result;
        }
    }

    public sealed class ApiCreatedAtResponse<TResult> : ApiBaseResponse
    {
        public TResult Result { get; set; }
        public ApiCreatedAtResponse(TResult result) : base(true)
        {
            Result = result;
        }
    }
    public sealed class ApiNoContentResponse : ApiBaseResponse
    {
        
        public ApiNoContentResponse() : base(true)
        {

        }
    }

    public abstract class ApiNotFoundResponse : ApiBaseResponse
    {
        public string Message { get; set; }
        public ApiNotFoundResponse(string message) : base(false)
        {
            Message = message;
        }
    }
   
    public abstract class ApiBadRequestResponse : ApiBaseResponse
    {
        public string Message { get; set; }
        public ApiBadRequestResponse(string message) : base(false)
        {
            Message = message;
        }
    }

    public abstract class ApiModificationErrorResponse : ApiBaseResponse
    {
        public string Message { get; set; }
        public ApiModificationErrorResponse(string message) : base(false)
        {
            Message = message;
        }
    }

    public class BadCourseRequestResponse : ApiBadRequestResponse
    {

        public BadCourseRequestResponse(string message) : base(message)
        {

        }
    }

    public class BadModuleRequestResponse : ApiBadRequestResponse
    {

        public BadModuleRequestResponse(string message) : base(message)
        {

        }
    }

    public class BadPatchRequestResponse : ApiBadRequestResponse
    {
        public BadPatchRequestResponse(string? message) : base(message ="Patch document could not be read") { }
    }

    public class DuplicateActivityTypeResponse : ApiBadRequestResponse
    {
        public DuplicateActivityTypeResponse(string name) : base($"There is already an activiy type named {name}")
        {

        }
    }
}

