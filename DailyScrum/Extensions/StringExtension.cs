using System.Text;

namespace DailyScrum.Extensions
{
    public static class StringExtension
    {
        public static string ReplaceHTMLTags(this string text)
        {
            StringBuilder stringBuilder = new StringBuilder(text);

            stringBuilder.Replace("<", "&lt;");
            stringBuilder.Replace(">", "&gt;");

            return stringBuilder.ToString();
        }
    }
}
