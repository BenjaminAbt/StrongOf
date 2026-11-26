// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.ComponentModel;
using System.Globalization;
using System.Text;
using StrongOf.UnitTests.Generated;
using Xunit;

namespace StrongOf.UnitTests;

public sealed class PrimitiveContractTests
{
    [Fact]
    public void ComparableValuesOrderNullAndRejectUnrelatedObjects()
    {
        CheckComparable(new GenQuantity(3), new GenQuantity(3));
        CheckComparable(new GenFileSize(3), new GenFileSize(3));
        CheckComparable(new GenAmount(3), new GenAmount(3));
        CheckComparable(new GenRatio(3), new GenRatio(3));
        CheckComparable(new GenTimeout(TimeSpan.Zero), new GenTimeout(TimeSpan.Zero));
        CheckComparable(new GenGrade('A'), new GenGrade('A'));
        CheckComparable(new GenCreatedAt(DateTime.UnixEpoch), new GenCreatedAt(DateTime.UnixEpoch));
        CheckComparable(new GenOccurredAt(DateTimeOffset.UnixEpoch), new GenOccurredAt(DateTimeOffset.UnixEpoch));
        CheckComparable(new GenUserId(Guid.Empty), new GenUserId(Guid.Empty));
        CheckComparable(new GenAlias("A"), new GenAlias("A"));

        GenTimeout zero = new(TimeSpan.Zero);
        Assert.True(zero < (GenTimeout?)null);
        Assert.True(zero > (GenTimeout?)null);

        GenAlias alias = new("A");
        Assert.True(alias == (object)new GenAlias("A"));
        Assert.False(alias == new object());
    }

    private static void CheckComparable<T>(T left, T right)
        where T : class, IComparable, IComparable<T>, IEquatable<T>
    {
        Assert.Equal(0, left.CompareTo(right));
        Assert.Equal(0, left.CompareTo((object)right));
        Assert.True(left.CompareTo((T?)null) > 0);
        Assert.True(left.CompareTo((object?)null) > 0);
        Assert.Throws<ArgumentException>(() => left.CompareTo(new object()));
        Assert.True(left.Equals(right));
        Assert.True(left.Equals((object)right));
        Assert.False(left.Equals((T?)null));
        Assert.False(left.Equals((object?)null));
        Assert.False(left.Equals(new object()));
    }

    [Fact]
    public void Int32BoxedComparisons()
    {
        GenQuantity value = new(1);
        object[] equalOperands = [1, new GenQuantity(1), 1U];
        foreach (object operand in equalOperands)
        {
            Assert.True(value == operand);
            Assert.False(value != operand);
        }

        object[] orderedOperands = [1, 1L, new GenQuantity(1), 1U, 1d, 1m];
        foreach (object operand in orderedOperands)
        {
            Assert.False(value < operand);
            Assert.False(value > operand);
            Assert.True(value <= operand);
            Assert.True(value >= operand);
        }

        object unrelated = new();
        Assert.False(value == unrelated);
        Assert.False(value < unrelated);
        Assert.False(value > unrelated);
        Assert.False(value <= unrelated);
        Assert.False(value >= unrelated);
        GenQuantity? missing = null;
        Assert.True(missing == (object?)null);
    }

    [Fact]
    public void Int64BoxedComparisons()
    {
        GenFileSize value = new(1L);
        object[] equalOperands = [1, 1L, new GenFileSize(1L), 1U];
        foreach (object operand in equalOperands)
        {
            Assert.True(value == operand);
            Assert.False(value != operand);
        }

        object[] orderedOperands = [1, 1L, new GenFileSize(1L), 1U, 1d, 1m];
        foreach (object operand in orderedOperands)
        {
            Assert.False(value < operand);
            Assert.False(value > operand);
            Assert.True(value <= operand);
            Assert.True(value >= operand);
        }

        object unrelated = new();
        Assert.False(value == unrelated);
        Assert.False(value < unrelated);
        Assert.False(value > unrelated);
        Assert.False(value <= unrelated);
        Assert.False(value >= unrelated);
        GenFileSize? missing = null;
        Assert.True(missing == (object?)null);
    }

    [Fact]
    public void DecimalBoxedComparisons()
    {
        GenAmount value = new(1m);
        object[] equalOperands = [1m, new GenAmount(1m)];
        foreach (object operand in equalOperands)
        {
            Assert.True(value == operand);
            Assert.False(value != operand);
        }

        object[] orderedOperands = [1m, new GenAmount(1m), 1, 1L, 1U];
        foreach (object operand in orderedOperands)
        {
            Assert.False(value < operand);
            Assert.False(value > operand);
            Assert.True(value <= operand);
            Assert.True(value >= operand);
        }

        object unrelated = new();
        Assert.False(value == unrelated);
        Assert.False(value < unrelated);
        Assert.False(value > unrelated);
        Assert.False(value <= unrelated);
        Assert.False(value >= unrelated);
        GenAmount? missing = null;
        Assert.True(missing == (object?)null);
    }

    [Fact]
    public void DoubleBoxedComparisons()
    {
        GenRatio value = new(1d);
        object[] equalOperands = [1d, new GenRatio(1d), 1f, 1];
        foreach (object operand in equalOperands)
        {
            Assert.True(value == operand);
            Assert.False(value != operand);
        }

        object[] orderedOperands = [1d, new GenRatio(1d), 1f, 1, 1m];
        foreach (object operand in orderedOperands)
        {
            Assert.False(value < operand);
            Assert.False(value > operand);
            Assert.True(value <= operand);
            Assert.True(value >= operand);
        }

        object unrelated = new();
        Assert.False(value == unrelated);
        Assert.False(value < unrelated);
        Assert.False(value > unrelated);
        Assert.False(value <= unrelated);
        Assert.False(value >= unrelated);
        GenRatio? missing = null;
        Assert.True(missing == (object?)null);
    }

    [Fact]
    public void DateTimeBoxedComparisons()
    {
        GenCreatedAt value = new(DateTime.UnixEpoch);
        object[] equalOperands = [DateTime.UnixEpoch, new GenCreatedAt(DateTime.UnixEpoch)];
        foreach (object operand in equalOperands)
        {
            Assert.True(value == operand);
            Assert.False(value != operand);
        }

        object[] orderedOperands = [DateTime.UnixEpoch, new GenCreatedAt(DateTime.UnixEpoch), DateTimeOffset.UnixEpoch];
        foreach (object operand in orderedOperands)
        {
            Assert.False(value < operand);
            Assert.False(value > operand);
            Assert.True(value <= operand);
            Assert.True(value >= operand);
        }

        object unrelated = new();
        Assert.False(value == unrelated);
        Assert.False(value < unrelated);
        Assert.False(value > unrelated);
        Assert.False(value <= unrelated);
        Assert.False(value >= unrelated);
        GenCreatedAt? missing = null;
        Assert.True(missing == (object?)null);
    }

    [Fact]
    public void DateTimeOffsetBoxedComparisons()
    {
        GenOccurredAt value = new(DateTimeOffset.UnixEpoch);
        object[] equalOperands = [DateTimeOffset.UnixEpoch, new GenOccurredAt(DateTimeOffset.UnixEpoch)];
        foreach (object operand in equalOperands)
        {
            Assert.True(value == operand);
            Assert.False(value != operand);
        }

        object[] orderedOperands = [DateTimeOffset.UnixEpoch, new GenOccurredAt(DateTimeOffset.UnixEpoch), DateTime.UnixEpoch];
        foreach (object operand in orderedOperands)
        {
            Assert.False(value < operand);
            Assert.False(value > operand);
            Assert.True(value <= operand);
            Assert.True(value >= operand);
        }

        object unrelated = new();
        Assert.False(value == unrelated);
        Assert.False(value < unrelated);
        Assert.False(value > unrelated);
        Assert.False(value <= unrelated);
        Assert.False(value >= unrelated);
        GenOccurredAt? missing = null;
        Assert.True(missing == (object?)null);
    }

    [Fact]
    public void PrimitiveConvertersReadSupportedInputs()
    {
        CheckConverter(new StrongDoubleTypeConverter<GenRatio>(), new GenRatio(1), 1d, 1f, 1, "1");
        CheckConverter(new StrongInt64TypeConverter<GenFileSize>(), new GenFileSize(1), 1L, "1");
        CheckConverter(new StrongBooleanTypeConverter<GenIsActive>(), new GenIsActive(true), true, "True");
        CheckConverter(new StrongTimeSpanTypeConverter<GenTimeout>(), new GenTimeout(TimeSpan.FromSeconds(1)),
            TimeSpan.FromSeconds(1), TimeSpan.TicksPerSecond, "00:00:01");
    }

    private static void CheckConverter(TypeConverter converter, object expected, params object[] inputs)
    {
        foreach (object input in inputs)
        {
            Assert.True(converter.CanConvertFrom(input.GetType()));
            Assert.Equal(expected, converter.ConvertFrom(null, CultureInfo.InvariantCulture, input));
            Assert.Equal(expected, converter.ConvertFrom(null, null, input));
        }
        Assert.False(converter.CanConvertFrom(typeof(Uri)));
        Assert.Throws<NotSupportedException>(() => converter.ConvertFromInvariantString("invalid"));
        Assert.Throws<NotSupportedException>(() => converter.ConvertFrom(null, CultureInfo.InvariantCulture, new object()));
    }

    [Fact]
    public void ValidatedFactoriesApplyPredicateBeforeConstruction()
    {
        Assert.True(StrongValidation.TryCreate("A", s => s.Length == 1, GenAlias.Create, out GenAlias? alias));
        Assert.Equal("A", alias?.Value);
        Assert.False(StrongValidation.TryCreate("", s => s.Length == 1, GenAlias.Create, out alias));
        Assert.Null(alias);
        Assert.False(StrongValidation.TryCreate((string?)null, s => s.Length == 1, GenAlias.Create, out alias));
        Assert.Null(alias);
        Assert.Equal(1, StrongValidation.Require(1, n => n > 0));
        Assert.Throws<ArgumentException>(() => StrongValidation.Require(0, n => n > 0));
        ArgumentException custom = Assert.Throws<ArgumentException>(() => StrongValidation.Require(0, n => n > 0, "custom"));
        Assert.StartsWith("custom", custom.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void BulkFactoriesPreserveInputOrder()
    {
        int[] values = [3, 1, 2];
        IEnumerable<int>[] sources = [values, values.ToList(), Array.AsReadOnly(values),
            new LinkedList<int>(values), new Queue<int>(values), values.Where(n => n > 0)];
        foreach (IEnumerable<int> source in sources)
        {
            Assert.Equal(values, GenQuantity.From(source).Select(x => x.Value));
        }
        Assert.Equal(values, GenQuantity.FromArray(values).Select(x => x.Value));
        Assert.Empty(GenQuantity.FromSpan([]));
        Assert.Null(GenQuantity.FromArray(null));
    }

    [Fact]
    public void NumericTypesRoundTripUtf8()
    {
        CheckNumericUtf8(new GenQuantity(42));
        CheckNumericUtf8(new GenFileSize(42));
        CheckNumericUtf8(new GenAmount(42));
        CheckNumericUtf8(new GenRatio(42));
    }

    private static void CheckNumericUtf8<T>(T expected)
        where T : class, IUtf8SpanParsable<T>, IUtf8SpanFormattable
    {
        CultureInfo culture = CultureInfo.InvariantCulture;
        Assert.Equal(expected, T.Parse("42"u8, culture));
        Assert.True(T.TryParse("42"u8, culture, out T? parsed));
        Assert.Equal(expected, parsed);
        Span<byte> buffer = stackalloc byte[8];
        Assert.True(parsed.TryFormat(buffer, out int written, default, culture));
        Assert.Equal("42", Encoding.UTF8.GetString(buffer[..written]));
        Assert.False(T.TryParse("bad"u8, culture, out parsed));
        Assert.Null(parsed);
    }

    [Fact]
    public void SpanAndStringParsingAgree()
    {
        CheckParsing("true", new GenIsActive(true));
        CheckParsing("A", new GenGrade('A'));
        CheckParsing("1", new GenRatio(1));
        CheckParsing("42", new GenFileSize(42));
        CheckParsing("42", new GenQuantity(42));
        CheckParsing("42", new GenAmount(42));
        CheckParsing("01:00:00", new GenTimeout(TimeSpan.FromHours(1)));
        CheckParsing(Guid.Empty.ToString(), new GenUserId(Guid.Empty));
        CheckParsing("1970-01-01T00:00:00", new GenCreatedAt(DateTime.UnixEpoch));
        CheckParsing("1970-01-01T00:00:00Z", new GenOccurredAt(DateTimeOffset.UnixEpoch));
    }

    private static void CheckParsing<T>(string text, T expected) where T : class, ISpanParsable<T>
    {
        CultureInfo culture = CultureInfo.InvariantCulture;
        Assert.Equal(expected, T.Parse(text, culture));
        Assert.Equal(expected, T.Parse(text.AsSpan(), culture));
        Assert.True(T.TryParse(text, culture, out T? parsed));
        Assert.Equal(expected, parsed);
        Assert.True(T.TryParse(text.AsSpan(), culture, out parsed));
        Assert.Equal(expected, parsed);
        Assert.False(T.TryParse((string?)null, culture, out parsed));
        Assert.Null(parsed);
        Assert.False(T.TryParse("invalid", culture, out parsed));
        Assert.Null(parsed);
        Assert.False(T.TryParse("invalid".AsSpan(), culture, out parsed));
        Assert.Null(parsed);
    }

    private sealed class Label(string value) : StrongOf<string, Label>(value), IStrongOf<string, Label>
    {
        public static Label Create(string value) => new(value);
    }

    [Fact]
    public void BaseEqualityHandlesBoxedValues()
    {
        Label value = new("A");
        object other = new Label("A");
        object primitive = "A";
        object unrelated = 1;
        Assert.True(value == other);
        Assert.True(value == primitive);
        Assert.True(value != unrelated);
        Assert.True((Label?)null == (object?)null);
        Assert.False((Label?)null == value);
        Assert.False(value == (Label?)null);
        Assert.False(value.Equals((object?)null));
        Assert.True(value.Equals((object)value));
        Assert.True(value.Equals(other));
        Assert.True(value.Equals(primitive));
        Assert.False(value.Equals(unrelated));
        Assert.False(value.Equals((string?)null));
        Assert.False(value.Equals((StrongOf<string, Label>?)null));
    }

    [Fact]
    public void AdditionalPrimitiveConversions()
    {
        CultureInfo culture = CultureInfo.InvariantCulture;
        CheckConverter(new StrongDateTimeTypeConverter<GenCreatedAt>(), new GenCreatedAt(DateTime.UnixEpoch), DateTime.UnixEpoch, "1970-01-01T00:00:00Z");
        CheckConverter(new StrongDecimalTypeConverter<GenAmount>(), new GenAmount(1), 1m, 1d, 1, "1");
        CheckConverter(new StrongGuidTypeConverter<GenUserId>(), new GenUserId(Guid.Empty), Guid.Empty, Guid.Empty.ToString());
        CheckConverter(new StrongInt32TypeConverter<GenQuantity>(), new GenQuantity(1), 1, "1");
        StrongStringTypeConverter<GenAlias> stringConverter = new();
        Assert.Equal(new GenAlias("A"), stringConverter.ConvertFrom(null, culture, "A"));
        Assert.Throws<NotSupportedException>(() => stringConverter.ConvertFrom(null, culture, new object()));
        Assert.Equal("1", new GenRatio(1).ToString("0", culture));
#pragma warning disable MA0011 // Exercise the convenience overload.
        Assert.Equal("1", new GenAmount(1).ToString("0"));
#pragma warning restore MA0011
        Assert.Equal("A", new GenGrade('A').ToString(null, culture));
#pragma warning disable MA0011 // Exercise the convenience overload.
        Assert.Equal(Guid.Empty.ToString("N"), new GenUserId(Guid.Empty).ToString("N"));
#pragma warning restore MA0011
    }

    [Fact]
    public void BoxedGuidAndCharacterComparisons()
    {
        GenUserId value = new(Guid.Empty);
        object raw = Guid.Empty;
        Assert.True(value >= raw);
        Assert.True(value <= raw);
        Assert.False(value < raw);
        object unrelated = 'a';
        Assert.False(value == unrelated);
        Assert.False(value > unrelated);
        Assert.False(value < unrelated);
        Assert.False(value <= unrelated);
        Assert.False(value >= unrelated);
        object grade = new GenGrade('B');
        GenGrade first = new('A');
        Assert.True(first < grade);
        Assert.False(first > grade);
        Assert.True(first <= grade);
        Assert.False(first >= grade);
        Assert.True(new GenIsActive(true) == (object)new GenIsActive(true));
        Assert.False(new GenIsActive(true) == unrelated);
        Assert.False((GenCreatedAt)null! == DateTime.UnixEpoch);
    }

    [Fact]
    public void TextComparisonsUseOrdinalOrder()
    {
        GenAlias a = new("abc");
        GenAlias b = new("def");
        CheckComparable(a, new GenAlias("abc"));
        Assert.True(a.Equals(new GenAlias("ABC"), StringComparison.OrdinalIgnoreCase));
#pragma warning disable MA0074 // Exercise the convenience overload.
        Assert.True(a.Contains("bc"));
        Assert.True(a.StartsWith("ab"));
        Assert.True(a.EndsWith("bc"));
#pragma warning restore MA0074
        Assert.True(a < b);
        Assert.False(a > b);
        Assert.True(a <= b);
        Assert.False(a >= b);
    }

    [Fact]
    public void MissingDurationSortsBeforeZero()
    {
        GenTimeout? missing = null;
        GenTimeout zero = new(TimeSpan.Zero);
        Assert.True(missing <= zero);
        Assert.False(missing >= zero);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConvenienceParsersReportWhetherInputWasValid(bool valid)
    {
        CultureInfo culture = CultureInfo.InvariantCulture;
        string number = valid ? "1" : "invalid";
        Assert.Equal(valid, GenRatio.TryParse(number.AsSpan(), out GenRatio? ratio, culture));
        Assert.Equal(valid ? 1d : (double?)null, ratio?.Value);
        Assert.Equal(valid, GenAmount.TryParse(number.AsSpan(), out GenAmount? amount, culture));
        Assert.Equal(valid, GenAmount.TryParse(number.AsSpan(), NumberStyles.Integer, culture, out amount));
        Assert.Equal(valid ? 1m : (decimal?)null, amount?.Value);
        Assert.Equal(valid, GenTimeout.TryParse(valid ? "01:00:00" : "invalid", out GenTimeout? timeout, culture));
        Assert.Equal(valid ? TimeSpan.FromHours(1) : (TimeSpan?)null, timeout?.Value);
#pragma warning disable MA0011 // Exercise the convenience overload.
        Assert.Equal(valid, GenGrade.TryParse(number, out GenGrade? grade));
#pragma warning restore MA0011
        Assert.Equal(valid ? '1' : (char?)null, grade?.Value);
#pragma warning disable MA0011 // Exercise the convenience overload.
        Assert.Equal(valid, GenUserId.TryParse(valid ? Guid.Empty.ToString() : "invalid", out GenUserId? id));
#pragma warning restore MA0011
        Assert.Equal(valid ? Guid.Empty : (Guid?)null, id?.Value);
        string date = valid ? "1970-01-01" : "invalid";
        Assert.Equal(valid, GenOccurredAt.TryParseExact(date, "yyyy-MM-dd", culture, DateTimeStyles.AssumeUniversal, out GenOccurredAt? offset));
        Assert.Equal(valid, GenOccurredAt.TryParse(date, culture, DateTimeStyles.AssumeUniversal, out offset));
        Assert.Equal(valid, GenCreatedAt.TryParse(date, culture, DateTimeStyles.AssumeUniversal, out GenCreatedAt? timestamp));
        Assert.Equal(valid, timestamp is not null);
        Assert.Equal(valid, offset is not null);
    }

    [Fact]
    public void TemporalWrappersPreserveUtcAndFormatUtf8()
    {
        GenOccurredAt value = GenOccurredAt.FromNullable(DateTimeOffset.UnixEpoch)!;
        Assert.Equal(DateTime.UnixEpoch, value.AsDateTimeUtc());
        Assert.Null(GenOccurredAt.FromNullable(null));
        Assert.Equal(DateTimeOffset.UnixEpoch.GetHashCode(), value.GetHashCode());
        GenCreatedAt date = new(DateTime.UnixEpoch);
        Assert.Equal(DateTime.UnixEpoch.GetHashCode(), date.GetHashCode());
        CheckUtf8(value, "yyyy", "1970");
        CheckUtf8(date, "yyyy", "1970");
        CheckUtf8(new GenUserId(Guid.Empty), "N", Guid.Empty.ToString("N"));
    }

    private static void CheckUtf8(IUtf8SpanFormattable value, string format, string expected)
    {
        Span<byte> buffer = stackalloc byte[64];
        Assert.True(value.TryFormat(buffer, out int written, format, CultureInfo.InvariantCulture));
        Assert.Equal(expected, Encoding.UTF8.GetString(buffer[..written]));
    }
}
