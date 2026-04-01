using API.Controllers;
using Moq;
using Xunit;
using Application.UseCase.Transactions.Queries.GetTransactionId;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using System.Text.Json;
using Application.DTOs.Transaction;
using Application.Common.Exceptions;

namespace UnitTests;

public class GetIdTransactionTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TransactionsController _controller;
    private readonly ITestOutputHelper _output;

    public GetIdTransactionTest(ITestOutputHelper output)
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TransactionsController(_mediatorMock.Object);
        _output = output;
    }
    [Fact]
    public async Task GetTransactionId_WithExistingId_ReturnsOkResult()
    {
        // Arrange
        long transactionId = 1;
        var expextedResponse = new GetTransactionIdResponse
        {
          Amount = 50000,
          Status = Domain.Entities.TransactionStatus.Completed,
          Type = Domain.Entities.TransactionType.Deposit,
          Description = "Deposit",
          CreatedAt = DateTime.UtcNow,
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetTransactionIdQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(expextedResponse);

        // Act
        var result = await _controller.GetTransactionId(transactionId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedValue = Assert.IsType<GetTransactionIdResponse>(okResult.Value);
        var jsonOutput = JsonSerializer.Serialize(returnedValue, new JsonSerializerOptions { WriteIndented = true});
        _output.WriteLine("=== DATA RESPONSE ===");
        _output.WriteLine(jsonOutput);
        _output.WriteLine("=== END DATA RESPONSE ===");
        Assert.Equal(expextedResponse.Amount, returnedValue.Amount);
    }
    [Fact]
    public async Task GetTransactionId_WithNonExistingId_ReturnsNotFound()
    {
        // Arrange
        long transactionId = 999;

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetTransactionIdQuery>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(new NotFoundException("Transaction not found"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() => _controller.GetTransactionId(transactionId));
        Assert.Equal("Transaction not found", exception.Message);
        
    }
    [Fact]
    public async Task GetTransactionId_WithValidationException_ReturnsBadRequest()
    {
        // Arrange
        long transactionId = -1;

        var validationFailure = new List<ValidationFailure>
        {
            new ValidationFailure("Id", "Id must be greater than 0")
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<GetTransactionIdQuery>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(new ValidationException(validationFailure));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ValidationException>(() => _controller.GetTransactionId(transactionId));
        Assert.Contains(exception.Errors, e => e.PropertyName == "Id");
    }
}