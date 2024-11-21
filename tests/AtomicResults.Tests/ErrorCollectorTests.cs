using AtomicResults.Abstractions;
using AtomicResults.Tests.Errors;
using FluentAssertions;

namespace AtomicResults.Tests;
public class ErrorCollectorTests
{
    [Fact]
    public void ErrorCollector_ShouldAggregateErrors()
    {
        // Arrange
        var error1 = new Error("Error 1");
        var error2 = new Error("Error 2");
        var collector = new ErrorCollector();

        // Act
        collector.WithError(error1).WithError(error2);

        // Assert
        collector.Errors.Should().HaveCount(2);
        collector.Errors.Should().Contain(error1);
        collector.Errors.Should().Contain(error2);
    }

    [Fact]
    public void ErrorCollector_WithResults_ShouldAddFailedResults()
    {
        // Arrange
        var successResult = Result.Ok();
        var failedResult = Result.Fail(new Error("Error"));
        var collector = new ErrorCollector();

        // Act
        collector.WithResults([successResult, failedResult]);

        // Assert
        collector.Errors.Should().ContainSingle();
        collector.Errors[0].Message.Should().Be("Error");
    }

    [Fact]
    public void ErrorCollector_WithResult_ShouldAddErrorIfResultFails()
    {
        // Arrange
        var failedResult = Result.Fail(new Error("Error"));
        var collector = new ErrorCollector();

        // Act
        collector.WithResult(failedResult);

        // Assert
        collector.Errors.Should().ContainSingle();
        collector.Errors[0].Message.Should().Be("Error");
    }

    [Fact]
    public void ErrorCollector_WithResult_ShouldNotAddErrorIfResultSucceeds()
    {
        // Arrange
        var successResult = Result.Ok();
        var collector = new ErrorCollector();

        // Act
        collector.WithResult(successResult);

        // Assert
        collector.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ErrorCollector_WithErrors_ShouldAddAllNonNullErrors()
    {
        // Arrange
        var error1 = new Error("Error 1");
        var error2 = new Error("Error 2");
        var errors = new List<IError> { error1, null!, error2 };
        var collector = new ErrorCollector();

        // Act
        collector.WithErrors(errors);

        // Assert
        collector.Errors.Should().HaveCount(2);
        collector.Errors.Should().Contain(error1);
        collector.Errors.Should().Contain(error2);
    }

    [Fact]
    public void ErrorCollector_WithErrors_ShouldHandleEmptyCollection()
    {
        // Arrange
        var collector = new ErrorCollector();

        // Act
        collector.WithErrors([]);

        // Assert
        collector.Errors.Should().BeEmpty();
    }

    [Fact]
    public void ErrorCollector_TryGetError_ShouldReturnErrorIfExists()
    {
        // Arrange
        var error = new Error("Error");
        var collector = new ErrorCollector().WithError(error);

        // Act
        var exists = collector.TryGetError<Error>(out var actualError);

        // Assert
        exists.Should().BeTrue();
        actualError.Should().Be(error);
    }

    [Fact]
    public void ErrorCollector_TryGetError_ShouldReturnNullIfNotExists()
    {
        // Arrange
        var collector = new ErrorCollector();

        // Act
        var exists = collector.TryGetError<Error>(out var actualError);

        // Assert
        exists.Should().BeFalse();
        actualError.Should().BeNull();
    }

    [Fact]
    public void ErrorCollector_TryGetErrors_ShouldFilterByType()
    {
        // Arrange
        var error1 = new Error("Error 1");
        var error2 = new Error("Error 2");
        var collector = new ErrorCollector()
            .WithError(error1)
            .WithError(error2);

        // Act
        collector.TryGetErrors<Error>(out var errors);

        // Assert
        errors.Should().HaveCount(2);
        errors.Should().Contain(error1);
        errors.Should().Contain(error2);
    }

    [Fact]
    public void ErrorCollector_TryGetErrors_ShouldReturnEmptyForMismatchedType()
    {
        // Arrange
        var error = new Error("Error");
        var collector = new ErrorCollector().WithError(error);

        // Act
        var result = collector.TryGetErrors<CustomError>(out var errors);

        // Assert
        result.Should().BeFalse();
        errors.Should().BeEmpty();
    }

    [Fact]
    public void ErrorCollector_HasError_ShouldReturnTrueIfErrorExists()
    {
        // Arrange
        var error = new Error("Error");
        var collector = new ErrorCollector().WithError(error);

        // Act
        var hasError = collector.HasError<Error>();

        // Assert
        hasError.Should().BeTrue();
    }
}
