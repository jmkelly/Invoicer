using Microsoft.AspNetCore.Mvc.Rendering;
using Invoicer.Web.Pages.Clients.Models;

namespace Invoicer.Web.Pages.Hours;

public static class CreateHoursModelExtensions
{
	public static List<SelectListItem> ToSelectListItem(this List<Client> clients)
	{
		return clients.Select(c => new SelectListItem
		{
			Value = c.Id.ToString(),
			Text = c.Name
		}).ToList();
	}
}

