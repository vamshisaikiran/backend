using Backend.Data;
using Backend.Middlewares;
using Backend.Repositories.EventRepository;
using Backend.Repositories.ReservationRepository;
using Backend.Repositories.SportRepository;
using Backend.Repositories.StadiumRepository;
using Backend.Repositories.TeamRepository;
using Backend.Repositories.UserRepository;
using Backend.Services.AuthService;
using Backend.Services.EventService;
using Backend.Services.PasswordService;
using Backend.Services.ReservationService;
using Backend.Services.SportService;
using Backend.Services.StadiumService;
using Backend.Services.TeamService;
using Backend.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "USEM", Version = "v1" }); });

builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services
    .AddDbContext<DataContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Data initializer
builder.Services.AddScoped<DataInitializer>();

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStadiumRepository, StadiumRepository>();
builder.Services.AddScoped<ISportRepository, SportRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// Services
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStadiumService, StadiumService>();
builder.Services.AddScoped<ISportService, SportService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

builder.Services.AddScoped<GlobalErrorHandlerMiddleware>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        var dataInitializer = services.GetRequiredService<DataInitializer>();
        dataInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "University Sports Event Management")
    );
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalErrorHandlerMiddleware>();

app.MapControllers();

app.Run();