using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace catedra3.src.Helpers
{
    public class RandomStringGenerator
    {
        private static readonly Random _random = new Random();
    
        // Characters to use in the random string
        private const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
        private const string Numbers = "0123456789";
        
        public static string Generate()
        {
            // Get random length between 10 and 15
            int length = _random.Next(10, 16);
            
            // Combine all character sets
            string allChars = Uppercase + Lowercase + Numbers;
            
            // Generate random string
            return new string(Enumerable.Repeat(allChars, length)
                .Select(s => s[_random.Next(s.Length)])
                .ToArray());
        }
        
    }
}