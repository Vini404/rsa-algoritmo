using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace RSAEncryption
{
    class Encrypt
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
            BigInteger n = BigInteger.Parse(keyLines[0]);
            BigInteger e = BigInteger.Parse(keyLines[1]);

            string plaintext = File.ReadAllText(inputFile);

            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            string base64Text = Convert.ToBase64String(plaintextBytes);

            byte[] base64Bytes = Encoding.UTF8.GetBytes(base64Text);
            BigInteger chunkSize = CalculateChunkSize(n);
            BigInteger encodedChunk;
            StringBuilder encodedChunksBuilder = new StringBuilder();
            for (int i = 0; i < base64Bytes.Length; i += (int)chunkSize)
            {
                byte[] chunk = new byte[Math.Min((int)chunkSize, base64Bytes.Length - i)];
                Array.Copy(base64Bytes, i, chunk, 0, chunk.Length);
                encodedChunk = BigInteger.ModPow(new BigInteger(chunk), e, n);
                encodedChunksBuilder.AppendLine(encodedChunk.ToString());
            }

            // Writing encrypted data to output file
            File.WriteAllText(outputFile, encodedChunksBuilder.ToString());
        }

        static BigInteger CalculateChunkSize(BigInteger n)
        {
            int bitLength = n.ToByteArray().Length * 8;
            int byteLength = (bitLength + 7) / 8;
            int maxChunkSize = byteLength - 1;
            return new BigInteger(maxChunkSize);
        }
    }
}
