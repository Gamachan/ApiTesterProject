using System;
using System.Security.Cryptography.X509Certificates;
using Xero.Api.Infrastructure.Interfaces;
using Xero.Api.Infrastructure.OAuth;
using Xero.Api.Infrastructure.OAuth.Signing;

namespace Test_Automation.AuxClasses
{
    public class PrivateAuthenticator : ICertificateAuthenticator
    {
        private readonly X509Certificate2 _certificate;

        public PrivateAuthenticator(string certificatePath, string password)
        {
            _certificate = new X509Certificate2();
            //_certificate.Import(certificatePath);
            //_certificate.Import(certificatePath, "qa1234", X509KeyStorageFlags.DefaultKeySet);
            _certificate.Import(certificatePath, password, X509KeyStorageFlags.DefaultKeySet);
        }

        public PrivateAuthenticator(X509Certificate2 certificate)
        {
            _certificate = certificate;
        }

        public string GetSignature(IConsumer consumer, IUser user, Uri uri, string verb, IConsumer consumer1)
        {
            return new RsaSha1Signer().CreateSignature(_certificate, new Token { ConsumerKey = consumer.ConsumerKey, ConsumerSecret = consumer.ConsumerSecret }, uri, verb);
        }

        public X509Certificate Certificate { get { return _certificate; } }

        public IToken GetToken(IConsumer consumer, IUser user)
        {
            return null;
        }

        public IUser User { get; set; }
    }
}