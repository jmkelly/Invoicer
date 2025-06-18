namespace Invoicer.Web.Pages.MyAccount
{
	public class Create : CreatePageModel<MyAccountCreateModel, MyAccount>
	{
		public Create(SqliteContext context, ILogger<CreatePageModel<MyAccountCreateModel, MyAccount>> logger) : base(context, logger)
		{
			logger.LogInformation("Create Account");
		}
	}
}
