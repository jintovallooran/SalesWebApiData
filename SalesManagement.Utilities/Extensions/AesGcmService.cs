using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;

namespace SalesManagement.Utilities.Extensions
{
    public static class AesGcmService
    {
        const string AesIV256 = "!QAZ12547689";
        static AesGcm GetAesGcmService()
        {
            const string password = "5TGB&YHN7UJM(IK<5TGB&YHN7UJM(IK<";

            byte[] finder = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(password), 100_000, HashAlgorithmName.SHA256).GetBytes(32);
            return new AesGcm(finder); ;
        }

        public static string Encrypt(this string plain)
        {

            try
            {
                if (plain == null)
                    throw new ArgumentNullException(nameof(plain));

                var encryptedData = AesGCM_Encrypt(Encoding.UTF8.GetBytes(plain).AsSpan());
                // Encode for transmission
                return ByteArrayToString(encryptedData);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public static byte[] EncryptByte(this byte[] input)
        {
            try
            {
                if (input == null) { throw new ArgumentNullException(nameof(input)); }
                var encryptedData = AesGCM_Encrypt(input.AsSpan());
                // Encode for transmission
                return encryptedData;
            }
            catch (Exception) { throw; }
        }
        public static string Decrypt(this string cipher)
        {
            // Decode
            Span<byte> encryptedData = StringToByteArray(cipher).AsSpan();

            // Convert plain bytes back into string
            return Encoding.UTF8.GetString(AesGCM_Decrypt(encryptedData));
        }

        public static byte[] DecryptByte(this byte[] cipher)
        {
            return AesGCM_Decrypt(cipher.AsSpan());
        }

        private static byte[] AesGCM_Encrypt(Span<byte> plainBytes)
        {
            try
            {
                var _aes = GetAesGcmService();
                // Get parameter sizes
                int nonceSize = AesGcm.NonceByteSizes.MaxSize;
                int tagSize = AesGcm.TagByteSizes.MaxSize;
                int cipherSize = plainBytes.Length;

                // We write everything into one big array for easier encoding
                int encryptedDataLength = 4 + nonceSize + 4 + tagSize + cipherSize;
                Span<byte> encryptedData = encryptedDataLength < 1024 ? stackalloc byte[encryptedDataLength] : new byte[encryptedDataLength].AsSpan();

                // Copy parameters
                BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(0, 4), nonceSize);
                BinaryPrimitives.WriteInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4), tagSize);
                var nonce = encryptedData.Slice(4, nonceSize);
                var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
                var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);
                var UTF = new UTF8Encoding();
                // Generate secure nonce
                // RandomNumberGenerator.Fill(nonce);
                UTF.GetBytes(AesIV256, nonce);
                // Encrypt
                _aes.Encrypt(nonce, plainBytes, cipherBytes, tag);
                return encryptedData.ToArray();
            }
            catch (Exception) { throw; }

        }

        private static byte[] AesGCM_Decrypt(Span<byte> encryptedData)
        {
            try
            {
                var _aes = GetAesGcmService();
                // Extract parameter sizes
                int nonceSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(0, 4));
                int tagSize = BinaryPrimitives.ReadInt32LittleEndian(encryptedData.Slice(4 + nonceSize, 4));
                int cipherSize = encryptedData.Length - 4 - nonceSize - 4 - tagSize;

                // Extract parameters
                var nonce = encryptedData.Slice(4, nonceSize);
                var tag = encryptedData.Slice(4 + nonceSize + 4, tagSize);
                var cipherBytes = encryptedData.Slice(4 + nonceSize + 4 + tagSize, cipherSize);

                // Decrypt
                Span<byte> plainBytes = cipherSize < 1024 ? stackalloc byte[cipherSize] : new byte[cipherSize];
                _aes.Decrypt(nonce, cipherBytes, tag, plainBytes);

                // Convert plain bytes back into string
                return plainBytes.ToArray();

            }
            catch (Exception) { throw; }

        }


        private static string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }
        private static byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }

}
