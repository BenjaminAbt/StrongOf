// Copyright (c) BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using StrongOf.AspNetCore.MinimalApis;
using StrongOf.AspNetCore.Mvc;
using Xunit;

namespace StrongOf.AspNetCore.IntegrationTests.TestApplication;

public static class TestHostFactory
{
    public static async Task<TestHostContext> CreateAsync(CancellationToken cancellationToken)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.WebHost.UseTestServer();

        builder.Services.AddControllers(options =>
        {
            options.AddStrongOfModelBinderProvider(typeof(TestUserId), typeof(TestEmailAddress), typeof(TestDuration));
        });

        WebApplication app = builder.Build();

        app.MapControllers();
        app.MapGet("/minimal/validate", (TestEmailAddress email) => Results.Text(email.Value)).WithStrongOfValidation();

        await app.StartAsync(cancellationToken).ConfigureAwait(false);

        HttpClient client = app.GetTestClient();
        return new TestHostContext(app, client);
    }
}

public sealed class TestHostContext(WebApplication app, HttpClient client) : IAsyncDisposable
{
    public HttpClient Client { get; } = client;

    public async ValueTask DisposeAsync()
    {
        Client.Dispose();
        await app.StopAsync(TestContext.Current.CancellationToken).ConfigureAwait(false);
        await app.DisposeAsync().ConfigureAwait(false);
    }
}
