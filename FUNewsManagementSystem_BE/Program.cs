﻿using BLL.ServiceImp;
using BLL.ServiceInterface;
using DAL.Models;
using DAL.Repositories.Implement;
using DAL.Repositories.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký DbContext đúng cách
builder.Services.AddDbContext<FUNewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<SystemAccount>("Accounts");
odataBuilder.EntitySet<Category>("Categories");
odataBuilder.EntitySet<NewsArticle>("NewsArticles");
odataBuilder.EntitySet<Tag>("Tags");

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddSingleton<JwtHelper>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// 7. JWT setting 
// Get JWT settings
var jwtConfig = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtConfig["SecretKey"];
var key = Encoding.UTF8.GetBytes(secretKey!);

// Add JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtConfig["Issuer"],
			ValidAudience = jwtConfig["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(key)
		};
	});



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseCors("AllowAll");


app.MapControllers();

app.Run();
