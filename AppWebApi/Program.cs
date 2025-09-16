using AppRepository.Data;
using AppRepository.Repository.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppService.Extenstions;
using VillaWebApi.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddMappings();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CS"));
});
builder.Services.AddResponseCaching();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("30s", new CacheProfile()
    {
        Duration = 30
    });
}).AddNewtonsoftJson();

builder.Services.AddUnitOfWorkRepository();
builder.Services.AddServices();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();