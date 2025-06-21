using Invoicer.Web.Entities;
using Invoicer.Web.Pages.Hours;
using Shouldly;
using Xunit;

namespace Invoicer.Tests.Entities;

public class HoursTests
{
    [Theory]
    [InlineData(0, 50, RateUnits.PerHour, 0)]
    [InlineData(8, 100, RateUnits.PerHour, 800)]
    [InlineData(4, 75, RateUnits.PerHour, 300)]
    [InlineData(8, 200, RateUnits.PerDay, 200)]
    [InlineData(4, 200, RateUnits.PerDay, 100)]
    [InlineData(16, 300, RateUnits.PerDay, 600)]
    public void Total_WithValidInputs_ShouldCalculateCorrectly(decimal hours, decimal rate, RateUnits units, decimal expected)
    {
        // Arrange
        var hoursEntity = CreateTestHours(hours, rate, units);

        // Act
        var result = hoursEntity.Total();

        // Assert
        result.ShouldBe(expected);
    }

    [Theory]
    [InlineData(-1, 50, RateUnits.PerHour)]
    [InlineData(8, -50, RateUnits.PerHour)]
    [InlineData(0, 0, RateUnits.PerHour)]
    public void Total_WithInvalidInputs_ShouldReturnZero(decimal hours, decimal rate, RateUnits units)
    {
        // Arrange
        var hoursEntity = CreateTestHours(hours, rate, units);

        // Act
        var result = hoursEntity.Total();

        // Assert
        result.ShouldBe(0);
    }

    [Fact]
    public void Total_WithUnknownRateUnits_ShouldThrowArgumentException()
    {
        // Arrange
        var hoursEntity = CreateTestHours(8, 100, (RateUnits)999);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => hoursEntity.Total());
        exception.Message.ShouldContain("Unknown rate units");
    }

    [Fact]
    public void HasInvoice_WithNullInvoice_ShouldReturnFalse()
    {
        // Arrange
        var hours = CreateTestHours(8, 100, RateUnits.PerHour);
        hours.Invoice = null;

        // Act
        var result = hours.HasInvoice();

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void HasInvoice_WithInvoice_ShouldReturnTrue()
    {
        // Arrange
        var hours = CreateTestHours(8, 100, RateUnits.PerHour);
        var client = new Invoicer.Web.Pages.Clients.Models.Client
        {
            Id = Guid.NewGuid(),
            Name = "Test Client",
            CompanyName = "Test Company",
            ClientCode = "TEST"
        };
        var account = new Invoicer.Web.Pages.MyAccount.MyAccount
        {
            Id = Guid.NewGuid(),
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
        hours.Invoice = new Invoicer.Web.Pages.Invoices.Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceCode = "INV-001",
            Client = client,
            Account = account,
            CreatedAt = DateTime.UtcNow,
            InvoiceDate = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            InvoiceStatus = Invoicer.Web.Pages.Invoices.InvoiceStatus.Created
        };

        // Act
        var result = hours.HasInvoice();

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void Constructor_ShouldSetRequiredProperties()
    {
        // Act
        var hours = new Hours
        {
            Id = Guid.NewGuid(),
            Date = DateOnly.FromDateTime(DateTime.Today),
            NumberOfHours = 8,
            Description = "Test work",
            Rate = 100,
            RateUnits = RateUnits.PerHour,
            ClientId = Guid.NewGuid(),
            DateRecorded = DateTime.UtcNow
        };

        // Assert
        hours.Id.ShouldNotBe(Guid.Empty);
        hours.Date.ShouldBe(DateOnly.FromDateTime(DateTime.Today));
        hours.NumberOfHours.ShouldBe(8);
        hours.Description.ShouldBe("Test work");
        hours.Rate.ShouldBe(100);
        hours.RateUnits.ShouldBe(RateUnits.PerHour);
        hours.ClientId.ShouldNotBe(Guid.Empty);
        hours.DateRecorded.ShouldBeGreaterThan(DateTime.UtcNow.AddSeconds(-1));
    }

    [Theory]
    [InlineData(0, 50, RateUnits.PerHour)]
    [InlineData(24, 50, RateUnits.PerHour)]
    [InlineData(8, 100, RateUnits.PerHour)]
    [InlineData(8, 200, RateUnits.PerDay)]
    public void Total_WithBoundaryValues_ShouldCalculateCorrectly(decimal hours, decimal rate, RateUnits units)
    {
        // Arrange
        var hoursEntity = CreateTestHours(hours, rate, units);

        // Act
        var result = hoursEntity.Total();

        // Assert
        result.ShouldBeGreaterThanOrEqualTo(0);
    }

    private Hours CreateTestHours(decimal numberOfHours, decimal rate, RateUnits rateUnits)
    {
        return new Hours
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