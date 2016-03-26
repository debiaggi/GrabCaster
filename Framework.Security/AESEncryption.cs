using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using GrabCaster.Framework.Base;
using GrabCaster.Framework.Log;

namespace GrabCaster.Framework.Security
{
    public class AESEncryption
    {
        private static string SecurityFile = "KeyFile.gck";
        private static string SecurityPwdFile = "PwdKeyFile.gck";
        public static bool SetSecurity_Aes(string password)
        {

            byte[] encrypted = null;

            byte[] Key = new byte[32];
            byte[] IV = new byte[16];

            bool ret = false;

            try
            {
                // Create an AesManaged object
                // with the specified key and IV.
                using (AesManaged aesAlg = new AesManaged())
                {

                    byte[] keyContent = aesAlg.Key.Concat(aesAlg.IV).ToArray();
                    File.WriteAllBytes(Path.Combine(Configuration.BaseDirectory(), SecurityFile), keyContent);
                    aesAlg.Key = Key;
                    aesAlg.IV = IV;

                    byte[] passwordByte = Encoding.UTF8.GetBytes(password);

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {

                            csEncrypt.Write(passwordByte, 0, passwordByte.Length);
                            csEncrypt.FlushFinalBlock();
                            encrypted = msEncrypt.ToArray();
                            msEncrypt.Close();
                            csEncrypt.Close();
                        }
                    }
                    File.WriteAllBytes(Path.Combine(Configuration.BaseDirectory(), SecurityPwdFile), passwordByte);


                }
                ret = true;
            }
            catch (Exception ex)
            {


                LogEngine.WriteLog(Configuration.EngineName,
                              $"Error in {MethodBase.GetCurrentMethod().Name}",
                              Constant.DefconOne,
                              Constant.TaskCategoriesError,
                              ex,
                              EventLogEntryType.Error);
                ret = false;

            }


            return ret;

        }

        public static byte[] EncryptByteToBytes_Aes(byte[] byteContent,bool create)
        {

            byte[] encrypted = null;

            byte[] Key = new byte[32];
            byte[] IV = new byte[16];
            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                if (create)
                {
                    byte[] keyContent = aesAlg.Key.Concat(aesAlg.IV).ToArray();
                    File.WriteAllBytes("C:\\KeyFile.gck", keyContent);
                }
                if (!File.Exists("C:\\KeyFile.gck"))
                {

                }
                else
                {
                    byte[] contentKey = File.ReadAllBytes("C:\\KeyFile.gck");

                    Array.Copy(contentKey, 0, Key, 0, 32);
                    Array.Copy(contentKey, 32, IV, 0, 16);
                }
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {

                        csEncrypt.Write(byteContent, 0, byteContent.Length);
                        csEncrypt.FlushFinalBlock();
                        encrypted = msEncrypt.ToArray();
                        msEncrypt.Close();
                        csEncrypt.Close();
                    }
                }

            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        public static byte[] DecryptByteFromBytes_Aes(byte[] cipherText)
        {

            // the decrypted byte array.
            byte[] deCipherText = new byte[cipherText.Length]; ;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                byte[] Key = new byte[32];
                byte[] IV = new byte[16];
                Array.Clear(Key, 0, Key.Length);
                Array.Clear(Key, 0, Key.Length);


                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                            csDecrypt.Read(deCipherText, 0, deCipherText.Length);
                            msDecrypt.Close();
                        csDecrypt.Close();
                    }
                 }
                }
            return deCipherText;
        }




        public static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");
            byte[] encrypted;
            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {

                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        public static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("Key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
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