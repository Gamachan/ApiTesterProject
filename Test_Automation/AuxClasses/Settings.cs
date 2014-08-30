using System.Configuration;

namespace Test_Automation.AuxClasses
{
    public class Settings
    {
        public string Uri
        {
            get { return ConfigurationManager.AppSettings["BaseUrl"]; }
        }

        public string SigningCertificatePath
        {
            get { return ConfigurationManager.AppSettings["SigningCertificate"]; }
        }

        public string Password
        {
            get { return ConfigurationManager.AppSettings["Password"]; }
        }

        public string Key
        {
            get { return ConfigurationManager.AppSettings["ConsumerKey"]; }
        }

        public string Secret
        {
            get { return ConfigurationManager.AppSettings["ConsumerSecret"]; }
        }
    }
}