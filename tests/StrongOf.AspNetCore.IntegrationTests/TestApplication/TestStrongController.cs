// Copyright (c) BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc;

namespace StrongOf.AspNetCore.IntegrationTests.TestApplication;

[ApiController]
[Route("mvc")]
public sealed class TestStrongController : ControllerBase
{
    [HttpGet("bind/{id:guid}")]
    public ActionResult<string> Bind(
        [FromRoute] TestUserId id,
        [FromQuery] TestEmailAddress email,
        [FromQuery] TestDuration duration)
    {
        return Ok($"{id.Value}|{email.Value}|{duration.Value:c}");
    }
}
