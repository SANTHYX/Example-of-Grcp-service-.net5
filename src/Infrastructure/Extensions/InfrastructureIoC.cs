using Application.Commons.CQRS.Command;
using Application.Commons.CQRS.Queries;
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
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<ICommand>())
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<ICommandDispatcher>())
                    .AsMatchingInterface()
                    .WithSingletonLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IQuery<>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                .AddClasses(classes => classes.AssignableTo<IQueryDispatcher>())
                    .AsMatchingInterface()
                    .WithSingletonLifetime());
        }
    }
}
