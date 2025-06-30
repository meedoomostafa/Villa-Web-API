using Microsoft.EntityFrameworkCore;
using VillaRepository.Data;
using VillaRepository.Repository;
using VillaRepository.Repository.Interfaces;
using VillaWebApi;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CS"));
});
builder.Services.AddAutoMapper(typeof(MappingConfig));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("AllowMVC",
//         builder =>
//         {
//             builder.WithOrigins("http://localhost:5282")
//                 .AllowAnyHeader()
//                 .AllowAnyMethod()
//                 .AllowCredentials();
//         });
// });
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseCors("AllowMVC");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
