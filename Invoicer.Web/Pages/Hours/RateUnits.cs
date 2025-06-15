using System.ComponentModel.DataAnnotations;

namespace Invoicer.Web.Pages.Hours;

public enum RateUnits
{
	[Display(Name = "Per Hour")]
	PerHour,
	[Display(Name = "Per Day")]
	PerDay
}
