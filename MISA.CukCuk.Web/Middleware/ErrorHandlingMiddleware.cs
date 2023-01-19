using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MISA.ApplicationCore.Entities;
using MISA.ApplicationCore.Enums;
using Newtonsoft.Json;

namespace MISA.CukCuk.Web.Middleware
{
    /// <summary>
    /// Xử lý khi có exception xảy ra
    /// </summary>
    /// Author: HHDang (4/8/2021)
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                await HanlderExceptionAsync(context, ex);
            }
        }

        public static Task HanlderExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            var result = JsonConvert.SerializeObject(
                new ServiceResult
                {
                    Data = ex,
                    Messenger = MISA.ApplicationCore.Properties.Resources.ErrorException,
                    MISACode = MISACode.Exception
                });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
