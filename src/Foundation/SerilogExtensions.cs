using Microsoft.VisualBasic;
using Serilog;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Foundation
{
    public static class SerilogExtensions
    {
        private static readonly IDictionary<string, bool> LoggedMessages = new Dictionary<string, bool>();

        public static void ErrorWithLocation(this ILogger logger, string message, object obj, [CallerMemberName] string memberName = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var codeLocationStr = string.Empty;
            try
            {
                var objType = obj?.GetType();
                var typeName = objType?.Name;
                var objNameSpace = objType?.Namespace;
                codeLocationStr = string.Concat(objNameSpace, ".", typeName, ".", memberName, " line number: ", sourceLineNumber);

                logger.Write(Serilog.Events.LogEventLevel.Information, string.Concat(message, " ", codeLocationStr));
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        public static void ErrorOnce(this ILogger logger, string message)
        {
            var codeLocationStr = string.Empty;
            try
            {
                if (!LoggedMessages.ContainsKey(message))
                {
                    logger.Write(Serilog.Events.LogEventLevel.Error, string.Concat(Constants.ErrorMessages.Once, " ", message, " ", codeLocationStr));
                    LoggedMessages[message] = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
    }
}