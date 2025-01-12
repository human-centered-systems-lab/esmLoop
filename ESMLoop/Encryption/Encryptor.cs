using ESMLoop.Logging;
using ESMLoop.LoggingData;
using System.IO;

namespace ESMLoop.Encryption
{
    internal class Encryptor
    {
        //private const string FILE_ENDING = "ENCRYPTED";
        private const string FILE_ENDING = "PASCAL";

        /// <summary>
        ///     Encrypts all Logging Data in a Session Folder
        /// </summary>
        /// <param name="path">dir path</param>
        public static void EncryptLoggingData(string path)
        {
            string fileEnding = AbstractLeafLogger<AbstractLoggingData>.FILE_ENDING;
            foreach (string file in Directory.GetFiles(path))
            {
                if (!File.Exists(file) || !Path.GetExtension(file).Contains(fileEnding)) continue;
                string enc = File.ReadAllText(file).Encrypt();
                File.WriteAllText(Path.ChangeExtension(file, FILE_ENDING), enc);
                File.Delete(file);
            }
        }
        /// <summary>
        ///     Decrypts all files with FILE_ENDING as file ending
        /// </summary>
        /// <param name="path">dir path</param>
        public static void DecryptLoggingData(string path)
        {
            foreach (string file in Directory.GetFiles(path, "*.*", SearchOption.AllDirectories))
            {
                if (!File.Exists(file) || !Path.GetExtension(file).Contains(FILE_ENDING)) continue;
                string enc = File.ReadAllText(file);
                File.WriteAllText(Path.ChangeExtension(file, "csv"), enc.Decrypt());
                File.Delete(file);
            }
        }
    }
}
