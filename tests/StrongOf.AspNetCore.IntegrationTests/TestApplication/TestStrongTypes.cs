// Copyright (c) BEN ABT (https://benjamin-abt.com) - all rights reserved

namespace StrongOf.AspNetCore.IntegrationTests.TestApplication;

public sealed class TestUserId(Guid value) : StrongGuid<TestUserId>(value) { }

public sealed class TestDuration(TimeSpan value) : StrongTimeSpan<TestDuration>(value) { }

public sealed class TestEmailAddress(string value) : StrongString<TestEmailAddress>(value), IValidatable
{
    public bool IsValidFormat() => Value.Contains('@', StringComparison.Ordinal);
}
