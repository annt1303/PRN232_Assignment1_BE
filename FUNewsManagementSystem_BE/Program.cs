using BLL.ServiceImp;
using BLL.ServiceInterface;
using DAL.Models;
using DAL.Repositories.Implement;
using DAL.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

// 1. Đăng ký DbContext đúng cách
builder.Services.AddDbContext<FUNewsManagementContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Kiểm tra kết nối và log thông tin
var testConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());
var logger = loggerFactory.CreateLogger("Startup");

try
{
    var options = new DbContextOptionsBuilder<FUNewsManagementContext>()
        .UseSqlServer(testConnectionString)
        .Options;

    using var context = new FUNewsManagementContext(options);
    if (context.Database.CanConnect())
    {
        logger.LogInformation("✅ Connected successfully to the database.");
    }
    else
    {
        logger.LogWarning("⚠️ Cannot connect to the database.");
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "❌ Connection failed!");
    logger.LogInformation("Connection string used: {cs}", testConnectionString);
}

// 3. Cấu hình OData
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<SystemAccount>("Accounts");
odataBuilder.EntitySet<Category>("Categories");
odataBuilder.EntitySet<NewsArticle>("NewsArticles");
odataBuilder.EntitySet<Tag>("Tags");

// 4. Đăng ký Repository
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();

// 5. Đăng ký Service
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
builder.Services.AddScoped<ITagService, TagService>();

// 6. Controller & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 7. Middlewares
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
