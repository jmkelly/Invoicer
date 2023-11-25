using System.Text;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Invoicer.Web.Pages.Invoices
{
    public static class TempDataExtensions
    {
        public static void AddMessage(this ITempDataDictionary tempData, string message)
        {
            var messages = tempData["errors"] as List<string> ?? new List<string>();
            messages.Add(message);
            tempData["errors"] = messages;
        }

        public static string GetErrorsAsHtml(this ITempDataDictionary tempData)
        {
            var messages = tempData["errors"] as List<string> ?? new List<string>();
            var sb = new StringBuilder();
            foreach (var message in messages)
            {
                sb.Append(message);
                sb.Append("<br/>");
            }

            return sb.ToString();
        }
    }
}
