namespace SearchSuggestions.WebAPI.Middleware
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Serilog;

    internal class ExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment environment;

        public ExceptionFilter(IHostingEnvironment env)
        {
            this.environment = env;
        }

        public void OnException(ExceptionContext context)
        {
            Log.Logger.Error(context.Exception, "An error occurred.");

            if (this.environment.IsDevelopment())
            {
                // In development we can return the full exception
                context.Result = new ObjectResult(context.Exception)
                {
                    StatusCode = 500
                };
            }
            else
            {
                // In production we don't want to give away too much
                context.Result = new ObjectResult("An error has occurred.")
                {
                    StatusCode = 500
                };
            }
        }
    }
}