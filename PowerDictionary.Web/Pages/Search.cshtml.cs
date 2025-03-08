using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerDictionary.Web.Extensions;

namespace PowerDictionary.Web.Pages;

public class Search : PageModel
{
    public string SearchedWord { get; set; } = string.Empty;
    
    public void OnGet(string word)
    {
        SearchedWord = word;
    }
}