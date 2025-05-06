using Bike4Me.Domain.Bikes;
using Medallion.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Polly;
using Polly.Retry;

namespace Bike4Me.Infrastructure.Database.Seed;

public class Bike4MeContextSeed : IDbContextSeed<Bike4MeContext>
{
    private static readonly Guid _defaultModelId = Guid.Parse("c5154201-9b1d-e813-90a2-100d3ab14b1a");
    private const string DefaultLockPrefix = "InitSeed";
    private const int DefaultRetryCount = 3;
    private const int DefaultLockTimeoutSeconds = 2;

    public async Task SeedAsync(
        Bike4MeContext context,
        IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<Bike4MeContextSeed>>();
        var lockProvider = serviceProvider.GetRequiredService<IDistributedLockProvider>();

        var policy = CreateRetryPolicy(logger);

        await policy.ExecuteAsync(async () =>
        {
            await using var lockHandle = await AcquireLockAsync(lockProvider, logger);
            if (lockHandle is not null)
            {
                await SeedDefaultBikeAsync(context, logger, cancellationToken);
            }
        });
    }

    private static async Task<IDistributedSynchronizationHandle?> AcquireLockAsync(
        IDistributedLockProvider lockProvider,
        ILogger logger)
    {
        var lockKey = $"{DefaultLockPrefix}_{_defaultModelId}";
        var @lock = lockProvider.CreateLock(lockKey);

        var handle = await @lock.TryAcquireAsync(
            TimeSpan.FromSeconds(DefaultLockTimeoutSeconds));

        if (handle is null)
        {
            logger.LogWarning("Could not acquire lock for seeding");
        }

        return handle;
    }

    private static async Task SeedDefaultBikeAsync(
        Bike4MeContext context,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        if (await context.Bikes.AnyAsync(c => c.ModelId == _defaultModelId, cancellationToken))
        {
            logger.LogInformation("Default bike already exists");
            return;
        }

        var model = CreateDefaultMotorcycleModel();
        var bike = CreateDefaultBike(model.Id);

        await context.BikeModels.AddAsync(model, cancellationToken);
        await context.Bikes.AddAsync(bike, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Successfully seeded default bike");
    }

    private static BikeModel CreateDefaultMotorcycleModel() =>
        BikeModel.Create(
            _defaultModelId,
            new Name("CB 300F Twister Default Model"),
            new Manufacturer("Honda Default"),
            new Year(2025),
            "293,5 cm³ Capacity Default");

    private static Bike CreateDefaultBike(Guid modelId) =>
        Bike.Create(
            Guid.NewGuid(),
            new LicensePlate("ABC-1234"),
            modelId,
            "Blue");

    private static AsyncRetryPolicy CreateRetryPolicy(
        ILogger logger,
        int retries = DefaultRetryCount) =>
        Policy
            .Handle<PostgresException>()
            .WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry =>
                    TimeSpan.FromSeconds(Math.Pow(2, retry)),
                onRetry: (exception, timeSpan, retry, _) =>
                {
                    logger.LogWarning(
                        exception,
                        "Error seeding database (Attempt {Retry} of {Retries})",
                        retry,
                        retries);
                });
}