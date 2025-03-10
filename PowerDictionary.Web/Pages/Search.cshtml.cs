using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PowerDictionary.Web.Entities;
using PowerDictionary.Web.Extensions;

namespace PowerDictionary.Web.Pages;

public class Search : PageModel
{
    private readonly HttpClient _httpClient;
    public string SearchedWord { get; set; } = string.Empty;
    public WordDescription Word { get; set; }

    public Search(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://localhost:7066/v1");
    }

    public async Task<IActionResult> OnGet(string word)
    {
        SearchedWord = word;
        var url = $"{_httpClient.BaseAddress}/{word.RemoveSpecialChars().ToLower()}";
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<Response>();

        if (response.StatusCode == HttpStatusCode.OK && result is not null && result.Status)
        {
            Word = JsonSerializer.Deserialize<WordDescription>(result.Data,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Page();
        }

        return RedirectToPage("Error", new { message = result.Data.ToString() });
    }
}