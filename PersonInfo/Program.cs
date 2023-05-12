using Microsoft.Extensions.Configuration;
using PersonInfo.Models;

namespace PersonInfo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSingleton<IUserRepository, UserRepository>(serviceProvider =>
            new UserRepository("Server=192.168.137.155;Database=testdb;User ID=pavlo;PASSWORD=12345678Aa; TrustServerCertificate=True;"));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddMvc(options =>
            {
                options.SuppressAsyncSuffixInActionNames = false;
            });

            builder.Logging.AddLog4Net(new Log4NetProviderOptions
            {
                Log4NetConfigFileName = "log4net.config"
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}