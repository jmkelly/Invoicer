using Invoicer.Web;
using Invoicer.Web.Pages.Clients;
using Invoicer.Web.Pages.Clients.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shouldly;
using Xunit;

namespace Invoicer.Tests;

public class ClientCodeGeneratorTests
{
    private SqliteContext CreateInMemoryContext()
    {
        var databaseName = Guid.NewGuid().ToString();
        var context = new TestSqliteContext(databaseName);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithCompanyName_ShouldGenerate4LetterCode()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "Microsoft Corporation";
        var clientName = "John Smith";

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.ShouldBe("MICR");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithSingleWordCompany_ShouldGenerate4LetterCode()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "Apple";
        var clientName = "John Smith";

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.ShouldBe("APPL");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithShortCompanyName_ShouldPadWithFirstLetter()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "IBM";
        var clientName = "John Smith";

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.ShouldBe("IBMI");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithNoCompanyName_ShouldUseClientName()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        string? companyName = null;
        var clientName = "John Smith";

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.ShouldBe("JOHN");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithMultipleWords_ShouldUseFirstLetters()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "Acme Corporation Limited";
        var clientName = "John Smith";

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.ShouldBe("ACME");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithDuplicateCode_ShouldGenerateDifferentCode()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "Microsoft Corporation";
        var clientName = "John Smith";

        // Add existing client with the same code
        var existingClient = new Client
        {
            Id = Guid.NewGuid(),
            Name = "Existing Client",
            ClientCode = "MICR",
            CompanyName = "Existing Company"
        };
        context.Clients.Add(existingClient);
        await context.SaveChangesAsync();

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.Length.ShouldBe(4);
        result.ShouldNotBe("MICR");
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithMultipleDuplicates_ShouldFindUniqueCode()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "Microsoft Corporation";
        var clientName = "John Smith";

        // Add multiple existing clients with variations
        var existingClients = new[]
        {
            new Client { Id = Guid.NewGuid(), Name = "Client 1", ClientCode = "MICR", CompanyName = "Company 1" },
            new Client { Id = Guid.NewGuid(), Name = "Client 2", ClientCode = "MICS", CompanyName = "Company 2" },
            new Client { Id = Guid.NewGuid(), Name = "Client 3", ClientCode = "MICT", CompanyName = "Company 3" },
            new Client { Id = Guid.NewGuid(), Name = "Client 4", ClientCode = "MICU", CompanyName = "Company 4" },
            new Client { Id = Guid.NewGuid(), Name = "Client 5", ClientCode = "MICV", CompanyName = "Company 5" },
            new Client { Id = Guid.NewGuid(), Name = "Client 6", ClientCode = "MICW", CompanyName = "Company 6" },
            new Client { Id = Guid.NewGuid(), Name = "Client 7", ClientCode = "MICX", CompanyName = "Company 7" },
            new Client { Id = Guid.NewGuid(), Name = "Client 8", ClientCode = "MICY", CompanyName = "Company 8" },
            new Client { Id = Guid.NewGuid(), Name = "Client 9", ClientCode = "MICZ", CompanyName = "Company 9" },
            new Client { Id = Guid.NewGuid(), Name = "Client 10", ClientCode = "MIAR", CompanyName = "Company 10" }
        };
        
        context.Clients.AddRange(existingClients);
        await context.SaveChangesAsync();

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.Length.ShouldBe(4);
        // Should not be any of the existing codes
        result.ShouldNotBe("MICR");
        result.ShouldNotBe("MICS");
        result.ShouldNotBe("MICT");
        result.ShouldNotBe("MICU");
        result.ShouldNotBe("MICV");
        result.ShouldNotBe("MICW");
        result.ShouldNotBe("MICX");
        result.ShouldNotBe("MICY");
        result.ShouldNotBe("MICZ");
        result.ShouldNotBe("MIAR");
        
        // Should be all uppercase letters
        result.ShouldMatch("^[A-Z]{4}$");
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithSpecialCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "A-B-C Corporation";
        var clientName = "John Smith";

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.ShouldBe("AAAA");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithEmptyString_ShouldHandleGracefully()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "";
        var clientName = "John Smith";

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.ShouldBe("JOHN");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_WithWhitespaceOnly_ShouldHandleGracefully()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "   ";
        var clientName = "John Smith";

        // Act
        var result = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);

        // Assert
        result.ShouldBe("JOHN");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public async Task GenerateUniqueClientCodeAsync_ShouldAlwaysReturnUniqueCodes()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var companyName = "Test Company";
        var clientName = "Test Client";

        // Act - Generate multiple codes for the same input
        var codes = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            var code = await ClientCodeGenerator.GenerateUniqueClientCodeAsync(context, companyName, clientName);
            codes.Add(code);
            // Add the generated code to the context so the next iteration will generate a different one
            context.Clients.Add(new Client { Id = Guid.NewGuid(), Name = $"Client {i}", ClientCode = code });
            await context.SaveChangesAsync();
        }

        // Assert - All codes should be unique
        codes.Distinct().Count().ShouldBe(codes.Count);
        codes.All(c => c.Length == 4).ShouldBeTrue();
        codes.All(c => c.All(char.IsUpper)).ShouldBeTrue();
    }

    [Fact]
    public void GenerateBaseCode_WithCompanyName_ShouldGenerateCorrectCode()
    {
        // Arrange
        var companyName = "Microsoft Corporation";

        // Act
        var result = ClientCodeGenerator.GenerateBaseCode(companyName);

        // Assert
        result.ShouldBe("MICR");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public void GenerateBaseCode_WithSingleWord_ShouldGenerateCorrectCode()
    {
        // Arrange
        var companyName = "Apple";

        // Act
        var result = ClientCodeGenerator.GenerateBaseCode(companyName);

        // Assert
        result.ShouldBe("APPL");
        result.Length.ShouldBe(4);
    }

    [Fact]
    public void GenerateBaseCode_WithShortWord_ShouldPadCorrectly()
    {
        // Arrange
        var companyName = "IBM";

        // Act
        var result = ClientCodeGenerator.GenerateBaseCode(companyName);

        // Assert
        result.ShouldBe("IBMI");
        result.Length.ShouldBe(4);
    }
} 