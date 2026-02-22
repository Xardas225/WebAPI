using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI.Data;
using WebAPI.Repositories;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services;
using WebAPI.Services.Interfaces;
using WebAPI.Models.User.Enums;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

builder.Services.AddScoped<IChefsService, ChefsService>();
builder.Services.AddScoped<IChefsRepository, ChefsRepository>();

builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IStorageRepository, StorageRepository>();

builder.Services.AddScoped<IDishesService, DishesService>();
builder.Services.AddScoped<IDishesRepository, DishesRepository>();

builder.Services.AddScoped<IIngredientsService, IngredientsService>();
builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();

builder.Services.AddScoped<IKitchensRepository, KitchensRepository>();
builder.Services.AddScoped<IKitchensService, KitchensService>();

builder.Services.AddScoped<ICategoriesRepository, CategoriesRepository>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();


builder.Services.AddHttpClient<IVisionServiceClient, VisionServiceClient>(client =>
{
    var baseUrl = builder.Configuration["VisionService:Url"] ?? "http://localhost:8000";
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// ========== ПОДКЛЮЧЕНИЕ К MYSQL ==========
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
try
{   
    // Регистрируем DbContext с MySQL
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseMySql(connectionString,
            ServerVersion.AutoDetect(connectionString), // Автоопределение версии MySQL
            mysqlOptions =>
            {
                mysqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });

        // Включить детальное логирование только для разработки
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging()
                   .EnableDetailedErrors()
                   .LogTo(Console.WriteLine, LogLevel.Information);
        }
    });

    Console.WriteLine("✅ DbContext успешно зарегистрирован с MySQL");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ ОШИБКА при настройке DbContext: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
    }
    return;
}


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173",
                    "http://127.0.0.1:5173",
                    "https://localhost:5173",
                    "https://127.0.0.1:5173")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

 
var redisConnection = builder.Configuration.GetConnectionString("Redis");
if (string.IsNullOrEmpty(redisConnection))
{
    throw new InvalidOperationException("Redis connection string is not configured.");
}

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});


// Настройка JWT аутентификации
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],

        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],

        ValidateLifetime = true, // Проверяет срок действия токена

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
    };
});


// Добавляем политики авторизации
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.RequireRole(UserRole.Admin.ToString()));

    options.AddPolicy("Chef", policy =>
        policy.RequireRole(UserRole.Chef.ToString()));

    options.AddPolicy("User", policy =>
        policy.RequireRole(UserRole.User.ToString()));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
   
app.UseAuthorization();

app.MapControllers();

app.UseDeveloperExceptionPage();

// ========== ПРОВЕРКА ПОДКЛЮЧЕНИЯ К БАЗЕ ДАННЫХ ==========

Console.WriteLine("🔍 Проверка подключения к базе данных...");

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Пробуем подключиться
        var canConnect = await context.Database.CanConnectAsync();

        if (canConnect)
        {
            Console.WriteLine("✅ Подключение к базе данных успешно!");

            // Применяем миграции
            Console.WriteLine("🔄 Применение миграций...");
            await context.Database.MigrateAsync();
            Console.WriteLine("✅ Миграции успешно применены");

            // Выводим информацию о БД
            var dbConnection = context.Database.GetDbConnection();
            Console.WriteLine($"📊 Информация о базе данных:");
            Console.WriteLine($"   Сервер: {dbConnection.DataSource}");
            Console.WriteLine($"   База данных: {dbConnection.Database}");
            Console.WriteLine($"   Состояние: {dbConnection.State}");
        }
        else
        {
            Console.WriteLine("❌ Не удалось подключиться к базе данных");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ ОШИБКА при подключении к базе данных: {ex.Message}");
        Console.WriteLine("Проверьте:");
        Console.WriteLine("1. Запущен ли MySQL сервер");
        Console.WriteLine("2. Правильность строки подключения");
        Console.WriteLine("3. Существует ли база данных 'smartgymdb'");
        Console.WriteLine("4. Правильность логина и пароля");

        if (ex.InnerException != null)
        {
            Console.WriteLine($"Подробности: {ex.InnerException.Message}");
        }

        // Можно продолжить работу без БД или завершить
         return;
    }
}

app.Run();
