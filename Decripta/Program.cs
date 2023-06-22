using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace RSAEncryption
{
    class Decrypt
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Modo de uso: <key_file> <input_file> <output_file>");
                return;
            }

            string keyFile = args[0];
            string inputFile = args[1];
            string outputFile = args[2];

            string[] keyLines = File.ReadAllLines(keyFile);
            var n = BigInteger.Parse(keyLines[0]);
            var d = BigInteger.Parse(keyLines[1]);

            string[] encodedChunks = File.ReadAllLines(inputFile);
            
            string plaintext = RSADecrypt(encodedChunks, d, n);

            File.WriteAllText(outputFile, plaintext);
        }

        static string RSADecrypt(string[] encodedChunks, BigInteger d, BigInteger n)
        {
            var decryptedData = new StringBuilder();
            
            foreach (string encodedChunk in encodedChunks)
            {
                var chunk = BigInteger.ModPow(BigInteger.Parse(encodedChunk), d, n);
                var decryptedChunk = Encoding.ASCII.GetString(chunk.ToByteArray());
                decryptedData.Append(decryptedChunk);
            }

            return decryptedData.ToString();
        }
    }
}