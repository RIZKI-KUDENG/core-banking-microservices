using API.Controllers;
using Moq;
using Xunit;
using Application.DTOs.Transaction;
using Application.UseCase.Transactions.Commands.CreateTransaction;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using System.Text.Json;

namespace UnitTests;

public class CreateTransactionTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TransactionsController _controller;
    private readonly ITestOutputHelper _output;

    public CreateTransactionTests(ITestOutputHelper output)
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TransactionsController(_mediatorMock.Object);
        _output = output;
    }

    [Fact]
    public async Task CreateTransaction_WithValidCommand_ReturnsOkResult()
    {
        // Arrange
        var command = new CreateTransactionCommand
        {
            SourceAccountId = 1,
            Amount = 50000,
            TransactionType = Domain.Entities.TransactionType.Deposit,
        };
        var expectedResponse = new CreateTransactionResponse
        {
           ReferenceNumber = Guid.NewGuid(),
           Amount = 50000,
           Status = Domain.Entities.TransactionStatus.Pending, 
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(expectedResponse);

        // Act
        var result = await _controller.CreateTransaction(command);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedValue = Assert.IsType<CreateTransactionResponse>(okResult.Value);
        var jsonOutput = JsonSerializer.Serialize(returnedValue, new JsonSerializerOptions { WriteIndented = true});
        _output.WriteLine("=== DATA RESPONSE ===");
        _output.WriteLine(jsonOutput);
        _output.WriteLine("=== END DATA RESPONSE ===");
        Assert.Equal(expectedResponse.Amount, returnedValue.Amount);
    }

    [Fact]
    public async Task CreateTransaction_WithValidException_ReturnsBadRequest()
    {
        // Arrange
        var command = new CreateTransactionCommand
        {
            SourceAccountId = 0,
            Amount = -10000
        };
        var validationFailure = new List<ValidationFailure>
        {
            new ValidationFailure("Amount", "Amount must be greater than Rp 25.000"),
            new ValidationFailure("SourceAccountId", "Source Account did not found")
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTransactionCommand>(), It.IsAny<CancellationToken>()))
        .ThrowsAsync(new ValidationException(validationFailure));

        // act
        var result = await _controller.CreateTransaction(command);

        //assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
       Assert.NotNull(badRequestResult.Value);
    }
}