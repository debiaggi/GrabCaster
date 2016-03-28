// -----------------------------------------------------------------------------------
// 
// GRABCASTER LTD CONFIDENTIAL
// ___________________________
// 
// Copyright © 2013 - 2016 GrabCaster Ltd. All rights reserved.
// This work is registered with the UK Copyright Service: Registration No:284701085
// 
// 
// NOTICE:  All information contained herein is, and remains
// the property of GrabCaster Ltd and its suppliers,
// if any.  The intellectual and technical concepts contained
// herein are proprietary to GrabCaster Ltd
// and its suppliers and may be covered by UK and Foreign Patents,
// patents in process, and are protected by trade secret or copyright law.
// Dissemination of this information or reproduction of this material
// is strictly forbidden unless prior written permission is obtained
// from GrabCaster Ltd.
// 
// -----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrabCaster.Framework.Base
{
    using System.Diagnostics;
    using System.Net.Mime;
    using System.Runtime.InteropServices.WindowsRuntime;
    using System.Windows.Forms;

    using GrabCaster.Framework.Common;

    using LogicNP.CryptoLicensing;

    public enum LicenseStatus
    {
        LicenseOk,
        LicenseNotLoaded,
        LicenseExpired
    }

    public enum LicenseFeatures
    {
        Embedded = 1,
        Throttling,
        BizTalk,
        WindowsNt

    }
    public static class Licensing
    {
        static CryptoLicense CreateLicense()
        {
            CryptoLicense ret = new CryptoLicense();
            ret.ValidationKey = InternalSignature.SignatureLicKey;
            return ret;
        }
        public static bool EvaluateLicense(LicenseFeatures licenseFeatures,bool throwException)
        {
            bool valid = true;
            string message = "";

            CryptoLicense license = CreateLicense();
            license.StorageMode = LicenseStorageMode.ToRegistry;
            license.RegistryStoragePath = license.RegistryStoragePath + "InternalSignature";

            // Load the license from the registry 
            if (!license.Load())
            {

                message = "Licensing missing, execute GrabCaster in console mode and enter a valid license key.";
                valid = false;
            }
            if (!license.IsFeaturePresentEx((int)licenseFeatures))
            {
                message = $"The feature {licenseFeatures.ToString()} is not enabled , execute GrabCaster in console mode and enter a valid license key.";
                valid = false;
            }
            if (license.Status != LogicNP.CryptoLicensing.LicenseStatus.Valid)
            {
                message = "Licensing expired, execute GrabCaster in console mode and enter a valid license key.";
                valid = false;
            }

            if (!valid)
            {
                Methods.DirectEventViewerLog(message, EventLogEntryType.Error);
                if (throwException)
                {
                    throw new Exception(message);
                }

            }
            return valid;


        }
    }
}
