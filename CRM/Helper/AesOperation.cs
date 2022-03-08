using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CRM.Helper
{
    public class AesOperation
    {
       
        public static string EncryptionString(string plainText, string key, string iv)
        {

            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);//"Lhy7c4rt93PAUGHl"
                aes.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public static string DecryptString(string key, string cipherText)
        {
            //  byte[] iv = new byte[16];

            var iv = Encoding.UTF8.GetBytes("7061747323313233");
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


        public static string DecryptStringAES(string encryptedValue)
        {
            var keybytes = Encoding.UTF8.GetBytes("AFUs9oL9yI7Lhy7c4rt93PAUGHlXfLaO4ffeVDKLXzPQ84iw/PWXykKre1rkOFLwV5V34Ng4VJTYWdmFd0qt9IB8/VfjRqX+m1des9YpfGUD20/SiYvkGvsm6dmg2fFu9GSdqjRCp579GjLmucUd9n62dsUTmJgv2EoDzMdwnyFD6HDmFwoB8uzYrU4RviM96p6covdV6sdSgqsjn5Xp0esa+vcnHIWU/wyp4lV0q8gc0Widi14Ht8K1rlTTs+Xuc2giDmBHYnwbEti1FujjuKtlUnDvvWDCHqsQzPJEYe+cfvrweiIMgbPgb6SyLiTIX0OMtbZxUB+xHm9FWbBGjUz0CNNtmoDT1HiJeH9nTfieATOaVRGGtliTckPgJ7ygcofE/FTzobl4/iaNxPVbNvVvegfIPVFGHke7s3wKAN9nKE6KWTcbN31F20VenurFa04lEVmW4/ssG1ofOAX38Wqzg50AGbMokf5HGm/29mULi4aH/uo/Mllx2piFHGChu6BIJu3g3gblbs0e2UaDlFeMqfIvZFOkZbFkzqh8WkDEzbxBm0vsAKfzIrUfOd7QJgJgPM3Swd+EC57yz92ulBVe1xtJoW5hZ98WCMzbXGo3CpizKVon5Y3fWC5Z4dN3m6Wt6993x31rrIF7N5yvzOVDpjw5t5CH1IPmYzEWhSTTU4SOrZ0jrs7uDbcuyp8a86R2U85q11sqVSYy2bFwLK1c3l1Ng4TFA8GDoI+ZJ/hE+wT3fm8mhP79LVysQAFW9z/oKM3l+Pd5VWL38r2KjXkKnPJkW3rjMPnDwlUueqVw6dRHBOA6yuY0mrZ4XDphpceqtK6KG2Sa1INyUTpNZarCuLsEMX568FIcxoKIzPurMVpfJzmsjxaYjzQ+Qsdv0AJeqjpxlvb0IxGvWcAQlW3KdAgZ0zWW+ZosAFX423aDzRaey+V/V26DCKkRCygFpIvF/8hAp8Q6QuE4NtVX7vwjSYISijrauLcc59pjGPJ+V3biT+QaKUQjmvcxv+rd");
            var iv = Encoding.UTF8.GetBytes("GFFGGHGFHJGFJGHJGHFDSGF2564789544EEEMHDFD+G");

            //DECRYPT FROM CRIPTOJS
            var encrypted = Convert.FromBase64String(encryptedValue);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);

            return decriptedFromJavascript;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }
    }
}
