using Invoicer.Web.Pages.Clients.Models;
using Invoicer.Web.Pages.Hours;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shouldly;
using Xunit;

namespace Invoicer.Tests.Extensions;

public class CreateHoursModelExtensionsTests
{
    [Fact]
    public void ToSelectListItem_WithEmptyClientList_ShouldReturnEmptyList()
    {
        // Arrange
        var clients = new List<Client>();

        // Act
        var result = clients.ToSelectListItem();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void ToSelectListItem_WithSingleClient_ShouldReturnOneItem()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clients = new List<Client>
        {
            new Client
            {
                Id = clientId,
                Name = "Test Client",
                CompanyName = "Test Company",
                ClientCode = "TEST"
            }
        };

        // Act
        var result = clients.ToSelectListItem();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].Value.ShouldBe(clientId.ToString());
        result[0].Text.ShouldBe("Test Client");
    }

    [Fact]
    public void ToSelectListItem_WithMultipleClients_ShouldReturnAllItems()
    {
        // Arrange
        var client1Id = Guid.NewGuid();
        var client2Id = Guid.NewGuid();
        var client3Id = Guid.NewGuid();

        var clients = new List<Client>
        {
            new Client { Id = client1Id, Name = "Client 1", CompanyName = "Company 1", ClientCode = "CLI1" },
            new Client { Id = client2Id, Name = "Client 2", CompanyName = "Company 2", ClientCode = "CLI2" },
            new Client { Id = client3Id, Name = "Client 3", CompanyName = "Company 3", ClientCode = "CLI3" }
        };

        // Act
        var result = clients.ToSelectListItem();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(3);
        
        result[0].Value.ShouldBe(client1Id.ToString());
        result[0].Text.ShouldBe("Client 1");
        
        result[1].Value.ShouldBe(client2Id.ToString());
        result[1].Text.ShouldBe("Client 2");
        
        result[2].Value.ShouldBe(client3Id.ToString());
        result[2].Text.ShouldBe("Client 3");
    }

    [Fact]
    public void ToSelectListItem_WithNullClientName_ShouldHandleGracefully()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clients = new List<Client>
        {
            new Client
            {
                Id = clientId,
                Name = null,
                CompanyName = "Test Company",
                ClientCode = "TEST"
            }
        };

        // Act
        var result = clients.ToSelectListItem();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].Value.ShouldBe(clientId.ToString());
        result[0].Text.ShouldBeNull();
    }

    [Fact]
    public void ToSelectListItem_WithEmptyClientName_ShouldHandleGracefully()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var clients = new List<Client>
        {
            new Client
            {
                Id = clientId,
                Name = "",
                CompanyName = "Test Company",
                ClientCode = "TEST"
            }
        };

        // Act
        var result = clients.ToSelectListItem();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].Value.ShouldBe(clientId.ToString());
        result[0].Text.ShouldBe("");
    }
} 