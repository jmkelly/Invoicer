using Invoicer.Web.Pages.Invoices;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shouldly;
using Xunit;

namespace Invoicer.Tests.Extensions;

public class EnumHelperTests
{
    [Fact]
    public void ToSelectList_WithSimpleEnum_ShouldReturnCorrectItems()
    {
        // Act
        var result = EnumHelper.ToSelectList<TestEnum>();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(3);
        
        result[0].Value.ShouldBe("0");
        result[0].Text.ShouldBe("Value1");
        
        result[1].Value.ShouldBe("1");
        result[1].Text.ShouldBe("Value2");
        
        result[2].Value.ShouldBe("2");
        result[2].Text.ShouldBe("Value3");
    }

    [Fact]
    public void ToSelectList_WithEnumWithCustomValues_ShouldReturnCorrectItems()
    {
        // Act
        var result = EnumHelper.ToSelectList<TestEnumWithCustomValues>();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(3);
        
        result[0].Value.ShouldBe("10");
        result[0].Text.ShouldBe("CustomValue1");
        
        result[1].Value.ShouldBe("20");
        result[1].Text.ShouldBe("CustomValue2");
        
        result[2].Value.ShouldBe("30");
        result[2].Text.ShouldBe("CustomValue3");
    }

    [Fact]
    public void ToSelectList_WithSingleValueEnum_ShouldReturnOneItem()
    {
        // Act
        var result = EnumHelper.ToSelectList<SingleValueEnum>();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].Value.ShouldBe("0");
        result[0].Text.ShouldBe("SingleValue");
    }

    [Fact]
    public void ToSelectList_WithEmptyEnum_ShouldReturnEmptyList()
    {
        // Act
        var result = EnumHelper.ToSelectList<EmptyEnum>();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(0);
    }

    [Fact]
    public void ToSelectList_WithInvoiceStatusEnum_ShouldReturnCorrectItems()
    {
        // Act
        var result = EnumHelper.ToSelectList<InvoiceStatus>();

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBeGreaterThan(0);
        
        // Verify that all items have valid values and text
        foreach (var item in result)
        {
            item.Value.ShouldNotBeNullOrEmpty();
            item.Text.ShouldNotBeNullOrEmpty();
            int.TryParse(item.Value, out _).ShouldBeTrue();
        }
    }

    private enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }

    private enum TestEnumWithCustomValues
    {
        CustomValue1 = 10,
        CustomValue2 = 20,
        CustomValue3 = 30
    }

    private enum SingleValueEnum
    {
        SingleValue
    }

    private enum EmptyEnum
    {
        // No values
    }
} 