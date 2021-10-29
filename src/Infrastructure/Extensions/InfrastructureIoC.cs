using Application.Commons.CQRS.Command;
using Application.Commons.CQRS.Queries;
using Application.Commons.Persistance;
using Infrastructure.CQRS.Command;
using Infrastructure.CQRS.Queries;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class InfrastructureIoC
    {
        public static void InjectInfrastructureIoC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(opt => 
            { 
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"), 
                    npgCfg => npgCfg.MigrationsAssembly("Infrastructure"));
                opt.EnableDetailedErrors();
            });
            
            services.Scan(scan => scan.FromApplicationDependencies()
                .AddClasses(classes =>classes.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime());
            services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
            services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
            services.AddSingleton<IDataContext, DataContext>();
        }
    }
}
