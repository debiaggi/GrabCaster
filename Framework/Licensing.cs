using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework
{
    using System.Net.Mime;
    using System.Windows.Forms;

    using GrabCaster.Framework.Base;

    using LogicNP.CryptoLicensing;

    using LicenseStatus = LogicNP.CryptoLicensing.LicenseStatus;

    public class Licensing
    {
        CryptoLicense CreateLicense()
        {
            CryptoLicense ret = new CryptoLicense();
            ret.ValidationKey = InternalSignature.SignatureLicKey;
            return ret;
        }
        public bool EvaluateLicense()
        {
            var licenseValid = false;

            CryptoLicense license = CreateLicense();

            // The license will be loaded from/saved to the registry
            license.StorageMode = LicenseStorageMode.ToRegistry;

            // To avoid conflicts with other scenarios from this sample, the default load/save registry key is changed
            license.RegistryStoragePath = license.RegistryStoragePath + "InternalSignature";

            // The remove method can be useful during development and testing - it deletes a previously saved license.
           license.Remove();

            // Another useful method during development and testing is .ResetEvaluationInfo()



            // Load the license from the registry 
            bool loadDialog = !license.Load() || license.Status != LicenseStatus.Valid;
            
            if (loadDialog)
            {
                string dialogMessage = !license.Load()?"Licensing missing, enter the license key": license.Status != LicenseStatus.Valid?"Licensing expired, enter a new license key": "Licensing missing, enter the license key";
                // When app runs for first time, the load will fail, so specify an evaluation code....
                // This license code was generated from the Generator UI with a "Limit Usage Days To" setting of 30 days.
                LicenseForm licenseForm = new LicenseForm();
                licenseForm.labelMessage.Text = dialogMessage;
                if (licenseForm.ShowDialog() == DialogResult.OK)
                {

                    string licenseKey= licenseForm.textBoxLicense.Text;
                    license.LicenseCode = licenseKey;
                    // Save it so that it will get loaded the next time app runs
 

                    if (license.Status != LicenseStatus.Valid)
                    {
                        licenseValid = false;

                    }
                    else
                    {
                        license.Save();
                        licenseValid = true;
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            if (license.Status != LicenseStatus.Valid)
            {
                licenseValid = false;

            }
            else
            {
                licenseValid = true;
            }

            return licenseValid;
  
        }
    }
}
