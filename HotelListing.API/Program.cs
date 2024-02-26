using HotelListing.API.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//for database
var connectionString = builder.Configuration.GetConnectionString("HotelListingDbConnectionString");

//AddDbContext - connection to the db. Object that will run and live as a connection as a bridge to db.
//we need to have class file outlined that is some form of db context i.e.HotelListingDbContext
// options.UseSqlServer ->  since we are using sql we are mentioning it. If we use some other db, then we have to mention that.

builder.Services.AddDbContext<HotelListingDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", 
        b => b.AllowAnyHeader()
            .AllowAnyOrigin()
            .AllowAnyMethod());
});

//SERILOG
//ctx - context variable
//lc - logger configuration
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

//CORS
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
