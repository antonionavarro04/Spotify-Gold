using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL {
    internal static class TokenUtils {
        public static string GenerateToken(string username) {
            // First Part is Gonna be the current year, month and day
            // Second Part is Gonna be a random string of 30 characters
            // Third Part is Gonna be the Username
            string token = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.{RandomCharacters(30)}.{username}";

            return token;
        }

        public static string RandomCharacters(int length) {
            string random = "";
            Random randomGenerator = new();
            for (int i = 0; i < length; i++) {
                random += (char) (randomGenerator.Next(33, 126));
            }
            return random;
        }
    }
}
