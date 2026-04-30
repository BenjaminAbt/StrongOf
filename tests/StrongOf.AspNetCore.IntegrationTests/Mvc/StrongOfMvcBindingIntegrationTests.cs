// Copyright (c) BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Net;
using StrongOf.AspNetCore.IntegrationTests.TestApplication;
using Xunit;

namespace StrongOf.AspNetCore.IntegrationTests.Mvc;

public sealed class StrongOfMvcBindingIntegrationTests
{
    [Fact]
    public async Task Get_Bind_WithValidStrongTypes_ReturnsBoundPrimitiveValues()
    {
        await using TestHostContext context = await TestHostFactory.CreateAsync(TestContext.Current.CancellationToken);
        HttpClient client = context.Client;
        Guid id = Guid.NewGuid();

        HttpResponseMessage response = await client.GetAsync(
            $"/mvc/bind/{id}?email=user%40example.com&duration=00:05:00",
            TestContext.Current.CancellationToken);

        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal($"{id}|user@example.com|00:05:00", responseBody);
    }
}
