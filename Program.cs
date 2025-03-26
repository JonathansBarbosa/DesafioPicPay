
using PicPaySimplificado.Application;
using PicPaySimplificado.Controllers;
using PicPaySimplificado.Infrastructure.Persistence;
using PicPaySimplificado.Application.Interfaces;
using PicPaySimplificado.Application.Services;
using PicPaySimplificado.Infrastructure.Services;
using Microsoft.EntityFrameworkCore; // Importante para o UseSqlServer
using Pomelo.EntityFrameworkCore.MySql;

namespace PicPaySimplificado
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection"); // Obtem a string de conexão
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 34)); // Ou ServerVersion.AutoDetect(connectionString)

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(connectionString, serverVersion));
                        

            // Use o seu provedor de banco de dados e string de conexão aqui!

            // Registrando os serviços (CORRETO AGORA!)
            builder.Services.AddScoped<ITransacaoService, TransacaoService>();
            builder.Services.AddScoped<IAutorizadorService, AutorizadorServiceMock>();
            builder.Services.AddScoped<INotificadorService, NotificadorServiceMock>();

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
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>();
            services.AddScoped<TransacoesController>();
            services.AddControllers();
            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PicPay API v1"));

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
