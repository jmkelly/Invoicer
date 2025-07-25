using Invoicer.Web.Entities;
using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.Hours;
using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.MyAccount;
using Shouldly;
using Xunit;

namespace Invoicer.Tests.Entities;

public class InvoiceTests
{
    [Fact]
    public void Total_WithNoHours_ShouldReturnZero()
    {
        // Arrange
        var invoice = CreateTestInvoice();

        // Act
        var result = invoice.Total();

        // Assert
        result.ShouldBe(0.0m);
    }

    [Fact]
    public void Total_WithSingleHour_ShouldCalculateCorrectly()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        var hours = CreateTestHours(10.0m, 50.0m, RateUnits.PerHour);
        invoice.Hours.Add(hours);

        // Act
        var result = invoice.Total();

        // Assert
        result.ShouldBe(500.0m); // 10 hours * $50/hour = $500
    }

    [Fact]
    public void Total_WithMultipleHours_ShouldCalculateCorrectly()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        invoice.Hours.Add(CreateTestHours(5.0m, 100.0m, RateUnits.PerHour)); // $500
        invoice.Hours.Add(CreateTestHours(8.0m, 75.0m, RateUnits.PerHour));  // $600
        invoice.Hours.Add(CreateTestHours(8.0m, 200.0m, RateUnits.PerDay));  // $200 (8 hours = 1 day)

        // Act
        var result = invoice.Total();

        // Assert
        result.ShouldBe(1300.0m); // $500 + $600 + $200 = $1300
    }

    [Fact]
    public void Total_WithNullHours_ShouldReturnZero()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        invoice.Hours = null!;

        // Act
        var result = invoice.Total();

        // Assert
        result.ShouldBe(0.0m);
    }

    [Fact]
    public void Total_WithEmptyHours_ShouldReturnZero()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        invoice.Hours.Clear();

        // Act
        var result = invoice.Total();

        // Assert
        result.ShouldBe(0.0m);
    }

    [Fact]
    public void IsAllowedToBeDeleted_WithCreatedStatus_ShouldReturnTrue()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        invoice.InvoiceStatus = InvoiceStatus.Created;

        // Act
        var result = invoice.IsAllowedToBeDeleted();

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void IsAllowedToBeDeleted_WithSentStatus_ShouldReturnFalse()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        invoice.InvoiceStatus = InvoiceStatus.Sent;

        // Act
        var result = invoice.IsAllowedToBeDeleted();

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void IsAllowedToBeDeleted_WithPaidStatus_ShouldReturnFalse()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        invoice.InvoiceStatus = InvoiceStatus.Paid;

        // Act
        var result = invoice.IsAllowedToBeDeleted();

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void RemoveAllHours_WithExistingHours_ShouldClearHoursList()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        invoice.Hours.Add(CreateTestHours(5.0m, 100.0m, RateUnits.PerHour));
        invoice.Hours.Add(CreateTestHours(8.0m, 75.0m, RateUnits.PerHour));
        
        invoice.Hours.Count.ShouldBe(2); // Verify we have hours before removal

        // Act
        invoice.RemoveAllHours();

        // Assert
        invoice.Hours.Count.ShouldBe(0);
    }

    [Fact]
    public void RemoveAllHours_WithEmptyHours_ShouldNotThrow()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        invoice.Hours.Clear();

        // Act & Assert - should not throw
        invoice.RemoveAllHours();
        invoice.Hours.Count.ShouldBe(0);
    }

    [Fact]
    public void RemoveAllHours_WithNullHours_ShouldNotThrow()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        invoice.Hours = null!;

        // Act & Assert - should not throw
        invoice.RemoveAllHours();
    }

    [Fact]
    public void AddHours_WithMatchingClient_ShouldAddHoursSuccessfully()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        var hours = CreateTestHours(5.0m, 100.0m, RateUnits.PerHour);
        hours.ClientId = invoice.Client.Id; // Match the client

        // Act
        var result = invoice.AddHours(hours);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        invoice.Hours.Count.ShouldBe(1);
        invoice.Hours[0].ShouldBe(hours);
    }

    [Fact]
    public void AddHours_WithDifferentClient_ShouldReturnFailure()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        var hours = CreateTestHours(5.0m, 100.0m, RateUnits.PerHour);
        hours.ClientId = Guid.NewGuid(); // Different client

        // Act
        var result = invoice.AddHours(hours);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("Work client differs from the invoice client");
        invoice.Hours.Count.ShouldBe(0);
    }

    [Fact]
    public void AddHours_WithNullHours_ShouldReturnFailure()
    {
        // Arrange
        var invoice = CreateTestInvoice();

        // Act
        var result = invoice.AddHours(null!);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.Error.ShouldContain("Hours cannot be null");
        invoice.Hours.Count.ShouldBe(0);
    }

    [Fact]
    public void AddHours_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var invoice = CreateTestInvoice();
        var originalUpdatedAt = invoice.UpdatedAt;
        var hours = CreateTestHours(5.0m, 100.0m, RateUnits.PerHour);
        hours.ClientId = invoice.Client.Id;

        // Act
        var result = invoice.AddHours(hours);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        invoice.UpdatedAt.ShouldBeGreaterThan(originalUpdatedAt);
    }

    [Fact]
    public void Constructor_ShouldSetDefaultValues()
    {
        // Act
        var invoice = new Invoice();

        // Assert
        invoice.Hours.ShouldNotBeNull();
        invoice.Hours.ShouldBeEmpty();
        invoice.CreatedAt.ShouldBeGreaterThan(DateTime.UtcNow.AddSeconds(-1));
        invoice.InvoiceDate.ShouldBeGreaterThan(DateTime.UtcNow.AddSeconds(-1));
        invoice.UpdatedAt.ShouldBeGreaterThan(DateTime.UtcNow.AddSeconds(-1));
    }

    private Invoice CreateTestInvoice()
    {
        var clientId = Guid.NewGuid();
        var accountId = Guid.NewGuid();

        var client = new Client
        {
            Id = clientId,
            Name = "Test Client",
            CompanyName = "Test Company",
            ClientCode = "TEST"
        };

        var account = new MyAccount.MyAccount
        {
            Id = accountId,
            Name = "Test Account",
            BSB = "123-456",
            AccountNo = "12345678",
            StreetNumber = "123",
            Street = "Test Street",
            City = "Test City",
            State = "NSW",
            Postcode = "2000",
            BankName = "Test Bank"
        };

        return new Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceCode = "INV-001",
            Client = client,
            Account = account,
            InvoiceStatus = InvoiceStatus.Created,
            CreatedAt = DateTime.UtcNow,
            InvoiceDate = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    private Invoicer.Web.Entities.Hours CreateTestHours(decimal numberOfHours, decimal rate, RateUnits rateUnits)
    {
        return new Invoicer.Web.Entities.Hours
        {
            Id = Guid.NewGuid(),
            Date = DateOnly.FromDateTime(DateTime.Today),
            NumberOfHours = numberOfHours,
            Description = "Test work",
            Rate = rate,
            RateUnits = rateUnits,
            ClientId = Guid.NewGuid(),
            DateRecorded = DateTime.UtcNow
        };
    }
} 