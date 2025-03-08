using System.Globalization;
using System.Text;

namespace PowerDictionary.Web.Extensions;

public static class StringExtensions
{
    public static string RemoveSpecialChars(this string value)
    {
        var sb = new StringBuilder();
        var normalized = value.Normalize(NormalizationForm.FormD);

        foreach (var letter in normalized)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                sb.Append(letter);
        }

        return sb.ToString().Normalize(NormalizationForm.FormC);
    }
}