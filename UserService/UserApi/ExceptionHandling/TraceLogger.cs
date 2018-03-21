using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace UserApi.ExceptionHandling
{
    public class TraceExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            Trace.TraceError(context.Exception.ToString());
        }
    }
}