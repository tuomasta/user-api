using DataAccess;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace AuthenticationApi.Domain
{
    internal class JwtHelper
    {
        private string privateKey;

        public JwtHelper(string privateKey)
        {
            this.privateKey = privateKey;
        }

        internal JwtPayload CreatePayload(AuthentincationInfo auth)
        {
            var issued = DateTimeOffset.UtcNow;
            var expireAt = issued + TimeSpan.FromHours(4);
            var payload = new Dictionary<string, object>()
            {
                ["sub"] = auth.UserId,
                ["exp"] = expireAt.ToUnixTimeSeconds(),
                ["iat"] = issued.ToUnixTimeSeconds()
            };

            return new JwtPayload(payload, this);
        }

        internal string CreateToken(Dictionary<string, object> payload)
        {
            RSAParameters rsaParams;
            using (var tr = new StringReader(privateKey))
            {
                var pemReader = new PemReader(tr);
                var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair == null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256);
            }
        }
    }

    internal class JwtPayload
    {
        private readonly JwtHelper helper;
        public Dictionary<string, object> payload { get; }

        public JwtPayload(Dictionary<string, object> payload, JwtHelper helper)
        {
            this.payload = payload;
            this.helper = helper;
        }

        public string CreateToken()
        {
            return helper.CreateToken(payload);
        }
    }
}
