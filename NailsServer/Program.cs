
using Microsoft.EntityFrameworkCore;
using NailsServer.Hubs;
using NailsServer.Models;
using Microsoft.AspNetCore.SignalR;

namespace NailsServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSignalR();

            #region Add Database context to Dependency Injection
            //Read connection string from app settings.json
            string connectionString = builder.Configuration
                .GetSection("ConnectionStrings")
                .GetSection("NailsDB").Value;

            //Add Database to dependency injection
            builder.Services.AddDbContext<NailsDbContext>(
                    options => options.UseSqlServer(connectionString));

            #endregion 

            #region Add Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = false;
                options.Cookie.IsEssential = true;
            });
            #endregion

            #region for debugging UI
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            #endregion

            var app = builder.Build();

            #region for debugging UI
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            #endregion


            #region Add Session
            app.UseSession(); //In order to enable session management
            #endregion 

            app.UseHttpsRedirection();
            app.UseStaticFiles(); //Support static files delivery from wwwroot folder
            app.MapControllers(); //Map all controllers classes
            app.MapHub<ChatHub>("/chat"); //Map the chatHub to the /chatHub URL
            // Configure the application to listen on all network interfaces
            // Also note the changes in launchSettings.json
            // And you should press "allow" when windows firewall prompte a message after running the server for the first time!
            //app.Urls.Add("http://*:5110");
            //app.Urls.Add("https://*:7012");

            app.Run();
        }
    }
}
