using Invoicer.Web.Extensions;
using Shouldly;
using Xunit;

namespace Invoicer.Tests.Extensions;

public class ObjectExtensionsTests
{
    [Fact]
    public void ToJson_WithSimpleObject_ShouldSerializeCorrectly()
    {
        // Arrange
        var testObject = new { Name = "John", Age = 30 };

        // Act
        var result = testObject.ToJson();

        // Assert
        result.ShouldContain("John");
        result.ShouldContain("30");
        result.ShouldContain("Name");
        result.ShouldContain("Age");
    }

    [Fact]
    public void ToJson_WithComplexObject_ShouldSerializeCorrectly()
    {
        // Arrange
        var testObject = new
        {
            Name = "Test Company",
            Address = new { Street = "123 Main St", City = "Anytown" },
            Tags = new[] { "tag1", "tag2" }
        };

        // Act
        var result = testObject.ToJson();

        // Assert
        result.ShouldContain("Test Company");
        result.ShouldContain("123 Main St");
        result.ShouldContain("Anytown");
        result.ShouldContain("tag1");
        result.ShouldContain("tag2");
    }

    [Fact]
    public void ToJson_WithNullObject_ShouldHandleGracefully()
    {
        // Arrange
        object? testObject = null;

        // Act
        var result = testObject.ToJson();

        // Assert
        result.ShouldBe("null");
    }

    [Fact]
    public void HasDuplicates_WithNoDuplicates_ShouldReturnFalse()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var result = list.HasDuplicates();

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void HasDuplicates_WithDuplicates_ShouldReturnTrue()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 2, 5 };

        // Act
        var result = list.HasDuplicates();

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void HasDuplicates_WithEmptyList_ShouldReturnFalse()
    {
        // Arrange
        var list = new List<int>();

        // Act
        var result = list.HasDuplicates();

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void HasDuplicates_WithSingleItem_ShouldReturnFalse()
    {
        // Arrange
        var list = new List<int> { 1 };

        // Act
        var result = list.HasDuplicates();

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void HasDuplicates_WithAllSameItems_ShouldReturnTrue()
    {
        // Arrange
        var list = new List<int> { 1, 1, 1, 1 };

        // Act
        var result = list.HasDuplicates();

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void HasDuplicates_WithStringList_ShouldWorkCorrectly()
    {
        // Arrange
        var list = new List<string> { "apple", "banana", "apple", "cherry" };

        // Act
        var result = list.HasDuplicates();

        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public void HasDuplicates_WithCustomObject_ShouldWorkCorrectly()
    {
        // Arrange
        var list = new List<TestObject>
        {
            new TestObject { Id = 1, Name = "Test1" },
            new TestObject { Id = 2, Name = "Test2" },
            new TestObject { Id = 1, Name = "Test1" } // Duplicate
        };

        // Act
        var result = list.HasDuplicates();

        // Assert
        result.ShouldBeTrue();
    }

    private class TestObject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is not TestObject other) return false;
            return Id == other.Id && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
    }
} 