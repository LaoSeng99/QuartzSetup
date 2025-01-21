

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Quartz;
using QuartzSetup.Data;
using QuartzSetup.Services;
using System.Collections.Specialized;

var builder = WebApplication.CreateBuilder(args);
string connString = builder.Configuration.GetConnectionString("MyCrsConnection")!;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connString,
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
    .EnableSensitiveDataLogging(); //Show param in console

}, ServiceLifetime.Scoped);

//await builder.Services.AddQuartzServicesInMemory();
await builder.Services.AddQuartzServicesInDb(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//Start background job
var jobScheduler = app.Services.GetRequiredService<IJobSchedulerService>();
await jobScheduler.ScheduleBackgroundJob();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
