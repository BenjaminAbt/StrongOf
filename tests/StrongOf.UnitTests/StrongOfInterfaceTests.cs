// Copyright © Benjamin Abt (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

/// <summary>
/// Tests for the IStrongOf&lt;TTarget, TSelf&gt; interface with static abstract Create method.
/// </summary>
public class StrongOfInterfaceTests
{
    private sealed class TestGuidOf(Guid Value) : StrongGuid<TestGuidOf>(Value);
    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value);
    private sealed class TestInt32Of(int Value) : StrongInt32<TestInt32Of>(Value);
    private sealed class TestInt64Of(long Value) : StrongInt64<TestInt64Of>(Value);
    private sealed class TestDecimalOf(decimal Value) : StrongDecimal<TestDecimalOf>(Value);
    private sealed class TestCharOf(char Value) : StrongChar<TestCharOf>(Value);
    private sealed class TestDateTimeOf(DateTime Value) : StrongDateTime<TestDateTimeOf>(Value);
    private sealed class TestDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<TestDateTimeOffsetOf>(Value);

    /// <summary>
    /// Verifies that the static abstract Create method works through the generic interface constraint.
    /// </summary>
    private static T CreateViaInterface<T, TTarget>(TTarget value) where T : IStrongOf<TTarget, T>
        => T.Create(value);

    [Fact]
    public void Create_StrongGuid_ViaInterface()
    {
        Guid guid = Guid.NewGuid();
        TestGuidOf result = CreateViaInterface<TestGuidOf, Guid>(guid);
        Assert.Equal(guid, result.Value);
    }

    [Fact]
    public void Create_StrongString_ViaInterface()
    {
        TestStringOf result = CreateViaInterface<TestStringOf, string>("hello");
        Assert.Equal("hello", result.Value);
    }

    [Fact]
    public void Create_StrongInt32_ViaInterface()
    {
        TestInt32Of result = CreateViaInterface<TestInt32Of, int>(42);
        Assert.Equal(42, result.Value);
    }

    [Fact]
    public void Create_StrongInt64_ViaInterface()
    {
        TestInt64Of result = CreateViaInterface<TestInt64Of, long>(9999999999L);
        Assert.Equal(9999999999L, result.Value);
    }

    [Fact]
    public void Create_StrongDecimal_ViaInterface()
    {
        TestDecimalOf result = CreateViaInterface<TestDecimalOf, decimal>(99.99m);
        Assert.Equal(99.99m, result.Value);
    }

    [Fact]
    public void Create_StrongChar_ViaInterface()
    {
        TestCharOf result = CreateViaInterface<TestCharOf, char>('A');
        Assert.Equal('A', result.Value);
    }

    [Fact]
    public void Create_StrongDateTime_ViaInterface()
    {
        DateTime dt = new(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        TestDateTimeOf result = CreateViaInterface<TestDateTimeOf, DateTime>(dt);
        Assert.Equal(dt, result.Value);
    }

    [Fact]
    public void Create_StrongDateTimeOffset_ViaInterface()
    {
        DateTimeOffset dto = new(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);
        TestDateTimeOffsetOf result = CreateViaInterface<TestDateTimeOffsetOf, DateTimeOffset>(dto);
        Assert.Equal(dto, result.Value);
    }

    [Fact]
    public void IStrongOf_ImplementedByAllTypes()
    {
        // Verify that all strong types implement IStrongOf marker interface
        Assert.IsAssignableFrom<IStrongOf>(new TestGuidOf(Guid.NewGuid()));
        Assert.IsAssignableFrom<IStrongOf>(new TestStringOf("test"));
        Assert.IsAssignableFrom<IStrongOf>(new TestInt32Of(1));
        Assert.IsAssignableFrom<IStrongOf>(new TestInt64Of(1L));
        Assert.IsAssignableFrom<IStrongOf>(new TestDecimalOf(1m));
        Assert.IsAssignableFrom<IStrongOf>(new TestCharOf('A'));
        Assert.IsAssignableFrom<IStrongOf>(new TestDateTimeOf(DateTime.UtcNow));
        Assert.IsAssignableFrom<IStrongOf>(new TestDateTimeOffsetOf(DateTimeOffset.UtcNow));
    }

    [Fact]
    public void Value_AccessibleViaInterface()
    {
        Guid guid = Guid.NewGuid();
        IStrongOf<Guid, TestGuidOf> strong = new TestGuidOf(guid);
        Assert.Equal(guid, strong.Value);
    }
}
