using EFCore.Arvato.Context;
using EFCore.Arvato.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using EFCore.Arvato.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using EFCore.Arvato.Core.Orders;
using EFCore.Arvato.Services.Orders;
using EFCore.Arvato.Core.RabbitMq;
using EFCore.Arvato.Services.RabbitMq;


var builder = WebApplication.CreateBuilder(args);





builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IOrderServices, OrderService>();
builder.Services.AddScoped<IOrderCommentService, OrderCommentService>();
builder.Services.AddScoped<IRabbitMQServices,RabbitMqServices>();
builder.Services.AddScoped<MyDbContext, MyDbContext>();


builder.Services.AddIdentityCore<ViewUser>().AddEntityFrameworkStores<MyDbContext>().AddApiEndpoints();

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
}
);
builder.Services.AddAuthorization();
//builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme).AddBearerToken(IdentityConstants.BearerScheme);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
    };
});

builder.Services.AddSwaggerGen(
    op =>
    {
        op.AddSecurityDefinition("outh2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            In=ParameterLocation.Header,
            Name="Authorization",
            Type=SecuritySchemeType.ApiKey
        });

        op.OperationFilter<SecurityRequirementsOperationFilter>();
    });

var app = builder.Build();




if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<ViewUser>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();


app.MapControllers();

app.Run();
