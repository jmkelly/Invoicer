namespace Invoicer.Web.Pages.WorkItems;

public class CreateWorkItemModel
{
    public DateOnly Date {get;set;}
    public decimal Hours {get;set;}
    public required string Description {get;set;}
    public decimal Rate {get;set;}
    public RateUnits RateUnits {get;set;}
    public Guid ClientId {get;set;}
}
