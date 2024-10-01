
using EngGenius.Database;
using Microsoft.EntityFrameworkCore;

namespace EngGenius.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Connect to SQL Server
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddControllers();

            // Add Swagger UI
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add CORS policies
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicies",
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod();
                    });
            });

            var app = builder.Build();
            app.UseCors("CorsPolicies");

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
