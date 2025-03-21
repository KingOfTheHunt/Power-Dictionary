using System.Net;
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

    /**
     * Faz o download do html da página requerida e utiliza o Html Agility Pack
     * para navegar pelo do DOM e extrair os dados como classe gramatical, significados e etimologia da
     * palavra.
     */
    public async Task<WordDescription> GetWordDescription(string word)
    {
        var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}{word}");

        if (response.StatusCode == HttpStatusCode.NotFound)
            throw new WordNotFoundException($"A palavra {word} não foi encontrada.");
        
        var htmlContent = await response.Content.ReadAsStringAsync();
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