// Copyright (c) BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Net;
using StrongOf.AspNetCore.IntegrationTests.TestApplication;
using Xunit;

namespace StrongOf.AspNetCore.IntegrationTests.MinimalApis;

public sealed class StrongOfMinimalApiIntegrationTests
{
    [Fact]
    public async Task Get_Validate_WithValidEmail_ReturnsOk()
    {
        await using TestHostContext context = await TestHostFactory.CreateAsync(TestContext.Current.CancellationToken);
        HttpClient client = context.Client;

        HttpResponseMessage response = await client.GetAsync("/minimal/validate?email=user%40example.com", TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("user@example.com", responseBody);
    }
}
