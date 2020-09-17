using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Heavy.Web.Filters
{
    public class LogAsyncResourceFilter : Attribute, IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            Console.WriteLine("Executing async Resource Filter!");
            var exectedContext = await next();
            Console.WriteLine("Executed async Resource Filter...");
        }
    }
}
