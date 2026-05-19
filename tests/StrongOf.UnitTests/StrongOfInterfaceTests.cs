// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Xunit;

namespace StrongOf.UnitTests;

/// <summary>
/// Tests for the IStrongOf&lt;TTarget, TSelf&gt; interface with static abstract Create method.
/// </summary>
public class StrongOfInterfaceTests
{
    private sealed class TestGuidOf(Guid Value) : StrongGuid<TestGuidOf>(Value), IStrongOf<Guid, TestGuidOf>
    {
        public static TestGuidOf Create(Guid value) => new(value);
    }

    private sealed class TestStringOf(string Value) : StrongString<TestStringOf>(Value), IStrongOf<string, TestStringOf>
    {
        public static TestStringOf Create(string value) => new(value);
    }

    private sealed class TestInt32Of(int Value) : StrongInt32<TestInt32Of>(Value), IStrongOf<int, TestInt32Of>
    {
        public static TestInt32Of Create(int value) => new(value);
    }

    private sealed class TestInt64Of(long Value) : StrongInt64<TestInt64Of>(Value), IStrongOf<long, TestInt64Of>
    {
        public static TestInt64Of Create(long value) => new(value);
    }

    private sealed class TestDecimalOf(decimal Value) : StrongDecimal<TestDecimalOf>(Value), IStrongOf<decimal, TestDecimalOf>
    {
        public static TestDecimalOf Create(decimal value) => new(value);
    }

    private sealed class TestCharOf(char Value) : StrongChar<TestCharOf>(Value), IStrongOf<char, TestCharOf>
    {
        public static TestCharOf Create(char value) => new(value);
    }

    private sealed class TestDateTimeOf(DateTime Value) : StrongDateTime<TestDateTimeOf>(Value), IStrongOf<DateTime, TestDateTimeOf>
    {
        public static TestDateTimeOf Create(DateTime value) => new(value);
    }

    private sealed class TestDateTimeOffsetOf(DateTimeOffset Value) : StrongDateTimeOffset<TestDateTimeOffsetOf>(Value), IStrongOf<DateTimeOffset, TestDateTimeOffsetOf>
    {
        public static TestDateTimeOffsetOf Create(DateTimeOffset value) => new(value);
    }

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
