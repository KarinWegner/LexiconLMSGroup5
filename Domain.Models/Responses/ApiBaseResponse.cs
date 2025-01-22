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
    //public abstract class ApiInternalServerErrorResponse : ApiBaseResponse
    //{
    //    public string Message { get; set; }
    //    public ApiInternalServerErrorResponse(string message) : base(false)
    //    {
    //        Message = message;
    //    }
    //}
    public abstract class ApiBadRequestResponse : ApiBaseResponse
    {
        public string Message { get; set; }
        public ApiBadRequestResponse(string message) : base(false)
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

    

    public class CourseNotFoundResponse : ApiNotFoundResponse
    {
        public CourseNotFoundResponse(int id) : base($"Course with id {id} was not found")
        {
        }
    }
    public class ModuleNotFoundResponse : ApiNotFoundResponse
    {
        public ModuleNotFoundResponse(int id) : base($"Module with id {id} was not found")
        {
        }
    }
    public class ActivityNotFoundResponse : ApiNotFoundResponse
    {
        public ActivityNotFoundResponse(int id) : base($"Activity with id {id} was not found")
        {
        }
    }
    public class ActivityTypeNotFoundResponse : ApiNotFoundResponse
    {
        public ActivityTypeNotFoundResponse(int id) : base($"The Activity Type with id {id} was not found")
        {

        }
    }
    public class UserNotFoundResponse : ApiNotFoundResponse
    {
        public UserNotFoundResponse(string id) : base($"The User with id {id} was not found")
        {

        }
    }
   
    public class BadDateSequenceResponse : ApiBadRequestResponse
    {
        public BadDateSequenceResponse(DateTime startDate, DateTime endDate) : base($"Item end date{endDate} is before its start date {startDate}.") 
        {

        }
    }
    public class BadDateTimeFrameBreakResponse : ApiBadRequestResponse 
    {        
        public BadDateTimeFrameBreakResponse() : base("Item is scheduled outside allowed timeframe") 
        {
            
        }
    }
    public class BadDateOverlapResponse : ApiBadRequestResponse
    {
        public BadDateOverlapResponse() : base("Another item is scheduled during entered timespan") { }
    }
}

