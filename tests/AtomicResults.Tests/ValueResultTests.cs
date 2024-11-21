using AtomicResults.Abstractions;
using AtomicResults.Tests.Errors;
using FluentAssertions;

namespace AtomicResults.Tests;
public class ValueResultTests
{
    [Fact]
    public void ValueResult_Ok_ShouldBeSuccessful()
    {
        // Act
        var result = ValueResult.Ok();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Error.Should().BeNull();
    }

    [Fact]
    public void ValueResult_Fail_ShouldContainError()
    {
        // Arrange
        var error = new Error("Test error");

        // Act
        var result = ValueResult.Fail(error);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void ValueResult_HasError_ShouldReturnTrueForMatchingError()
    {
        // Arrange
        var error = new Error("Test error");
        var result = ValueResult.Fail(error);

        // Act
        var hasError = result.HasError<Error>();

        // Assert
        hasError.Should().BeTrue();
    }

    [Fact]
    public void ValueResult_TryGetError_ShouldReturnErrorIfExists()
    {
        // Arrange
        var error = new Error("Test error");
        var result = ValueResult.Fail(error);

        // Act
        var success = result.TryGetError(out var actualError);

        // Assert
        success.Should().BeTrue();
        actualError.Should().Be(error);
    }

    [Fact]
    public void Constructor_ShouldThrowException_WhenErrorIsNull()
    {
        // Act
        var act = () => new ValueResult((IError)null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("Value cannot be null. (Parameter 'error')");
    }

    [Fact]
    public void Constructor_ShouldAssignError_WhenErrorIsStruct()
    {
        // Arrange
        var error = new StructError("Struct Error");

        // Act
        var result = new ValueResult(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void HasError_ShouldReturnTrue_WhenErrorMatchesType()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = new ValueResult(error);

        // Act
        var hasError = result.HasError<Error>();

        // Assert
        hasError.Should().BeTrue();
    }

    [Fact]
    public void HasError_ShouldReturnFalse_WhenErrorDoesNotMatchType()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = new ValueResult(error);

        // Act
        var hasError = result.HasError<CustomError>();

        // Assert
        hasError.Should().BeFalse();
    }

    [Fact]
    public void TryGetError_ShouldReturnTrue_WhenErrorExists()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = new ValueResult(error);

        // Act
        var success = result.TryGetError(out var retrievedError);

        // Assert
        success.Should().BeTrue();
        retrievedError.Should().Be(error);
    }

    [Fact]
    public void TryGetError_ShouldReturnFalse_WhenErrorDoesNotExist()
    {
        // Arrange
        var result = ValueResult.Ok();

        // Act
        var success = result.TryGetError(out var retrievedError);

        // Assert
        success.Should().BeFalse();
        retrievedError.Should().BeNull();
    }

    [Fact]
    public void TryGetError_Generic_ShouldReturnTrue_WhenErrorMatchesType()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = new ValueResult(error);

        // Act
        var success = result.TryGetError<Error>(out var retrievedError);

        // Assert
        success.Should().BeTrue();
        retrievedError.Should().Be(error);
    }

    [Fact]
    public void TryGetError_Generic_ShouldReturnFalse_WhenErrorDoesNotMatchType()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = new ValueResult(error);

        // Act
        var success = result.TryGetError<CustomError>(out var retrievedError);

        // Assert
        success.Should().BeFalse();
        retrievedError.Should().BeNull();
    }

    [Fact]
    public void ImplicitOperator_ShouldCreateValueResultFromError()
    {
        // Arrange
        var error = new Error("Test Error");

        // Act
        ValueResult result = error;

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Ok_ShouldCreateSuccessfulValueResult_WithValue()
    {
        // Arrange
        var value = "Success Value";

        // Act
        var result = ValueResult.Ok(value);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().Be(value);
        result.Error.Should().BeNull();
    }

    [Fact]
    public void Ok_ShouldCreateSuccessfulValueResult_WhenValueIsNull()
    {
        // Act
        var result = ValueResult.Ok<string?>(null);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeNull();
    }


    [Fact]
    public void Ok_ShouldThrowArgumentNullException_WhenAccessValueInFailedValueResult()
    {
        // Arrange
        var error = new Error("Test Error");

        // Act
        var act = () => ValueResult.Fail<string>(error).Value;

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Fail_ShouldCreateFailedValueResult_WithError()
    {
        // Arrange
        var error = new Error("Test Error");

        // Act
        var result = ValueResult.Fail<string>(error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be(error);
    }

    [Fact]
    public void Fail_ShouldThrowException_WhenErrorIsNull()
    {
        // Act
        var act = () => ValueResult.Fail<string>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("Value cannot be null. (Parameter 'error')");
    }
}