using Application.Commons.CQRS.Command;
using Application.Commons.CQRS.Queries;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

            var assembly = typeof(InfrastructureIoC).Assembly.GetType().Assembly;
            
            services.Scan(scan => scan.FromAssemblyDependencies(assembly)
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
