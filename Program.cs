using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

//Configure AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddNpgsql<ShepherdsPieDbContext>(builder.Configuration["ShepherdsPieDbContextString"]);

//dotnet user-secrets set ShepherdsPieDbContextString "Host=localhost;Port=5432;Username=postgres;Password=<your password>;Database=ShepherdsPie"

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();