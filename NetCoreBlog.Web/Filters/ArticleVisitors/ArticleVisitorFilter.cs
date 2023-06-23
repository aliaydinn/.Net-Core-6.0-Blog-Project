using Microsoft.AspNetCore.Mvc.Filters;
using NetCoreBlog.Data.UnitOfWorks;
using NetCoreBlog.Entity.Entities;

namespace NetCoreBlog.Web.Filters.ArticleVisitors
{
    public class ArticleVisitorFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork unitOfWork;

        public ArticleVisitorFilter(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public  Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            List<Visitor> visitors = unitOfWork.GetRepository<Visitor>().GetAllAsync().Result;

            var ıpAddress= context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var userAgent= context.HttpContext.Request.Headers["User-Agent"];

            Visitor visitor = new(ıpAddress, userAgent);

            if (visitors.Any(x=>x.IpAddress==visitor.IpAddress))
            {
                return next();
            }
            else
            {
                 unitOfWork.GetRepository<Visitor>().AddAsync(visitor);
                 unitOfWork.SaveAsync();
            }
            return next();
        }
    }
}
