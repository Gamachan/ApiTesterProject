using Xero.Api.Core;
using Xero.Api.Infrastructure.OAuth;
using Xero.Api.Serialization;
using Test_Automation.AuxClasses;

namespace Test_Automation
{
    public class XeroTestCoreApi : XeroCoreApi
    {
        private static readonly DefaultMapper Mapper = new DefaultMapper();
        private static readonly Settings ApplicationSettings = new Settings();

        public XeroTestCoreApi() :
            base(ApplicationSettings.Uri,
                new PrivateAuthenticator(ApplicationSettings.SigningCertificatePath, ApplicationSettings.Password),
                new Consumer(ApplicationSettings.Key, ApplicationSettings.Secret),
                null,
                Mapper,
                Mapper)
        {
        }
    }
}
