using System.ComponentModel.DataAnnotations;

namespace Invoicer.Web.Pages.WorkItems;

public enum RateUnits
{
    [Display(Name = "Per Hour")]
    PerHour,
    [Display(Name = "Per Day")]
    PerDay
}