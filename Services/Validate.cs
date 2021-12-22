using System;
using System.Text.RegularExpressions;

namespace PGManagerApi.Services
{
    public static class Validate
    {
        public static string NoQuotes(string value, string name)
        {
            if (value != null && (value.Contains('\'') || value.Contains('"')))
            {
                throw new ArgumentException($"Value {name} (\"{value ?? "null"}\") should not contain quotes", name);
            }

            return value;
        }
    }
}