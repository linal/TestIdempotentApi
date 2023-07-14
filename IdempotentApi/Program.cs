using IdempotentAPI.Cache.FusionCache.Extensions.DependencyInjection;
using IdempotentAPI.Extensions.DependencyInjection;

namespace IdempotentApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddIdempotentAPI();

            // Register an implementation of the IDistributedCache.
            // For this example, we are using Redis.
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = "redis:6379";
            });

            // Register the FusionCache Serialization (e.g. NewtonsoftJson).
            // This is needed for the a 2nd-level cache.
            builder.Services.AddFusionCacheNewtonsoftJsonSerializer();

            // Register the IdempotentAPI.Cache.FusionCache.
            // Optionally: Configure the FusionCacheEntryOptions.
            builder.Services.AddIdempotentAPIUsingFusionCache();

            builder.Services.AddControllers();
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
        }
    }
}