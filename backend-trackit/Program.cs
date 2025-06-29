using backend_trackit.Context;
using backend_trackit.Helper;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<PaketContext>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("ConnectionStrings");
    return new PaketContext(connStr);
});
builder.Services.AddScoped<CustomerContext>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("ConnectionStrings");
    return new CustomerContext(connStr);
});
builder.Services.AddScoped<KurirContext>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var connStr = config.GetConnectionString("ConnectionStrings");
    return new KurirContext(connStr);
});

// Tambahkan ini setelah builder.Services yang lain
builder.Services.AddScoped<ICloudinaryHelper, CloudinaryHelper>();

// Configure file upload limits
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
