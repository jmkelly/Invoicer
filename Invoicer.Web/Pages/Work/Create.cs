using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Work;
public class Create : PageModel
{
    private readonly DataContext context;

    public Create(DataContext context)
    {
        this.context = context;
    }

    [BindProperty]
    public Guid ClientId {get;set;}

    public CreateWorkModel Model {get;set;}

    public  List<SelectListItem> Options {get;set;}
    public async Task OnGet()
    {
        var clients =  await context.Clients.ToListAsync();
        Options = clients.Select(c => new SelectListItem {
            Value = c.Id.ToString(),
            Text = c.Name
        }).ToList();
        
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        //convert the work


        return RedirectToPage("./Index");
    }
}

public class CreateWorkModel : Work,  IModel
{
}

public class Work : Entity 
{
    public DateTime Date {get;set;}
    public decimal Hours {get;set;}
    public required string Description {get;set;}
    public decimal Rate {get;set;}
    public RateUnits RateUnits {get;set;}
    public Guid ClientId {get;set;}
}

public enum RateUnits
{
    [Display(Name = "Per Hour")]
    PerHour,
    [Display(Name = "Per Day")]
    PerDay
}