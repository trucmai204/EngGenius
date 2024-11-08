
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
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "API của bạn",
                    Version = "v1",
                    Description = "Đây là tài liệu API của tôi"
                });
            });

            // Add CORS policies
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicies",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000") // Cho phép yêu cầu từ localhost:3000
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });


            var app = builder.Build();
            app.UseCors("CorsPolicies");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            });


            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
