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

[StrongInt32]
public partial class GenQuantity;

[StrongInt64]
public partial class GenFileSize;

[StrongString]
public partial class GenEmail;

[StrongTimeSpan]
public partial class GenTimeout;

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
        Assert.Equal(7, new GenQuantity(7).Value);
        Assert.Equal(1234L, new GenFileSize(1234L).Value);
        Assert.Equal("a@b", new GenEmail("a@b").Value);
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
        Assert.True(typeof(StrongString<GenEmail>).IsAssignableFrom(typeof(GenEmail)));
        Assert.True(typeof(StrongInt32<GenQuantity>).IsAssignableFrom(typeof(GenQuantity)));
        Assert.True(typeof(StrongDecimal<GenAmount>).IsAssignableFrom(typeof(GenAmount)));
    }
}
