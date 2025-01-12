using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ESMLoop
{
    public static class Extensions
    {
        private static readonly string[] Classifiers = { "Word", "PowerPoint", "Excel", "Outlook", "Teams", "Zoom", "OneNote", "Discord", "Whatsapp", "Slack", "PowerShell", "TeamSpeak", "TexMaker", "Overleaf", "Mendeley", "Citavi", "Zotero", "Adobe" };
        private static readonly string[] Browsers = { "Brave", "Chrome", "Firefox", "Edge", "Internet Explorer", "Safari", "Opera", "SeaMonkey" };
        private static readonly string[] IDEs = { "Visual Studio", "Vim", "PyCharm", "Intellij", "Eclipse", "JetBrains", "RStudio" };

        /// <summary>
        ///     Classifies a string into pre defined groups
        /// </summary>
        /// <param name="input">string to be classified</param>
        /// <returns>anonymous string</returns>
        public static string MakeAnonymous(this string input)
        {
            foreach (string classifier in Classifiers)
            {
                if (input.Contains(classifier))
                {
                    return classifier;
                }
            }
            foreach (string browser in Browsers)
            {
                if (input.Contains(browser))
                {
                    return "Internet Browser";
                }
            }
            foreach (string ide in IDEs)
            {
                if (input.Contains(ide))
                {
                    return "IDE";
                }
            }
            //return input;
            return "other";
        }

        /// <summary>
        ///     Joins two Lists. Will work with null.
        /// </summary>
        /// <typeparam name="T">Type of List elements</typeparam>
        /// <param name="first">first list or null</param>
        /// <param name="second">second list or null</param>
        /// <returns>List of type T containing all elements from First and second in order</returns>
        public static List<T> Join<T>(this List<T>? first, List<T> second)
        {
            if (first == null)
            {
                return second;
            }
            if (second == null)
            {
                return first;
            }

            return first.Concat(second).ToList();
        }

        /// <summary>
        ///     Key used for Encryption. MUST BE 32CHAR HEXADECIMAL LONG. THIS KEY IS NOT HASHED BEFORE USE!
        /// </summary>
        private const string key = "b14ca5898a4e4133bbce2ea2315a1916";

        /// <summary>
        ///     Encrypts string using a const key
        /// </summary>
        /// <param name="plainText">plaintext</param>
        /// <returns>cipher</returns>
        public static string Encrypt(this string plainText)
        {
            using Aes aes = Aes.Create();

            ICryptoTransform encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(key), new byte[16]);

            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            using (StreamWriter streamWriter = new((Stream)cryptoStream))
            {
                streamWriter.Write(plainText);
            }
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        /// <summary>
        ///     Decrypts string using a const key
        /// </summary>
        /// <param name="cipherText">cipher</param>
        /// <returns>plaintext</returns>
        public static string Decrypt(this string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
        private const string CSV_SEPARATOR = ";";
        /// <summary>
        ///     Seperates all elememets of array with given seperator
        /// </summary>
        /// <param name="array">array of elements</param>
        /// <param name="separator">string used as separator</param>
        /// <returns>string containing all elemets separated by the separator</returns>
        public static string ToCSVString<T>(this T[] array)
        {
            return string.Join(CSV_SEPARATOR, array);
        }
        /// <summary>
        ///     Seperates all elememets of array with given seperator
        /// </summary>
        /// <param name="array">2D nested array of elements</param>
        /// <param name="separator">string used as separator</param>
        /// <returns>string containing all elemets separated by the separator</returns>
        public static string ToCSVString<T>(this T[,] array)
        {
            var one = new List<T>();
            foreach (var item in array)
            {
                one.Add(item);
            };
            return one.ToArray().ToCSVString();
        }
    }
}
