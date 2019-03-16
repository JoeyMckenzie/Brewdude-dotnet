using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Brewdude.Common.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Extension method that interrogates the text in question for valid JSON structure.
        /// Suppresses exceptions thrown while attempting to parse string, will return false instead.
        /// </summary>
        /// <param name="text">String text passed from the caller</param>
        /// <returns>True, if string text conforms to valid JSON structure</returns>
        public static bool IsValidJson(this string text)
        {
            text = text.Trim();
            if (text.StartsWith("{") && text.EndsWith("}") ||
                    text.StartsWith("[") && text.EndsWith("]"))
            {
                try
                {
                    JToken.Parse(text);
                    return true;
                }
#pragma warning disable 168
                catch (JsonReaderException jsonReaderException)
#pragma warning restore 168
                {
                    return false;
                }
#pragma warning disable 168
                catch (Exception ex)
#pragma warning restore 168
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Determines of the string text is of an JSON object, or array, type.
        /// </summary>
        /// <param name="text">String text passed from the caller</param>
        /// <returns>True, if valid JSON structure characters are found</returns>
        private static bool ContainsValidJsonStructure(string text)
        {
            return text.StartsWith("{") && text.EndsWith("}") ||
                   text.StartsWith("[") && text.EndsWith("]");
        }
    }
}