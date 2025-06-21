using Invoicer.Web.Pages.Clients.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoicer.Web.Pages.Clients
{
    public static class ClientCodeGenerator
    {
        public static async Task<string> GenerateUniqueClientCodeAsync(SqliteContext context, string? companyName, string clientName)
        {
            // Use company name if available, otherwise use client name
            var baseName = !string.IsNullOrWhiteSpace(companyName) ? companyName : clientName;
            
            // Generate the base 4-letter code
            var baseCode = GenerateBaseCode(baseName);
            
            // Check if the code already exists
            var existingCode = await context.Clients
                .Where(c => c.ClientCode == baseCode)
                .FirstOrDefaultAsync();
            
            if (existingCode == null)
            {
                return baseCode;
            }
            
            // If code exists, try variations by changing the last character
            for (int i = 1; i <= 26; i++)
            {
                var lastChar = (char)('A' + (i - 1));
                var newCode = baseCode.Substring(0, 3) + lastChar;
                
                var existingWithVariation = await context.Clients
                    .Where(c => c.ClientCode == newCode)
                    .FirstOrDefaultAsync();
                
                if (existingWithVariation == null)
                {
                    return newCode;
                }
            }
            
            // If all variations are taken, try changing the third character
            for (int i = 1; i <= 26; i++)
            {
                var thirdChar = (char)('A' + (i - 1));
                var newCode = baseCode.Substring(0, 2) + thirdChar + baseCode[3];
                
                var existingWithVariation = await context.Clients
                    .Where(c => c.ClientCode == newCode)
                    .FirstOrDefaultAsync();
                
                if (existingWithVariation == null)
                {
                    return newCode;
                }
            }
            
            // If still no luck, try changing the second character
            for (int i = 1; i <= 26; i++)
            {
                var secondChar = (char)('A' + (i - 1));
                var newCode = baseCode[0] + secondChar + baseCode.Substring(2, 2);
                
                var existingWithVariation = await context.Clients
                    .Where(c => c.ClientCode == newCode)
                    .FirstOrDefaultAsync();
                
                if (existingWithVariation == null)
                {
                    return newCode;
                }
            }
            
            // If still no luck, try changing the first character
            for (int i = 1; i <= 26; i++)
            {
                var firstChar = (char)('A' + (i - 1));
                var newCode = firstChar + baseCode.Substring(1, 3);
                
                var existingWithVariation = await context.Clients
                    .Where(c => c.ClientCode == newCode)
                    .FirstOrDefaultAsync();
                
                if (existingWithVariation == null)
                {
                    return newCode;
                }
            }
            
            // Fallback: generate a completely random 4-letter code
            return await GenerateRandomCode(context);
        }
        
        public static string GenerateBaseCode(string name)
        {
            // Remove special characters and split into words
            var words = name.Split(new[] { ' ', '-', '_', '.', ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (words.Length == 1)
            {
                // Single word: take first 4 letters, or pad with first letter
                var word = words[0].ToUpperInvariant();
                if (word.Length >= 4)
                {
                    return word.Substring(0, 4);
                }
                else
                {
                    return word.PadRight(4, word[0]);
                }
            }
            else
            {
                // Multiple words: take first 4 letters of the first word
                var firstWord = words[0].ToUpperInvariant();
                if (firstWord.Length >= 4)
                {
                    return firstWord.Substring(0, 4);
                }
                else
                {
                    return firstWord.PadRight(4, firstWord[0]);
                }
            }
        }
        
        private static async Task<string> GenerateRandomCode(SqliteContext context)
        {
            var random = new Random();
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            
            while (true)
            {
                var code = new string(Enumerable.Repeat(letters, 4)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                
                var existing = await context.Clients
                    .Where(c => c.ClientCode == code)
                    .FirstOrDefaultAsync();
                
                if (existing == null)
                {
                    return code;
                }
            }
        }
    }
} 