using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Base
{
    using System.Net.Mime;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Windows.Forms;

    using LogicNP.CryptoLicensing;

    public enum LicenseStatus
    {
        LicenseOk,
        LicenseNotLoaded,
        LicenseExpired
    }

    public class Licensing
    {
        CryptoLicense CreateLicense()
        {
            CryptoLicense ret = new CryptoLicense();
            ret.ValidationKey = InternalSignature.SignatureLicKey;
            return ret;
        }
        public LicenseStatus EvaluateLicense()
        {
            CryptoLicense license = CreateLicense();

            // The license will be loaded from/saved to the registry
            license.StorageMode = LicenseStorageMode.ToRegistry;
            license.RegistryStoragePath = license.RegistryStoragePath + "InternalSignature";

            // Load the license from the registry 
            if (!license.Load())
                return LicenseStatus.LicenseNotLoaded;
            if (license.Status != LogicNP.CryptoLicensing.LicenseStatus.Valid)
                return LicenseStatus.LicenseExpired;

            return LicenseStatus.LicenseOk;

        }
    }
}
