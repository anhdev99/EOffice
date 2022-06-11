using System;
using EOffice.WebAPI.Models;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace EOffice.WebAPI.Services
{
    public static class SignDigitalService
    {
        public static void GeneratSignKey(ref User model)
        {
            RsaKeyPairGenerator rsaKey = new RsaKeyPairGenerator();
            rsaKey.Init(new Org.BouncyCastle.Crypto.KeyGenerationParameters(new SecureRandom(), 2048));
            AsymmetricCipherKeyPair keyPair = rsaKey.GenerateKeyPair();

            RsaKeyParameters Private_Key = (RsaKeyParameters)keyPair.Private;
            RsaKeyParameters Public_Key = (RsaKeyParameters)keyPair.Public;

            model.PrivateKey_string = PrivateKeytoString(Private_Key);
            model.PublicKey_string = PublicKeytoString(Public_Key);
        }
        
        private static string PublicKeytoString(RsaKeyParameters _key)
        {
            byte[] publicKeyDer = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(_key).GetDerEncoded();
            String publicKeyDerBase64 = Convert.ToBase64String(publicKeyDer);
            return publicKeyDerBase64;
        }
        
        public static string PrivateKeytoString(RsaKeyParameters _key)
        {

            PrivateKeyInfo privateKeyInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(_key);
            // Write out an RSA private key with it's asscociated information as described in PKCS8.
            byte[] serializedPrivateBytes = privateKeyInfo.ToAsn1Object().GetDerEncoded();
            // Convert to Base64 ..
            string serializedPrivateString = Convert.ToBase64String(serializedPrivateBytes);
            return serializedPrivateString;
        }
    }
}