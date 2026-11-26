// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using Microsoft.AspNetCore.Mvc.ModelBinding;
using StrongOf.AspNetCore.Mvc;
using Xunit;

namespace StrongOf.AspNetCore.IntegrationTests.Mvc;

[StrongBoolean]
internal sealed partial class BoundBoolean;

[StrongChar]
internal sealed partial class BoundChar;

[StrongDateTime]
internal sealed partial class BoundDateTime;

[StrongDateTimeOffset]
internal sealed partial class BoundDateTimeOffset;

[StrongDecimal]
internal sealed partial class BoundDecimal;

[StrongDouble]
internal sealed partial class BoundDouble;

[StrongGuid]
internal sealed partial class BoundGuid;

[StrongInt32]
internal sealed partial class BoundInt32;

[StrongInt64]
internal sealed partial class BoundInt64;

[StrongTimeSpan]
internal sealed partial class BoundTimeSpan;

[StrongString]
internal sealed partial class BoundString;

public class PrimitiveBinderTests
{
    [Fact]
    public void BooleanBinderParsesInput()
    {
        StrongBooleanBinder<BoundBoolean> binder = new();
        Assert.True(binder.TryHandle("true", out ModelBindingResult result));
        Assert.True(Assert.IsType<BoundBoolean>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void CharBinderParsesInput()
    {
        StrongCharBinder<BoundChar> binder = new();
        Assert.True(binder.TryHandle("a", out ModelBindingResult result));
        Assert.Equal('a', Assert.IsType<BoundChar>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void DateTimeBinderParsesInput()
    {
        StrongDateTimeBinder<BoundDateTime> binder = new();
        Assert.True(binder.TryHandle("2001-02-03", out ModelBindingResult result));
        Assert.Equal(new DateTime(2001, 2, 3), Assert.IsType<BoundDateTime>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void DateTimeOffsetBinderParsesInput()
    {
        StrongDateTimeOffsetBinder<BoundDateTimeOffset> binder = new();
        Assert.True(binder.TryHandle("1970-01-01T00:00:00Z", out ModelBindingResult result));
        Assert.Equal(DateTimeOffset.UnixEpoch, Assert.IsType<BoundDateTimeOffset>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void DecimalBinderParsesInput()
    {
        StrongDecimalBinder<BoundDecimal> binder = new();
        Assert.True(binder.TryHandle("2.5", out ModelBindingResult result));
        Assert.Equal(2.5m, Assert.IsType<BoundDecimal>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void DoubleBinderParsesInput()
    {
        StrongDoubleBinder<BoundDouble> binder = new();
        Assert.True(binder.TryHandle("2", out ModelBindingResult result));
        Assert.Equal(2d, Assert.IsType<BoundDouble>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void GuidBinderParsesInput()
    {
        StrongGuidBinder<BoundGuid> binder = new();
        Assert.True(binder.TryHandle("00000000-0000-0000-0000-000000000000", out ModelBindingResult result));
        Assert.Equal(Guid.Empty, Assert.IsType<BoundGuid>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void Int32BinderParsesInput()
    {
        StrongInt32Binder<BoundInt32> binder = new();
        Assert.True(binder.TryHandle("2", out ModelBindingResult result));
        Assert.Equal(2, Assert.IsType<BoundInt32>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void Int64BinderParsesInput()
    {
        StrongInt64Binder<BoundInt64> binder = new();
        Assert.True(binder.TryHandle("2", out ModelBindingResult result));
        Assert.Equal(2L, Assert.IsType<BoundInt64>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void TimeSpanBinderParsesInput()
    {
        StrongTimeSpanBinder<BoundTimeSpan> binder = new();
        Assert.True(binder.TryHandle("01:00:00", out ModelBindingResult result));
        Assert.Equal(TimeSpan.FromHours(1), Assert.IsType<BoundTimeSpan>(result.Model).Value);
        Assert.False(binder.TryHandle("invalid", out result));
        Assert.False(result.IsModelSet);
    }

    [Fact]
    public void StringBinderParsesInput()
    {
        StrongStringBinder<BoundString> binder = new();
        Assert.True(binder.TryHandle(" text ", out ModelBindingResult result));
        Assert.Equal("text", Assert.IsType<BoundString>(result.Model).Value);
        Assert.False(binder.TryHandle(null!, out result));
        Assert.False(result.IsModelSet);
    }
}
