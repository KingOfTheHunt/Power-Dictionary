namespace PowerDictionary.API.Exceptions;

public class WordNotFoundException(string message) : Exception(message)
{
}