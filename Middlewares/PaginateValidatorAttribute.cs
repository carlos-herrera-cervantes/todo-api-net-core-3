using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;
using TodoApiNet.Models;

namespace TodoApiNet.Middlewares
{
    public class PaginateValidatorAttribute : TypeFilterAttribute
    {
        public PaginateValidatorAttribute() : base(typeof(PaginateValidatorFilter)) { }

        private class PaginateValidatorFilter : IAsyncActionFilter
        {
            private readonly IStringLocalizer<SharedResources> _localizer;

            public PaginateValidatorFilter(IStringLocalizer<SharedResources> localizer) => _localizer = localizer;

            #region snippet_BeforeExecuted

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var paginate = context.ActionArguments["querys"] as Request;
                var isValidPagination = paginate.Page < 1 && paginate.PageSize > 0 && paginate.PageSize < 11;
                
                if (paginate.Paginate && isValidPagination)
                {
                    context.Result = new BadRequestObjectResult(new { messag = _localizer["InvalidPagination"].Value });
                    return;
                }

                await next();
            }

            #endregion
        }
    }
}