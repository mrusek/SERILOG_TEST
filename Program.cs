
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, services, configuration) => configuration
.ReadFrom.Configuration(context.Configuration)
.ReadFrom.Services(services)
.Enrich.FromLogContext()
);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
        diagnosticContext.Set("Username", httpContext.User.Identity?.Name ?? "Anonymous");
    };
});
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
