namespace Invoicer.Web.Pages.MyAccount
{
    public class MyAccount : Entity
    {
        public string? CompanyName { get; set; }
        public required string Name { get; set; }
        public required string BSB { get; set; }
        public required string AccountNo { get; set; }
        public required string StreetNumber { get; set; }
        public required string Street { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string Postcode { get; set; }
        public required string BankName { get; set; }
        public string PayId { get; set; }
		public string ABN {get;set;}

		public string Label()
		{
			return $"{Name} ({BSB}-{AccountNo})";
		}
    }
}
