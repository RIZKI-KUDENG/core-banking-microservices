using API.Controllers;
using Moq;
using Xunit;
using Application.DTOs.Transaction;
using Application.UseCase.Transactions.Queries.GetReferenceNumber;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Xunit.Abstractions;
using System.Text.Json;


namespace UnitTests;

public class GetReferenceNumberTest
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly TransactionsController _controller;
    private readonly ITestOutputHelper _output;

    public GetReferenceNumberTest(ITestOutputHelper output)
    {
        _mediatorMock = new Mock<IMediator>();
        _controller = new TransactionsController(_mediatorMock.Object);
        _output = output;
    }

    [Fact]
    public async Task GetReferenceNumber_WithValidCommand_ReturnsOkResult()
    {
        // Arrange
            var referenceNumber = Guid.NewGuid().ToString();
        var query = new GetReferenceNumberResponse
        {
            ReferenceNumber = Guid.Parse(referenceNumber),
            Amount = 50000,
            Status = Domain.Entities.TransactionStatus.Completed,
            Type = Domain.Entities.TransactionType.Deposit,
            Description = "Deposit",
            CreatedAt = DateTime.UtcNow,
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<GetReferenceNumberQuery>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(query);

        // Act
        var result = await _controller.GetReferenceNumber(referenceNumber);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedValue = Assert.IsType<GetReferenceNumberResponse>(okResult.Value);
        var jsonOutput = JsonSerializer.Serialize(returnedValue, new JsonSerializerOptions { WriteIndented = true});
        _output.WriteLine("=== DATA RESPONSE ===");
        _output.WriteLine(jsonOutput);
        _output.WriteLine("=== END DATA RESPONSE ===");
        Assert.Equal(query.Amount, returnedValue.Amount);
    }
}