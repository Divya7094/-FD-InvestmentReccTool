using WisVestAPI.Repositories.Matrix;
using WisVestAPI.Services;
using WisVestAPI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<UserService>();
builder.Services.AddScoped<IUserInputService, UserInputService>();
builder.Services.AddScoped<IAllocationService, AllocationService>();
builder.Services.AddScoped<MatrixRepository>(provider =>
    new MatrixRepository(provider.GetRequiredService<IConfiguration>()["MatrixFilePath"]));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Allow requests from this origin
              .AllowAnyHeader() // Allow any headers
              .AllowAnyMethod(); // Allow any HTTP methods
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

// Enable CORS
app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();