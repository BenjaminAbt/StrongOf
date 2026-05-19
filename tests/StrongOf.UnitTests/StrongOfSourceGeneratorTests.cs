// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using StrongOf.SourceGeneration;
using Xunit;

namespace StrongOf.UnitTests.Generated;

// Proof types that exercise every generator-supported primitive.
// The user only writes the marker attribute + the partial class declaration.
// The StrongOf.SourceGenerators incremental generator emits the constructor,
// the base-type inheritance (e.g. StrongGuid<UserId>) and the AOT-friendly
// static Create method - no Expression-based factory required.

[StrongBoolean]
public partial class GenIsActive;

[StrongChar]
public partial class GenGrade;

[StrongDateTime]
public partial class GenCreatedAt;

[StrongDateTimeOffset]
public partial class GenOccurredAt;

[StrongDecimal]
public partial class GenAmount;

[StrongDouble]
public partial class GenRatio;

[StrongGuid]
public partial class GenUserId;

[Strong<Guid>]
public partial class GenOrderId;

[StrongInt32]
public partial class GenQuantity;

[StrongInt64]
public partial class GenFileSize;

[StrongString]
public partial class GenEmail;

[Strong<string>]
public partial class GenAlias;

[StrongTimeSpan]
public partial class GenTimeout;

// Full matrix for generic syntax + typeof metadata marker.
[Strong<bool>]
public partial class GenGenericBool;

[Strong<char>]
public partial class GenGenericChar;

[Strong<DateTime>]
public partial class GenGenericDateTime;

[Strong<DateTimeOffset>]
public partial class GenGenericDateTimeOffset;

[Strong<decimal>]
public partial class GenGenericDecimal;

[Strong<double>]
public partial class GenGenericDouble;

[Strong<Guid>]
public partial class GenGenericGuid;

[Strong<int>]
public partial class GenGenericInt32;

[Strong<long>]
public partial class GenGenericInt64;

[Strong<string>]
public partial class GenGenericString;

[Strong<TimeSpan>]
public partial class GenGenericTimeSpan;

// Full matrix for non-generic typeof syntax (interop style).
[Strong(typeof(bool))]
public partial class GenTypeofBool;

[Strong(typeof(char))]
public partial class GenTypeofChar;

[Strong(typeof(DateTime))]
public partial class GenTypeofDateTime;

[Strong(typeof(DateTimeOffset))]
public partial class GenTypeofDateTimeOffset;

[Strong(typeof(decimal))]
public partial class GenTypeofDecimal;

[Strong(typeof(double))]
public partial class GenTypeofDouble;

[Strong(typeof(Guid))]
public partial class GenTypeofGuid;

[Strong(typeof(int))]
public partial class GenTypeofInt32;

[Strong(typeof(long))]
public partial class GenTypeofInt64;

[Strong(typeof(string))]
public partial class GenTypeofString;

[Strong(typeof(TimeSpan))]
public partial class GenTypeofTimeSpan;

// User-extended partial: only adds methods, never declares the base type or
// primary ctor (those come from the generator).
[StrongString]
public partial class GenEmailWithExtras
{
    public bool LooksLikeEmail() => Value.Contains('@');
}

public class StrongOfSourceGeneratorTests
{
    [Fact]
    public void Generator_Emits_Constructor_And_BaseType_For_All_Primitives()
    {
        Assert.True(new GenIsActive(true).Value);
        Assert.Equal('A', new GenGrade('A').Value);
        Assert.Equal(new DateTime(2024, 1, 1), new GenCreatedAt(new DateTime(2024, 1, 1)).Value);
        Assert.Equal(DateTimeOffset.UnixEpoch, new GenOccurredAt(DateTimeOffset.UnixEpoch).Value);
        Assert.Equal(42.5m, new GenAmount(42.5m).Value);
        Assert.Equal(0.5d, new GenRatio(0.5d).Value);
        Assert.NotEqual(Guid.Empty, new GenUserId(Guid.NewGuid()).Value);
        Assert.NotEqual(Guid.Empty, new GenOrderId(Guid.NewGuid()).Value);
        Assert.Equal(7, new GenQuantity(7).Value);
        Assert.Equal(1234L, new GenFileSize(1234L).Value);
        Assert.Equal("a@b", new GenEmail("a@b").Value);
        Assert.Equal("alias", new GenAlias("alias").Value);
        Assert.Equal(TimeSpan.FromMinutes(5), new GenTimeout(TimeSpan.FromMinutes(5)).Value);
    }

    [Fact]
    public void Generator_Emits_Static_Create_That_Matches_New()
    {
        Guid g = Guid.NewGuid();
        GenUserId viaCreate = GenUserId.Create(g);
        GenUserId viaNew = new(g);

        Assert.Equal(viaNew.Value, viaCreate.Value);
    }

    [Fact]
    public void Generator_From_Uses_Generated_Create_Without_Reflection()
    {
        // From() dispatches via the static abstract IStrongOf<,>.Create member.
        // For generated types this is the generated public static Create -> direct 'new'.
        Guid g = Guid.NewGuid();
        GenUserId fromValue = GenUserId.From(g);

        Assert.Equal(g, fromValue.Value);
        Assert.IsType<GenUserId>(fromValue);
    }

    [Fact]
    public void Generator_Allows_User_Extension_Partials()
    {
        GenEmailWithExtras email = new("user@example.com");
        Assert.True(email.LooksLikeEmail());

        GenEmailWithExtras invalid = new("no-at-symbol");
        Assert.False(invalid.LooksLikeEmail());
    }

    [Fact]
    public void Generator_Type_Is_Subclass_Of_Expected_Base()
    {
        // Sanity check: each generated type derives from the right StrongXxx<T> base.
        Assert.True(typeof(StrongGuid<GenUserId>).IsAssignableFrom(typeof(GenUserId)));
        Assert.True(typeof(StrongGuid<GenOrderId>).IsAssignableFrom(typeof(GenOrderId)));
        Assert.True(typeof(StrongString<GenEmail>).IsAssignableFrom(typeof(GenEmail)));
        Assert.True(typeof(StrongString<GenAlias>).IsAssignableFrom(typeof(GenAlias)));
        Assert.True(typeof(StrongInt32<GenQuantity>).IsAssignableFrom(typeof(GenQuantity)));
        Assert.True(typeof(StrongDecimal<GenAmount>).IsAssignableFrom(typeof(GenAmount)));
    }

    [Fact]
    public void Generator_Generic_StrongT_Works_For_All_Supported_Target_Types()
    {
        DateTimeOffset timestamp = DateTimeOffset.UtcNow;

        Assert.True(new GenGenericBool(true).Value);
        Assert.Equal('B', new GenGenericChar('B').Value);
        Assert.Equal(new DateTime(2025, 1, 2), new GenGenericDateTime(new DateTime(2025, 1, 2)).Value);
        Assert.Equal(timestamp, new GenGenericDateTimeOffset(timestamp).Value);
        Assert.Equal(19.99m, new GenGenericDecimal(19.99m).Value);
        Assert.Equal(1.25d, new GenGenericDouble(1.25d).Value);
        Assert.NotEqual(Guid.Empty, new GenGenericGuid(Guid.NewGuid()).Value);
        Assert.Equal(42, new GenGenericInt32(42).Value);
        Assert.Equal(4242L, new GenGenericInt64(4242L).Value);
        Assert.Equal("generic", new GenGenericString("generic").Value);
        Assert.Equal(TimeSpan.FromSeconds(30), new GenGenericTimeSpan(TimeSpan.FromSeconds(30)).Value);
    }

    [Fact]
    public void Generator_Generic_StrongT_Derives_From_Correct_Strong_Base_Types()
    {
        Assert.True(typeof(StrongBoolean<GenGenericBool>).IsAssignableFrom(typeof(GenGenericBool)));
        Assert.True(typeof(StrongChar<GenGenericChar>).IsAssignableFrom(typeof(GenGenericChar)));
        Assert.True(typeof(StrongDateTime<GenGenericDateTime>).IsAssignableFrom(typeof(GenGenericDateTime)));
        Assert.True(typeof(StrongDateTimeOffset<GenGenericDateTimeOffset>).IsAssignableFrom(typeof(GenGenericDateTimeOffset)));
        Assert.True(typeof(StrongDecimal<GenGenericDecimal>).IsAssignableFrom(typeof(GenGenericDecimal)));
        Assert.True(typeof(StrongDouble<GenGenericDouble>).IsAssignableFrom(typeof(GenGenericDouble)));
        Assert.True(typeof(StrongGuid<GenGenericGuid>).IsAssignableFrom(typeof(GenGenericGuid)));
        Assert.True(typeof(StrongInt32<GenGenericInt32>).IsAssignableFrom(typeof(GenGenericInt32)));
        Assert.True(typeof(StrongInt64<GenGenericInt64>).IsAssignableFrom(typeof(GenGenericInt64)));
        Assert.True(typeof(StrongString<GenGenericString>).IsAssignableFrom(typeof(GenGenericString)));
        Assert.True(typeof(StrongTimeSpan<GenGenericTimeSpan>).IsAssignableFrom(typeof(GenGenericTimeSpan)));
    }

    [Fact]
    public void Generator_Typeof_Syntax_Works_For_All_Supported_Target_Types()
    {
        DateTimeOffset timestamp = DateTimeOffset.UtcNow;

        Assert.True(new GenTypeofBool(true).Value);
        Assert.Equal('T', new GenTypeofChar('T').Value);
        Assert.Equal(new DateTime(2026, 1, 1), new GenTypeofDateTime(new DateTime(2026, 1, 1)).Value);
        Assert.Equal(timestamp, new GenTypeofDateTimeOffset(timestamp).Value);
        Assert.Equal(1.1m, new GenTypeofDecimal(1.1m).Value);
        Assert.Equal(2.2d, new GenTypeofDouble(2.2d).Value);
        Assert.NotEqual(Guid.Empty, new GenTypeofGuid(Guid.NewGuid()).Value);
        Assert.Equal(10, new GenTypeofInt32(10).Value);
        Assert.Equal(20L, new GenTypeofInt64(20L).Value);
        Assert.Equal("typeof", new GenTypeofString("typeof").Value);
        Assert.Equal(TimeSpan.FromMinutes(1), new GenTypeofTimeSpan(TimeSpan.FromMinutes(1)).Value);

        Assert.True(typeof(StrongBoolean<GenTypeofBool>).IsAssignableFrom(typeof(GenTypeofBool)));
        Assert.True(typeof(StrongChar<GenTypeofChar>).IsAssignableFrom(typeof(GenTypeofChar)));
        Assert.True(typeof(StrongDateTime<GenTypeofDateTime>).IsAssignableFrom(typeof(GenTypeofDateTime)));
        Assert.True(typeof(StrongDateTimeOffset<GenTypeofDateTimeOffset>).IsAssignableFrom(typeof(GenTypeofDateTimeOffset)));
        Assert.True(typeof(StrongDecimal<GenTypeofDecimal>).IsAssignableFrom(typeof(GenTypeofDecimal)));
        Assert.True(typeof(StrongDouble<GenTypeofDouble>).IsAssignableFrom(typeof(GenTypeofDouble)));
        Assert.True(typeof(StrongGuid<GenTypeofGuid>).IsAssignableFrom(typeof(GenTypeofGuid)));
        Assert.True(typeof(StrongInt32<GenTypeofInt32>).IsAssignableFrom(typeof(GenTypeofInt32)));
        Assert.True(typeof(StrongInt64<GenTypeofInt64>).IsAssignableFrom(typeof(GenTypeofInt64)));
        Assert.True(typeof(StrongString<GenTypeofString>).IsAssignableFrom(typeof(GenTypeofString)));
        Assert.True(typeof(StrongTimeSpan<GenTypeofTimeSpan>).IsAssignableFrom(typeof(GenTypeofTimeSpan)));
    }
}
