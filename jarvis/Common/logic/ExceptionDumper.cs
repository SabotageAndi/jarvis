using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace jarvis.common.logic
{
    public class ExceptionDumper
    {
        public static string Write(Exception exception)
        {
            if (exception == null)
                return String.Empty;

            if (exception is WebException)
            {
                return Write((WebException)exception);
            }

            return exception.ToString();
        }

        private static string Write(WebException webException)
        {
            return String.Format("Message: {4}\r\nStatus: {0}\r\nResponse: {1}\r\nInner Exception: {2}\r\nCall Stack: {3}", 
                webException.Status, 
                webException.Response, 
                Write(webException.InnerException), 
                webException.StackTrace,
                webException.Message);
        }
    }
}
