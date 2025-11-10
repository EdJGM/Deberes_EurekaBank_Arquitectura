using Eurekabank_Restfull_Dotnet.Services.Imp;
using Eurekabank_Restfull_Dotnet.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Eurekabank REST API",
        Version = "v1",
        Description = "API RESTful para operaciones bancarias Eurekabank"
    });
});

// Registrar servicios de negocio
builder.Services.AddScoped<IEurekaService, EurekaService>();
builder.Services.AddScoped<ILoginService, LoginService>();

// Configurar CORS (acceso desde web)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
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
