namespace Invoicer.Web.Pages.MyAccount
{
	public class Edit : EditPageModel<MyAccountEditModel, MyAccount>
	{
		public Edit(SqliteContext context, ILogger<EditPageModel<MyAccountEditModel, MyAccount>> logger) : base(context, logger)
		{
			logger.LogInformation("Edit Account");
		}
	}
}
