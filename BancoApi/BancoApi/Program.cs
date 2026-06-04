using BancoApi.Services;
using BancoApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Oracle.ManagedDataAccess.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

builder.Services.AddScoped<service_JWT>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddScoped<Repository_Cliente>();
builder.Services.AddScoped<Repository_Tarjeta>();
builder.Services.AddScoped<Repository_Cuenta>();
builder.Services.AddScoped<Repository_Cajero>();
builder.Services.AddScoped<Repository_Sucursal>();
builder.Services.AddScoped<Repository_Admin>();
builder.Services.AddScoped<Repository_Autenticacion>();
builder.Services.AddScoped<Repository_Abono>();
builder.Services.AddScoped<Repository_Cuota>();
builder.Services.AddScoped<Repository_Transaccion>();
builder.Services.AddScoped<Repository_Prestamo>();

builder.Services.AddScoped<service_Cliente>();
builder.Services.AddScoped<service_Sucursal>();
builder.Services.AddScoped<service_Cajero>();
builder.Services.AddScoped<service_Cuenta>();
builder.Services.AddScoped<service_Autenticacion>();
builder.Services.AddScoped<service_Admin>();
builder.Services.AddScoped<service_Tarjeta>();
builder.Services.AddScoped<service_Abono>();
builder.Services.AddScoped<service_Cuota>();
builder.Services.AddScoped<service_Transaccion>();
builder.Services.AddScoped<service_Prestamo>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Habilitar Swagger en todos los ambientes para documentación de la API
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banco API v1");
    c.RoutePrefix = "swagger";
});

// Comentar HTTPS redirect en contenedores (causa problemas)
// app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
