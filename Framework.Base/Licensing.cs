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
        public bool EvaluateLicense(ref string message)
        {
            CryptoLicense license = CreateLicense();
            message = !license.Load() ? "Licensing missing, execute GrabCaster in console mode and enter the license key." : license.Status != LogicNP.CryptoLicensing.LicenseStatus.Valid ? "Licensing expired, execute GrabCaster in console mode and enter the license key." : "Licensing missing, execute GrabCaster in console mode and enter the license key.";

            // Load the license from the registry 
            if (!license.Load() || license.Status != LogicNP.CryptoLicensing.LicenseStatus.Valid) 
                return false;
            else return true;
 
        }
    }
}
