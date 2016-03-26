using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrabCaster.InternalLaboratory
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buttonAES_Click(object sender, EventArgs e)
        {
            //string original = "Here is some data to encrypt!";

            //// Create a new instance of the AesManaged
            //// class.  This generates a new key and initialization 
            //// vector (IV).
            //using (AesManaged myAes = new AesManaged())
            //{

            //    // Encrypt the string to an array of bytes.
            //    byte[] encrypted = AESEncryption.EncryptStringToBytes_Aes(original, myAes.Key, myAes.IV);

            //    // Decrypt the bytes to a string.
            //    string roundtrip = AESEncryption.DecryptStringFromBytes_Aes(encrypted, myAes.Key, myAes.IV);

            //    //Display the original data and the decrypted data.
            //    MessageBox.Show($"Original:   {original}");
            //    MessageBox.Show($"Round Trip: {roundtrip}");
            //}
        }

        private void buttonAESBytes_Click(object sender, EventArgs e)
        {
            byte[] contentFile = File.ReadAllBytes("c:\\test.txt");

            // Create a new instance of the AesManaged
            // class.  This generates a new key and initialization 
            // vector (IV).
            using (AesManaged myAes = new AesManaged())
            {

                // Encrypt the string to an array of bytes.
               
        //        byte[] encrypted = AESEncryption.EncryptByteToBytes_Aes(contentFile, myAes.Key, myAes.IV);
         //       File.WriteAllBytes("c:\\testCrypted.txt", encrypted);

                // Decrypt the bytes to a string.
         //       byte[] decrypted = AESEncryption.DecryptByteFromBytes_Aes(encrypted, myAes.Key, myAes.IV);
          //      File.WriteAllBytes("c:\\testdeCrypted.txt", decrypted);

                //Display the original data and the decrypted data.
                MessageBox.Show("done");

            }
        }
    }
}
