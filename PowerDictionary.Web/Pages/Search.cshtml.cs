using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PowerDictionary.Web.Pages;

public class Search : PageModel
{
    public string SearchedWord { get; set; } = string.Empty;
    
    public void OnGet(string word)
    {
        SearchedWord = word;
    }
}