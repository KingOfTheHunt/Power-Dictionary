namespace PowerDictionary.API.Entities;

public class WordDescription
{
    public string Word { get; set; }
    public string GrammarClass { get; set; }
    public List<string> Meanings { get; set; }
    public string Etymology { get; set; }

    public WordDescription(string word, string grammarClass, List<string> meanings, string etymology)
    {
        Word = word;
        GrammarClass = grammarClass;
        Meanings = meanings;
        Etymology = etymology;
    }
}