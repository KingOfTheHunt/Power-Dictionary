using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerDictionary.Web.Pages;

public class ErrorModel : PageModel
{
    public string Message { get; set; } = string.Empty;

    public void OnGet(string message)
    {
        Message = message;
    }
}