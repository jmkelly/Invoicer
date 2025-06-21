using Invoicer.Web.Entities;
using Invoicer.Web.Pages.Hours;
using Shouldly;
using Xunit;

namespace Invoicer.Tests.Entities;

public class HoursTests
{
    [Fact]
    public void Total_WithPerHourRate_ShouldCalculateCorrectly()
    {
        // Arrange
        var hours = new Hours
        {
            Id = Guid.NewGuid(),
            NumberOfHours = 10.0m,
            Rate = 50.0m,
            RateUnits = RateUnits.PerHour,
            Description = "Test work",
            Date = DateOnly.FromDateTime(DateTime.Now),
            DateRecorded = DateTime.Now,
            ClientId = Guid.NewGuid()
        };

        // Act
        var result = hours.Total();

        // Assert
        result.ShouldBe(500.0m); // 10 hours * $50/hour = $500
    }

    [Fact]
    public void Total_WithPerDayRate_ShouldCalculateCorrectly()
    {
        // Arrange
        var hours = new Hours
        {
            Id = Guid.NewGuid(),
            NumberOfHours = 24.0m, // 1 day
            Rate = 400.0m, // $400 per day
            RateUnits = RateUnits.PerDay,
            Description = "Test work",
            Date = DateOnly.FromDateTime(DateTime.Now),
            DateRecorded = DateTime.Now,
            ClientId = Guid.NewGuid()
        };

        // Act
        var result = hours.Total();

        // Assert
        // Formula: Rate * NumberOfHours / 24 / 8 = 400 * 24 / 24 / 8 = 400 / 8 = 50
        result.ShouldBe(50.0m);
    }

    [Fact]
    public void Total_WithPartialDay_ShouldCalculateCorrectly()
    {
        // Arrange
        var hours = new Hours
        {
            Id = Guid.NewGuid(),
            NumberOfHours = 8.0m, // 8 hours (1 work day)
            Rate = 400.0m, // $400 per day
            RateUnits = RateUnits.PerDay,
            Description = "Test work",
            Date = DateOnly.FromDateTime(DateTime.Now),
            DateRecorded = DateTime.Now,
            ClientId = Guid.NewGuid()
        };

        // Act
        var result = hours.Total();

        // Assert
        // Formula: Rate * NumberOfHours / 24 / 8 = 400 * 8 / 24 / 8 = 400 / 24 = 16.67
        result.ShouldBe(16.67m, 0.01m); // Allow for rounding
    }

    [Fact]
    public void Total_WithZeroHours_ShouldReturnZero()
    {
        // Arrange
        var hours = new Hours
        {
            Id = Guid.NewGuid(),
            NumberOfHours = 0.0m,
            Rate = 100.0m,
            RateUnits = RateUnits.PerHour,
            Description = "Test work",
            Date = DateOnly.FromDateTime(DateTime.Now),
            DateRecorded = DateTime.Now,
            ClientId = Guid.NewGuid()
        };

        // Act
        var result = hours.Total();

        // Assert
        result.ShouldBe(0.0m);
    }

    [Fact]
    public void Total_WithZeroRate_ShouldReturnZero()
    {
        // Arrange
        var hours = new Hours
        {
            Id = Guid.NewGuid(),
            NumberOfHours = 10.0m,
            Rate = 0.0m,
            RateUnits = RateUnits.PerHour,
            Description = "Test work",
            Date = DateOnly.FromDateTime(DateTime.Now),
            DateRecorded = DateTime.Now,
            ClientId = Guid.NewGuid()
        };

        // Act
        var result = hours.Total();

        // Assert
        result.ShouldBe(0.0m);
    }

    [Fact]
    public void Total_WithInvalidRateUnits_ShouldThrowException()
    {
        // Arrange
        var hours = new Hours
        {
            Id = Guid.NewGuid(),
            NumberOfHours = 10.0m,
            Rate = 100.0m,
            RateUnits = (RateUnits)999, // Invalid enum value
            Description = "Test work",
            Date = DateOnly.FromDateTime(DateTime.Now),
            DateRecorded = DateTime.Now,
            ClientId = Guid.NewGuid()
        };

        // Act & Assert
        Should.Throw<NotImplementedException>(() => hours.Total());
    }

    [Fact]
    public void HasInvoice_WithNullInvoice_ShouldReturnFalse()
    {
        // Arrange
        var hours = new Hours
        {
            Id = Guid.NewGuid(),
            NumberOfHours = 10.0m,
            Rate = 100.0m,
            RateUnits = RateUnits.PerHour,
            Description = "Test work",
            Date = DateOnly.FromDateTime(DateTime.Now),
            DateRecorded = DateTime.Now,
            ClientId = Guid.NewGuid(),
            Invoice = null
        };

        // Act
        var result = hours.HasInvoice();

        // Assert
        result.ShouldBeFalse();
    }
} 