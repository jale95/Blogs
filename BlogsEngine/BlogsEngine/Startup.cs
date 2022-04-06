
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using BlogsEngine.Core.Blogs;
using BlogsEngine.Core.Interfaces;
using BlogsEngine.Core.Users;
using Microsoft.Extensions.Logging;
using BlogsEngine.DAL.Interfaces;
using BlogsEngine.DAL;
using Microsoft.Extensions.Logging.Console;

[assembly: FunctionsStartup(typeof(BlogsEngine.Startup))]

namespace BlogsEngine
{
    

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<ILoggerProvider, ConsoleLoggerProvider>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IBlogsService, BlogsService>();
            builder.Services.AddScoped<IBlogsRepository, BlogsSQLRepository>();
            builder.Services.AddScoped<IUsersRepository, UsersInMemoryRepository>();


        }

    }
}
