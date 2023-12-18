using Microsoft.AspNetCore.Mvc.Filters;

namespace ef6EssencialNetCore.Filters;

    public class ApiLogginFilter : IActionFilter
    {
        private readonly ILogger<ApiLogginFilter> _logger;
        
        public ApiLogginFilter(ILogger<ApiLogginFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("## EXECUTANDO -> OnActionExecuting");
            _logger.LogInformation("##############################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
            _logger.LogInformation("##############################");
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {   
            _logger.LogInformation("## EXECUTANDO -> OnActionExecuted");
            _logger.LogInformation("##############################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation("##############################");
        }

    }
