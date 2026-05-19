// Copyright © BEN ABT (https://benjamin-abt.com) - all rights reserved

using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using NSubstitute;
using StrongOf.AspNetCore.Mvc;
using Xunit;

namespace StrongOf.AspNetCore.UnitTests.Mvc;

public class StrongOfModelBinderProviderTests
{
    private sealed class TestUserId(Guid value) : StrongGuid<TestUserId>(value), IStrongOf<Guid, TestUserId>
    {
        public static TestUserId Create(Guid value) => new(value);
    }

    private sealed class TestEmail(string value) : StrongString<TestEmail>(value), IStrongOf<string, TestEmail>
    {
        public static TestEmail Create(string value) => new(value);
    }

    private sealed class TestTimeout(TimeSpan value) : StrongTimeSpan<TestTimeout>(value), IStrongOf<TimeSpan, TestTimeout>
    {
        public static TestTimeout Create(TimeSpan value) => new(value);
    }

    [Fact]
    public void AddStrongOfModelBinderProvider_WithStrongTypes_RegistersProviderThatResolvesBinders()
    {
        // Arrange
        MvcOptions options = new();

        // Act
        options.AddStrongOfModelBinderProvider(typeof(TestUserId), typeof(TestEmail), typeof(TestTimeout));
        StrongOfModelBinderProvider provider = Assert.IsType<StrongOfModelBinderProvider>(options.ModelBinderProviders[0]);

        // Assert
        IModelBinder? guidBinder = provider.GetBinder(CreateContext(typeof(TestUserId)));
        IModelBinder? stringBinder = provider.GetBinder(CreateContext(typeof(TestEmail)));
        IModelBinder? timeSpanBinder = provider.GetBinder(CreateContext(typeof(TestTimeout)));

        Assert.IsType<BinderTypeModelBinder>(guidBinder);
        Assert.IsType<BinderTypeModelBinder>(stringBinder);
        Assert.IsType<BinderTypeModelBinder>(timeSpanBinder);
    }

    [Fact]
    public void AddStrongOfModelBinderProviderFromAssemblies_WithAssembly_RegistersDiscoveredStrongTypes()
    {
        // Arrange
        MvcOptions options = new();

        // Act
        options.AddStrongOfModelBinderProviderFromAssemblies(typeof(TestUserId).Assembly);
        StrongOfModelBinderProvider provider = Assert.IsType<StrongOfModelBinderProvider>(options.ModelBinderProviders[0]);

        // Assert
        IModelBinder? guidBinder = provider.GetBinder(CreateContext(typeof(TestUserId)));
        IModelBinder? stringBinder = provider.GetBinder(CreateContext(typeof(TestEmail)));

        Assert.IsType<BinderTypeModelBinder>(guidBinder);
        Assert.IsType<BinderTypeModelBinder>(stringBinder);
        Assert.Null(provider.GetBinder(CreateContext(typeof(string))));
    }

    [Fact]
    public void CreateBinderMap_WithUnsupportedType_ThrowsNotSupportedException()
    {
        // Arrange
        MvcOptions options = new();

        // Act
        NotSupportedException exception = Assert.Throws<NotSupportedException>(
            () => options.AddStrongOfModelBinderProvider(typeof(string)));

        // Assert
        Assert.Contains(typeof(string).ToString(), exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void CreateBinderMapFromAssemblies_WithAssemblyWithoutStrongTypes_ThrowsArgumentException()
    {
        // Arrange
        MvcOptions options = new();

        // Act
        ArgumentException exception = Assert.Throws<ArgumentException>(
            () => options.AddStrongOfModelBinderProviderFromAssemblies(typeof(Assembly).Assembly));

        // Assert
        Assert.Contains("At least one supported StrongOf type", exception.Message, StringComparison.Ordinal);
    }

    private static ModelBinderProviderContext CreateContext(Type modelType)
    {
        EmptyModelMetadataProvider metadataProvider = new();
        ModelMetadata metadata = metadataProvider.GetMetadataForType(modelType);
        ModelBinderProviderContext context = Substitute.For<ModelBinderProviderContext>();
        context.Metadata.Returns(metadata);
        return context;
    }
}
