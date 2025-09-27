using Client.Services;
using Refit;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        builder.Services.AddRefitClient<IServerError>()
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7090"));
        builder.Services.AddRefitClient<IServerSlow>()
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7091"));
        builder.Services.AddRefitClient<IServerTimeout>()
                        .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7092"));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        if(app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Run();
    }
}