namespace SearchSuggestions.WebAPI.Middleware
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Serilog;

    /// <summary>
    /// Middleware filter to catch all uncaught exceptions
    /// </summary>
    internal class ExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// The current environment
        /// </summary>
        private readonly IHostingEnvironment environment;

        /// <summary>
        /// Initializes the default instance of the <see cref="ExceptionFilter" /> class
        /// </summary>
        /// <param name="env">The current hosting environment</param>
        public ExceptionFilter(IHostingEnvironment env)
        {
            this.environment = env;
        }

        /// <summary>
        /// Method called when handling exceptions
        /// </summary>
        /// <param name="context">The current exception context</param>
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