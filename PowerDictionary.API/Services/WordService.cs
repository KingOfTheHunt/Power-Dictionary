using HtmlAgilityPack;
using PowerDictionary.API.Entities;
using PowerDictionary.API.Exceptions;

namespace PowerDictionary.API.Services;

public class WordService
{
    private readonly HttpClient _httpClient;

    public WordService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://www.dicio.com.br");
    }

    public async Task<WordDescription> GetWordDescription(string word)
    {
        var htmlContent = await _httpClient.GetStringAsync($"{_httpClient.BaseAddress}{word}");
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(htmlContent);

        var paragraph = htmlDocument.DocumentNode.SelectSingleNode("//p[@class='significado textonovo']");
        var meanings = new List<string>();

        if (paragraph == null)
            throw new WordNotFoundException($"A palavra {word} não foi encontrada.");

        var contents = paragraph.SelectNodes(".//span");
        if (contents == null)
            throw new HtmlElementNotFoundException("Não foi possível encontrar o elemento html.");
        
        var grammarClass = contents[0].InnerText;
        var etymology = contents.Last().InnerText;

        for (int i = 1; i < contents.Count - 1; i++)
        {
            if (contents[i].InnerText.StartsWith("[") && contents[i].InnerText.EndsWith("]"))
                continue;
            
            meanings.Add(contents[i].InnerText);
        }
        
        return new WordDescription(word, grammarClass, meanings, etymology);
    }
}