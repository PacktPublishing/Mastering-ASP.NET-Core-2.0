using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;

namespace BookShop.WebComponents.Logging
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true,
    Inherited = true)]
    public sealed class LoggerAttribute : ActionFilterAttribute
    {
        public LoggerAttribute(string logMessage)
        {
            this.LogMessage = logMessage;
        }

        public string LogMessage { get; }
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        private EventId _eventId;

        private string GetLogMessage(ModelStateDictionary modelState)
        {
            var logMessage = this.LogMessage;

            foreach (var key in modelState.Keys)
            {
                logMessage = logMessage.Replace("{" + key + "}", modelState[key].RawValue?.ToString());
            }

            return logMessage;
        }

        private ILogger GetLogger(HttpContext context, ControllerActionDescriptor action)
        {
            var logger = context
               .RequestServices
               .GetService(typeof(ILogger<>)
               .MakeGenericType(action.ControllerTypeInfo.UnderlyingSystemType)) as ILogger;
            return logger;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var cad = context.ActionDescriptor as ControllerActionDescriptor;
            var logMessage = this.GetLogMessage(context.ModelState);
            var logger = this.GetLogger(context.HttpContext, cad);
            var duration = TimeSpan.FromMilliseconds(Environment.TickCount - this._eventId.Id);

            logger.Log(this.LogLevel, this._eventId, $"After {cad.ControllerName}.{cad.ActionName} with {logMessage} and result {context.HttpContext.Response.StatusCode} in {duration}", null, (state, ex) => state.ToString());

            base.OnActionExecuted(context);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var cad = context.ActionDescriptor as ControllerActionDescriptor;
            var logMessage = this.GetLogMessage(context.ModelState);
            var logger = this.GetLogger(context.HttpContext, cad);

            this._eventId = new EventId(Environment.TickCount, $"{cad.ControllerName}.{cad.ActionName}");

            logger.Log(this.LogLevel, this._eventId, $"Before {cad.ControllerName}.{cad.ActionName} with {logMessage}", null, (state, ex) => state.ToString());

            base.OnActionExecuting(context);
        }
    }
}
