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
        var result2 = ValueResult.Fail<int>(error);

        // Act
        var success = result.TryGetError(out var actualError);
        var success2 = result2.TryGetError(out var actualError2);

        // Assert
        success.Should().BeTrue();
        success2.Should().BeTrue();
        actualError.Should().Be(error);
        actualError2.Should().Be(error);
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
        var result2 = new ValueResult<int>(error);

        // Act
        var hasError = result.HasError<Error>();
        var hasError2 = result2.HasError<Error>();

        // Assert
        hasError.Should().BeTrue();
        hasError2.Should().BeTrue();
    }

    [Fact]
    public void HasError_ShouldReturnFalse_WhenErrorDoesNotMatchType()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = new ValueResult(error);
        var result2 = new ValueResult<int>(error);

        // Act
        var hasError = result.HasError<CustomError>();
        var hasError2 = result2.HasError<CustomError>();

        // Assert
        hasError.Should().BeFalse();
        hasError2.Should().BeFalse();
    }

    [Fact]
    public void TryGetError_ShouldReturnTrue_WhenErrorExists()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = new ValueResult(error);
        var result2 = new ValueResult<int>(error);

        // Act
        var success = result.TryGetError(out var retrievedError);
        var success2 = result2.TryGetError(out var retrievedError2);

        // Assert
        success.Should().BeTrue();
        success2.Should().BeTrue();
        retrievedError.Should().Be(error);
        retrievedError2.Should().Be(error);
    }

    [Fact]
    public void TryGetError_ShouldReturnFalse_WhenErrorDoesNotExist()
    {
        // Arrange
        var result = ValueResult.Ok();
        var result2 = ValueResult.Ok(1);

        // Act
        var success = result.TryGetError(out var retrievedError);
        var success2 = result2.TryGetError(out var retrievedError2);

        // Assert
        success.Should().BeFalse();
        success2.Should().BeFalse();
        retrievedError.Should().BeNull();
        retrievedError2.Should().BeNull();
    }

    [Fact]
    public void TryGetError_Generic_ShouldReturnTrue_WhenErrorMatchesType()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = new ValueResult(error);
        var result2 = new ValueResult<int>(error);

        // Act
        var success = result.TryGetError<Error>(out var retrievedError);
        var success2 = result2.TryGetError<Error>(out var retrievedError2);

        // Assert
        success.Should().BeTrue();
        success2.Should().BeTrue();
        retrievedError.Should().Be(error);
        retrievedError2.Should().Be(error);
    }

    [Fact]
    public void TryGetError_Generic_ShouldReturnFalse_WhenErrorDoesNotMatchType()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = new ValueResult(error);
        var result2 = new ValueResult<int>(error);

        // Act
        var success = result.TryGetError<CustomError>(out var retrievedError);
        var success2 = result2.TryGetError<CustomError>(out var retrievedError2);

        // Assert
        success.Should().BeFalse();
        success2.Should().BeFalse();
        retrievedError.Should().BeNull();
        retrievedError2.Should().BeNull();
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
        var result = new ValueResult<string>(value);

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
        var result = new ValueResult<string?>(value: null);

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

    [Fact]
    public void FailIfNull_ShouldReturnSameValueResult_WhenValueIsNotNull()
    {
        // Arrange
        var value = "Test Value";
        var result = ValueResult.Ok(value);
        var error = new Error("Test Error");

        // Act
        var newResult = result.FailIfNull(error);

        // Assert
        newResult.Should().Be(result);
        newResult.IsSuccess.Should().BeTrue();
        newResult.Value.Should().Be(value);
    }

    [Fact]
    public void FailIfNull_ShouldReturnFailedValueResult_WhenValueIsNull()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = ValueResult.Ok<string?>(null);

        // Act
        var newResult = result.FailIfNull(error);

        // Assert
        newResult.Should().NotBe(result);
        newResult.IsFailure.Should().BeTrue();
        newResult.Error.Should().Be(error);
    }

    [Fact]
    public void FailIfNull_ShouldThrowException_WhenErrorIsNull()
    {
        // Arrange
        var result = ValueResult.Ok<string?>(null);

        // Act
        var act = () => result.FailIfNull(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
           .WithMessage("Value cannot be null. (Parameter 'error')");
    }

    [Fact]
    public void TryGetValue_ShouldReturnTrueAndSetValue_WhenValueResultIsSuccessful()
    {
        // Arrange
        var value = "Test Value";
        var result = ValueResult.Ok(value);

        // Act
        var success = result.TryGetValue(out var retrievedValue);

        // Assert
        success.Should().BeTrue();
        retrievedValue.Should().Be(value);
    }

    [Fact]
    public void TryGetValue_ShouldReturnFalseAndSetNull_WhenValueResultIsFailed()
    {
        // Arrange
        var error = new Error("Test Error");
        var result = ValueResult.Fail<string>(error);

        // Act
        var success = result.TryGetValue(out var retrievedValue);

        // Assert
        success.Should().BeFalse();
        retrievedValue.Should().BeNull();
    }
}