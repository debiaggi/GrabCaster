using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrabCaster.Framework
{
    using GrabCaster.Framework.Base;

    using LogicNP.CryptoLicensing;

    using LicenseStatus = LogicNP.CryptoLicensing.LicenseStatus;

    public partial class LicenseForm : Form
    {
        CryptoLicense CreateLicense()
        {
            CryptoLicense ret = new CryptoLicense();
            ret.ValidationKey = InternalSignature.SignatureLicKey;
            return ret;
        }
        public LicenseForm()
        {
            InitializeComponent();
        }

        private void linkLabelContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://grabcaster.io/grabcaster-contact/");
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            CryptoLicense license = CreateLicense();

            license.StorageMode = LicenseStorageMode.ToRegistry;
            license.RegistryStoragePath = license.RegistryStoragePath + "InternalSignature";
            license.LicenseCode = textBoxLic.Text; ;
            if (license.Status != LicenseStatus.Valid)
            {
                MessageBox.Show(
                    "The current license key is not valid, contact the support.",
                    Configuration.EngineName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                license.Save();
                MessageBox.Show(
                    "License key valid, GrabCaster activated.",
                    Configuration.EngineName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                this.Close();
            }

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
