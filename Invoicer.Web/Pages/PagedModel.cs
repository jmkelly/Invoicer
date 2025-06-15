using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Invoicer.Web.Pages;

public class PagedModel : PageModel
{

	[BindProperty(SupportsGet = true)]

	public string Search { get; set; } = string.Empty;

	public int PageNum { get; set; } = 1;
	public int PageSize { get; set; } = 20;
	internal int Skip { get; set; } = 0;

	public void SetPaging(string? search, int? pageNum, int? pageSize)
	{
		PageSize = pageSize ?? 20;
		PageNum = pageNum ?? 1;
		Skip = (PageNum - 1) * PageSize;
		Search = search ?? string.Empty;
	}

}

