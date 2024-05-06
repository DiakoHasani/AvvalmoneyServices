using AS.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Results;

namespace AS.WithdrawApi.ErrorHandling
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private readonly ILogger _logger;
        public GlobalExceptionHandler(ILogger logger)
        {
            _logger = logger;
        }
        public override Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            var st = new StackTrace(context.Exception, true);
            if (st.GetFrames().Length != 0)
            {
                _logger.Error("Get Exception in GlobalExceptionHandler", context.Exception, GetFileName(st), GetLineNumber(st), GetMethodName(st));
            }
            else
            {
                _logger.Error("Get Exception in GlobalExceptionHandler", context.Exception);
            }

            const string errorMessage = "An unexpected error occured";
            var response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                new
                {
                    Message = errorMessage
                });
            response.Headers.Add("X-Error", errorMessage);
            context.Result = new ResponseMessageResult(response);

            return base.HandleAsync(context, cancellationToken);
        }

        private string GetFileName(StackTrace st)
        {
            return st.GetFrames().Select(frame => frame.GetFileName()).FirstOrDefault();
        }

        private int GetLineNumber(StackTrace st)
        {
            return st.GetFrames().Select(frame => frame.GetFileLineNumber()).FirstOrDefault();
        }

        private string GetMethodName(StackTrace st)
        {
            return st.GetFrames().Select(frame => frame.GetMethod().Name).FirstOrDefault();
        }
    }
}