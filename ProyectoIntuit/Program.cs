using Microsoft.EntityFrameworkCore;
using ProyectoIntuit.Context;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)   // lee del appsettings.json
        .ReadFrom.Services(services)                    // permite usar DI
        .Enrich.FromLogContext()                        // agrega info de contexto
        .WriteTo.Console()                              // muestra en consola
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day); // archivo diario
});

//crear variable para la cadena de conexion
var conecctionString = builder.Configuration.GetConnectionString("Conecction");

//registrar servicio para la conexion 
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(conecctionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
