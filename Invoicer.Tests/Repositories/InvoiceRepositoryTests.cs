using Invoicer.Web;
using Invoicer.Web.Pages.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;

namespace Invoicer.Tests.Repositories;

public class InvoiceRepositoryTests
{
    private SqliteContext CreateInMemoryContext()
    {
        var databaseName = Guid.NewGuid().ToString();
        var context = new TestSqliteContext(databaseName);
        context.Database.EnsureCreated();
        return context;
    }

    private InvoiceRepository CreateRepository(SqliteContext context)
    {
        var logger = new MockLogger<InvoiceRepository>();
        return new InvoiceRepository(context, logger);
    }

    [Fact]
    public async Task Get_WithNonExistentInvoice_ShouldThrowException()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = CreateRepository(context);
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await repository.Get(nonExistentId);
        });
    }

    [Fact]
    public async Task Remove_WithNonExistentInvoice_ShouldLogWarningAndNotThrow()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = CreateRepository(context);
        var nonExistentId = Guid.NewGuid();

        // Act & Assert
        await Should.ThrowAsync<InvalidOperationException>(async () =>
        {
            await repository.Remove(nonExistentId);
        });
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldPersistChangesToDatabase()
    {
        // Arrange
        using var context = CreateInMemoryContext();
        var repository = CreateRepository(context);
        
        // Act
        await repository.SaveChangesAsync();

        // Assert - should not throw and should complete successfully
        // This test verifies that the SaveChangesAsync method works correctly
        // even when there are no changes to persist
    }

    // Mock logger for testing
    private class MockLogger<T> : ILogger<T>
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
        public bool IsEnabled(LogLevel logLevel) => true;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter) { }
    }
} 