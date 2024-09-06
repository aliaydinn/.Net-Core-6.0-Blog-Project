using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCoreBlog.Data.Context;
using NetCoreBlog.Data.Repositories.Abstractions;
using NetCoreBlog.Data.Repositories.Concretions;
using NetCoreBlog.Data.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCoreBlog.Data.Extensions
{
    public static class DataLayerExtensions
    {
        public static IServiceCollection LoadDataLayerExtension(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddDbContext<AppDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging());
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
                  
            return services;
        }
    }
}
