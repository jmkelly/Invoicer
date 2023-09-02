namespace Invoicer.Web.Pages.Creditors.Models
{
    public class Creditor
    {
        public Guid Id { get; set; }
        public string? CompanyName { get; set; }
        public required string Name { get; set; }
        public string? BSB { get; set; }
        public string? AccountNo { get; set; }
        public string? StreetNumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
