using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ActionFilters
{
    public class ResponsHeaderFilterFactoryAttribut : Attribute, IFilterFactory
    {
        public bool IsReusable => false;
        private readonly string? Key;
        private readonly string? Value;
        private readonly int Order;
       
        public ResponsHeaderFilterFactoryAttribut(string key, string value, int order)
        {
            Key = key;
            Value = value;
            Order = order;
        }
        //Controller -> FilterFactory -> Filter
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            
            var filter = serviceProvider.GetRequiredService<ResponsHeaderActionFilter>();
            filter.Key = Key;
            filter.Value = Value;
            filter.Order = Order; 
            //return filter object 
            return filter; 
        }
    }
    public class ResponsHeaderActionFilter : IAsyncActionFilter, IOrderedFilter
    //ActionFilterAttribute
    //IAsyncActionFilter ,IOrderedFilter
    {
        
       public string Key;
       public string Value;
        public int Order { get; set; }
        private readonly ILogger<ResponsHeaderActionFilter> _logger;
        public ResponsHeaderActionFilter(ILogger<ResponsHeaderActionFilter> logger)
        {
            _logger = logger; 
        }

        

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // before 
            context.HttpContext.Response.Headers[Key] = Value;
            await next(); //calls the subsequent filter or action method 
            //after 
        }
    }
}
