// Copyright (c) BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.AspNetCore.IntegrationTests.TestApplication;

[StrongGuid]
public sealed partial class TestUserId { }

[StrongTimeSpan]
public sealed partial class TestDuration { }

[StrongString]
public sealed partial class TestEmailAddress : IValidatable
{
    public bool IsValidFormat() => Value.Contains('@', StringComparison.Ordinal);
}
