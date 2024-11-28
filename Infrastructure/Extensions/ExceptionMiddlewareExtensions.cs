﻿using System.Net;
using Application.Contracts;
using Application.Validation;
using Domain.Entities.ErrorModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILoggerManager logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var exception = contextFeature.Error;

                    int statusCode;
                    string message;

                    switch (exception)
                    {
                        case NotFoundException notFoundException:
                            statusCode = (int)HttpStatusCode.NotFound;
                            message = notFoundException.Message;
                            break;
                        case ConflictException conflictException:
                            statusCode = (int)HttpStatusCode.Conflict;
                            message = conflictException.Message;
                            break;
                        default:
                            statusCode = (int)HttpStatusCode.InternalServerError;
                            message = "Internal Server Error.";
                            break;
                    }

                    logger.LogError($"Something went wrong: {exception}");

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = statusCode,
                        Message = message
                    }.ToString());
                }
            });
        });
    }
}
