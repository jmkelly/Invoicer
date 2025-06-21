using Invoicer.Web.Pages.Invoices;
using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.MyAccount;
using Invoicer.Web.Pages.Hours;
using Invoicer.Web.Exceptions;
using Invoicer.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;

namespace Invoicer.Tests.Repositories;

public class InvoiceRepositoryTests : IDisposable
{
    private readonly TestSqliteContext _context;
    private readonly Mock<ILogger<InvoiceRepository>> _loggerMock;
    private readonly InvoiceRepository _repository;

    public InvoiceRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<Invoicer.Web.SqliteContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TestSqliteContext(options);
        _loggerMock = new Mock<ILogger<InvoiceRepository>>();
        _repository = new InvoiceRepository(_context, _loggerMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingInvoice_ShouldReturnInvoice()
    {
        // Arrange
        var invoice = await CreateAndSaveTestInvoice();

        // Act
        var result = await _repository.GetByIdAsync(invoice.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(invoice.Id);
        result.InvoiceCode.ShouldBe(invoice.InvoiceCode);
    }

    [Fact]
    public async Task GetByIdAsync_WithNonExistingInvoice_ShouldReturnNull()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistingId);

        // Assert
        result.ShouldBeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllInvoices()
    {
        // Arrange
        var invoice1 = await CreateAndSaveTestInvoice();
        var invoice2 = await CreateAndSaveTestInvoice();

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.ShouldContain(i => i.Id == invoice1.Id);
        result.ShouldContain(i => i.Id == invoice2.Id);
    }

    [Fact]
    public async Task GetByClientAsync_WithExistingClient_ShouldReturnClientInvoices()
    {
        // Arrange
        var client = await CreateAndSaveTestClient();
        var invoice1 = await CreateAndSaveTestInvoice(client);
        var invoice2 = await CreateAndSaveTestInvoice(client);
        var otherClient = await CreateAndSaveTestClient();
        var otherInvoice = await CreateAndSaveTestInvoice(otherClient);

        // Act
        var result = await _repository.GetByClientAsync(client.Id);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.ShouldContain(i => i.Id == invoice1.Id);
        result.ShouldContain(i => i.Id == invoice2.Id);
        result.ShouldNotContain(i => i.Id == otherInvoice.Id);
    }

    [Fact]
    public async Task GetByStatusAsync_WithExistingStatus_ShouldReturnStatusInvoices()
    {
        // Arrange
        var invoice1 = await CreateAndSaveTestInvoice(status: InvoiceStatus.Created);
        var invoice2 = await CreateAndSaveTestInvoice(status: InvoiceStatus.Created);
        var sentInvoice = await CreateAndSaveTestInvoice(status: InvoiceStatus.Sent);

        // Act
        var result = await _repository.GetByStatusAsync(InvoiceStatus.Created);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
        result.ShouldContain(i => i.Id == invoice1.Id);
        result.ShouldContain(i => i.Id == invoice2.Id);
        result.ShouldNotContain(i => i.Id == sentInvoice.Id);
    }

    [Fact]
    public async Task AddAsync_WithValidInvoice_ShouldAddAndReturnInvoice()
    {
        // Arrange
        var invoice = CreateTestInvoice();

        // Act
        var result = await _repository.AddAsync(invoice);

        // Assert
        result.ShouldNotBeNull();
        result.Id.ShouldBe(invoice.Id);
        result.InvoiceCode.ShouldBe(invoice.InvoiceCode);
        
        var savedInvoice = await _context.Invoices.FindAsync(invoice.Id);
        savedInvoice.ShouldNotBeNull();
    }

    [Fact]
    public async Task AddAsync_WithNullInvoice_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(async () =>
            await _repository.AddAsync(null!));
    }

    [Fact]
    public async Task UpdateAsync_WithValidInvoice_ShouldUpdateAndReturnInvoice()
    {
        // Arrange
        var invoice = await CreateAndSaveTestInvoice();
        var originalUpdatedAt = invoice.UpdatedAt;
        
        invoice.InvoiceCode = "UPDATED-001";

        // Act
        var result = await _repository.UpdateAsync(invoice);

        // Assert
        result.ShouldNotBeNull();
        result.InvoiceCode.ShouldBe("UPDATED-001");
        result.UpdatedAt.ShouldBeGreaterThan(originalUpdatedAt);
        
        var updatedInvoice = await _context.Invoices.FindAsync(invoice.Id);
        updatedInvoice.ShouldNotBeNull();
        updatedInvoice.InvoiceCode.ShouldBe("UPDATED-001");
    }

    [Fact]
    public async Task UpdateAsync_WithNullInvoice_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        await Should.ThrowAsync<ArgumentNullException>(async () =>
            await _repository.UpdateAsync(null!));
    }

    [Fact]
    public async Task DeleteAsync_WithDeletableInvoice_ShouldDeleteInvoice()
    {
        // Arrange
        var invoice = await CreateAndSaveTestInvoice(status: InvoiceStatus.Created);

        // Act
        await _repository.DeleteAsync(invoice.Id);

        // Assert
        var deletedInvoice = await _context.Invoices.FindAsync(invoice.Id);
        deletedInvoice.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteAsync_WithNonDeletableInvoice_ShouldThrowInvoiceNotDeletableException()
    {
        // Arrange
        var invoice = await CreateAndSaveTestInvoice(status: InvoiceStatus.Sent);

        // Act & Assert
        await Should.ThrowAsync<InvoiceNotDeletableException>(async () =>
            await _repository.DeleteAsync(invoice.Id));
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistingInvoice_ShouldNotThrow()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act & Assert - should not throw
        await _repository.DeleteAsync(nonExistingId);
    }

    [Fact]
    public async Task ExistsAsync_WithExistingInvoice_ShouldReturnTrue()
    {
        // Arrange
        var invoice = await CreateAndSaveTestInvoice();

        // Act
        var result = await _repository.ExistsAsync(invoice.Id);

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistingInvoice_ShouldReturnFalse()
    {
        // Arrange
        var nonExistingId = Guid.NewGuid();

        // Act
        var result = await _repository.ExistsAsync(nonExistingId);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task SearchAsync_WithSearchTerm_ShouldReturnMatchingInvoices()
    {
        // Arrange
        var client1 = await CreateAndSaveTestClient("Client A", "Company A");
        var client2 = await CreateAndSaveTestClient("Client B", "Company B");
        var invoice1 = await CreateAndSaveTestInvoice(client1, "INV-A-001");
        var invoice2 = await CreateAndSaveTestInvoice(client2, "INV-B-001");

        // Act
        var result = await _repository.SearchAsync("Client A", 0, 10);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(1);
        result.First().Id.ShouldBe(invoice1.Id);
    }

    [Fact]
    public async Task SearchAsync_WithEmptySearchTerm_ShouldReturnAllInvoices()
    {
        // Arrange
        var invoice1 = await CreateAndSaveTestInvoice();
        var invoice2 = await CreateAndSaveTestInvoice();

        // Act
        var result = await _repository.SearchAsync("", 0, 10);

        // Assert
        result.ShouldNotBeNull();
        result.Count().ShouldBe(2);
    }

    [Fact]
    public async Task GetTotalCountAsync_ShouldReturnCorrectCount()
    {
        // Arrange
        await CreateAndSaveTestInvoice();
        await CreateAndSaveTestInvoice();
        await CreateAndSaveTestInvoice();

        // Act
        var result = await _repository.GetTotalCountAsync();

        // Assert
        result.ShouldBe(3);
    }

    private async Task<Invoice> CreateAndSaveTestInvoice(
        Client? client = null, 
        string? invoiceCode = null, 
        InvoiceStatus status = InvoiceStatus.Created)
    {
        client ??= await CreateAndSaveTestClient();
        var account = await CreateAndSaveTestAccount();
        
        var invoice = CreateTestInvoice(client, account, invoiceCode, status);
        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();
        
        return invoice;
    }

    private async Task<Client> CreateAndSaveTestClient(string name = "Test Client", string companyName = "Test Company")
    {
        var client = new Client
        {
            Id = Guid.NewGuid(),
            Name = name,
            CompanyName = companyName,
            ClientCode = "TEST"
        };
        
        _context.Clients.Add(client);
        await _context.SaveChangesAsync();
        
        return client;
    }

    private async Task<MyAccount> CreateAndSaveTestAccount()
    {
        var account = new MyAccount
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
        
        _context.MyAccounts.Add(account);
        await _context.SaveChangesAsync();
        
        return account;
    }

    private Invoice CreateTestInvoice(
        Client? client = null, 
        MyAccount? account = null, 
        string? invoiceCode = null, 
        InvoiceStatus status = InvoiceStatus.Created)
    {
        var testClient = client ?? new Client 
        { 
            Id = Guid.NewGuid(), 
            Name = "Test Client", 
            CompanyName = "Test Company",
            ClientCode = "TEST"
        };
        
        var testAccount = account ?? new MyAccount 
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
        
        return new Invoice
        {
            Id = Guid.NewGuid(),
            InvoiceCode = invoiceCode ?? "INV-001",
            Client = testClient,
            Account = testAccount,
            InvoiceStatus = status,
            CreatedAt = DateTime.UtcNow,
            InvoiceDate = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
} 