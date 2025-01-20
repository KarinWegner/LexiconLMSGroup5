using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LMS.API.Extensions
{
    public static class ExceptionMiddleWare
    {
        public static void ConfigureExceptionHandler(this WebApplication app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {

                    var contextFeatures = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeatures != null)
                    {
                        var problemDetailsFactory = app.Services.GetService<ProblemDetailsFactory>();
                        //Validate

                        var problemDetails = CreateProblemDetails(context, contextFeatures.Error, problemDetailsFactory, app);

                        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
                        //context.Response.ContentType= ""
                        await context.Response.WriteAsJsonAsync(problemDetails);
                    }

                });
            });
        }
        private static ProblemDetails CreateProblemDetails(HttpContext context, Exception error, ProblemDetailsFactory? problemDetailsFactory, WebApplication app)
        {
            return error switch
            {
                CourseNotFoundException courseNotFoundException => problemDetailsFactory.CreateProblemDetails(
                    context,
                    StatusCodes.Status404NotFound,
                    title: courseNotFoundException.Title,
                    detail: courseNotFoundException.Message,
                    instance: context.Request.Path),

                ModuleNotFoundException moduleNotFoundException => problemDetailsFactory.CreateProblemDetails(
                context,
                StatusCodes.Status404NotFound,
                title: moduleNotFoundException.Title,
                detail: moduleNotFoundException.Message,
                instance: context.Request.Path),

                ActivityNotFoundException activityNotFoundException => problemDetailsFactory.CreateProblemDetails(
                    context,
                    StatusCodes.Status404NotFound,
                    title: activityNotFoundException.Title,
                    detail: activityNotFoundException.Message,
                    instance: context.Request.Path),

                _ => problemDetailsFactory.CreateProblemDetails(
                 context,
                 StatusCodes.Status500InternalServerError,
                 title: "Internal Server Error",
                 detail: app.Environment.IsDevelopment() ? error.Message : "An unexpected error occurred.")
            };

        }
    }
}
