using Microsoft.Data.SqlClient;
using Polly;

namespace ForCreate.Shared.Infrastructure;

public static class ResiliencyPolicy
{
    private static readonly ISet<int> TransientNumbers =
        new HashSet<int>(new[] { 40613, 40197, 40501, 49918, 40549, 40550, 1205 });

    private static readonly ISet<int> NetworkingNumbers =
        new HashSet<int>(new[] { 258, -2, 10060, 10061, 0, 64, 26, 40, 10053 });

    private static readonly ISet<int> ConstraintViolationNumbers = new HashSet<int>(new[] { 2627, 547, 2601 });

    public static IAsyncPolicy GetSqlResiliencyPolicy(
        TimeSpan? maxTimeout = null,
        int transientRetries = 3,
        int networkRetries = 3)
    {
        var timeoutPolicy = Policy.TimeoutAsync(maxTimeout ?? TimeSpan.FromMinutes(2));

        var transientPolicy = Policy.Handle<SqlException>(ex => TransientNumbers.Contains(ex.Number))
            .WaitAndRetryAsync(
                transientRetries,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

        var networkPolicy = Policy.Handle<SqlException>(ex => NetworkingNumbers.Contains(ex.Number))
            .WaitAndRetryAsync(
                networkRetries,
                attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));

        var constraintPolicy = Policy.Handle<SqlException>(ex => ConstraintViolationNumbers.Contains(ex.Number))
            .CircuitBreakerAsync(
                1,
                TimeSpan.MaxValue);

        var resiliencyPolicy = timeoutPolicy
            .WrapAsync(transientPolicy)
            .WrapAsync(networkPolicy)
            .WrapAsync(constraintPolicy);

        return resiliencyPolicy!;
    }
}