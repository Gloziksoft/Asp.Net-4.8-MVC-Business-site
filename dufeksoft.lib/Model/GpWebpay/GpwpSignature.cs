using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace dufeksoft.lib.Model.GpWebpay
{
    public class GpwpSignature
    {
        public string Message { get; private set; }
        public string PrivateCertificateFile { get; private set; }
        public string Password { get; private set; }
        public string Signature { get; private set; }

        public string Error { get; private set; }

        /// <summary>
        /// Ze zpravy vytvori podpis pomoci certifikatu a vysledek zakoduje do Base64.
        /// Funkce pro vytvoreni Digestu pro podpis pozadavku
        /// </summary>
        /// <param name="message">Podepisovaná zpráva</param>
        /// <param name="privateKeyFile">Cesta k sukromnemu klucu vo formáte PEM</param>
        /// <param name="password">Heslo k certifikatu</param>
        /// <returns>Podpis zakódovaný do Base64</returns>
        public static GpwpSignature SignData(string message, string privateKeyFile, string password)
        {
            GpwpSignature ret = new GpwpSignature();
            ret.Message = Utf8ToAscii(message);
            ret.PrivateCertificateFile = privateKeyFile;
            ret.Password = password;

            try
            {
                byte[] msgData = Encoding.ASCII.GetBytes(ret.Message);

                //X509Certificate2 cert = new X509Certificate2(ret.PrivateCertificateFile, ret.Password, X509KeyStorageFlags.Exportable);
                //ret.Cert = cert.ToString();
                //ret.HasPrivateKey = cert.HasPrivateKey.ToString();
                //ret.PrivateKey = cert.PrivateKey.ToString();

                //// export private key to XML-file
                //RSACryptoServiceProvider rsaFromXml = null;
                //if (privateCertificateFile.ToLower().EndsWith(".pfx"))
                //{
                //    string privateKeyXmlFile = privateCertificateFile.ToLower().Replace(".pfx", ".xml");
                //    if (!File.Exists(privateKeyXmlFile))
                //    {
                //        File.WriteAllText(privateKeyXmlFile, cert.PrivateKey.ToXmlString(true));
                //    }
                //    else
                //    {
                //        rsaFromXml = new RSACryptoServiceProvider();
                //        // import private key from xml
                //        rsaFromXml.FromXmlString(File.ReadAllText(privateKeyXmlFile));
                //        ret.ProviderName = rsaFromXml.CspKeyContainerInfo.ProviderName;
                //        ret.ProviderType = rsaFromXml.CspKeyContainerInfo.ProviderType.ToString();
                //    }
                //}

                //ret.KeyExchangeAlgorithm = cert.PrivateKey.KeyExchangeAlgorithm;
                //ret.KeySize = cert.PrivateKey.KeySize.ToString();
                //ret.SignatureAlgorithm = cert.PrivateKey.SignatureAlgorithm;

                //RSACryptoServiceProvider rsaprov = RsaProviderFromPrivateKeyInPemFile(@"c:\_D\VS_SDS\biostore.sk\podklady\banky\GP_webpay\private-key\gpwebpay-pvk-20190912.key");
                //RSAPKCS1SignatureFormatter RSAFormatter = new RSAPKCS1SignatureFormatter(rsaFromXml != null ? rsaFromXml : cert.PrivateKey);
                //RSAFormatter.SetHashAlgorithm("SHA1");
                //byte[] signedHash = RSAFormatter.CreateSignature(hashResult);

                //ret.Signature = Convert.ToBase64String(signedHash);

                // Load private key
                var fileStream = File.OpenText(privateKeyFile);
                var pemReader = new PemReader(fileStream, new BCPassword(password));
                RsaPrivateCrtKeyParameters keyParameters = (RsaPrivateCrtKeyParameters)pemReader.ReadObject();

                // Create signature for the message using private key
                // ISigner signer = SignerUtilities.GetSigner("SHA1withRSA");
                RsaDigestSigner signer = new RsaDigestSigner(new Sha1Digest());
                signer.Init(true, keyParameters);
                signer.BlockUpdate(msgData, 0, msgData.Length);
                byte[] signedHash = signer.GenerateSignature();

                ret.Signature = Convert.ToBase64String(signedHash);

                //// Create hash to be signed
                //SHA1 sha = new SHA1CryptoServiceProvider();
                //byte[] hashResult = sha.ComputeHash(msgData);
                //// Create signer
                //RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(keyParameters);
                //RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
                //provider.ImportParameters(rsaParams);
                //RSAPKCS1SignatureFormatter rsaFormatter = new RSAPKCS1SignatureFormatter(provider);
                //rsaFormatter.SetHashAlgorithm("SHA1");
                //// Createsignature
                //byte[] signedHash2 = rsaFormatter.CreateSignature(hashResult);
                //string signature2 = Convert.ToBase64String(signedHash2);
                //if (ret.Signature == signature2)
                //{
                //    // Overenie spravnosti podpisu
                //    //
                //    RsaKeyParameters rsaPublic = new RsaKeyParameters(false, keyParameters.Modulus, keyParameters.PublicExponent);
                //    signer.Init(false, rsaPublic);
                //    signer.BlockUpdate(msgData, 0, msgData.Length);
                //    if (signer.VerifySignature(signedHash))
                //    {
                //        ret.Error = "podpis je rovnaky a je spravny";
                //    }
                //    else
                //    {
                //        ret.Error = "podpis je nespravny";
                //    }
                //}
                //else
                //{
                //    ret.Error = string.Format("{0} <> {1}", ret.Signature, signature2);
                //    ret.Signature = signature2;
                //}
            }
            catch (Exception exc)
            {
                ret.Error = exc.ToString();
            }

            return ret;
        }

        public static RSACryptoServiceProvider RsaProviderFromPrivateKeyInPemFile(string privateKeyPath, string password)
        {
            using (TextReader privateKeyTextReader = new StringReader(File.ReadAllText(privateKeyPath)))
            {
                PemReader pr = new PemReader(privateKeyTextReader, new BCPassword(password));
                RsaPrivateCrtKeyParameters keyParameters = (RsaPrivateCrtKeyParameters)pr.ReadObject();
                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters(keyParameters);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);
                return csp;
            }
        }

        public static string Utf8ToAscii(string input)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding ascii = Encoding.ASCII;

            return ascii.GetString(Encoding.Convert(utf8, ascii, utf8.GetBytes(input)));
        }
    }

    public class GpwpSignatureValidation
    {
        public bool IsValidSignature { get; private set; }
        public string Message { get; private set; }
        public string Digest { get; private set; }
        public string PublicCertificateFile { get; private set; }

        public string Error { get; private set; }

        /// <summary>
        /// Zkontroluje, zda zaslana odpoved je prava
        /// </summary>
        /// <param name="digest">Podpis zprávy</param>
        /// <param name="message">Zpráva</param>
        /// <param name="publicKeyFile">Verejny kluc</param>
        /// <returns></returns>
        public static GpwpSignatureValidation ValidateDigest(string digest, string message, string publicKeyFile)
        {
            GpwpSignatureValidation ret = new GpwpSignatureValidation();
            ret.Message = message;
            ret.Digest = digest;
            ret.PublicCertificateFile = publicKeyFile;

            try
            {
                byte[] digestData = Convert.FromBase64String(ret.Digest);
                byte[] msgData = Encoding.ASCII.GetBytes(ret.Message);

                // Load public key certificate
                var fileStream = File.OpenText(ret.PublicCertificateFile);
                var pemReader = new PemReader(fileStream);
                Org.BouncyCastle.X509.X509Certificate cert = (Org.BouncyCastle.X509.X509Certificate)pemReader.ReadObject();

                // Verify signature
                RsaDigestSigner signer = new RsaDigestSigner(new Sha1Digest());
                signer.Init(false, cert.GetPublicKey());
                signer.BlockUpdate(msgData, 0, msgData.Length);
                if (signer.VerifySignature(digestData))
                {
                    ret.IsValidSignature = true;
                }
                else
                {
                    ret.Error = "podpis je nespravny";
                }


                //X509Certificate2 cert = new X509Certificate2(privateCertificateFile, password, X509KeyStorageFlags.Exportable);

                //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                //rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;

                //byte[] data = new byte[message.Length];
                //data = System.Text.Encoding.GetEncoding(1250).GetBytes(message);

                //byte[] hashResult;
                //SHA1 sha = new SHA1CryptoServiceProvider();
                //hashResult = sha.ComputeHash(data);

                //RSAPKCS1SignatureDeformatter rsaDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                //rsaDeformatter.SetHashAlgorithm("SHA1");

                //if (rsaDeformatter.VerifySignature(hashResult, bDigest))
                //{
                //    ret = true;
                //}
            }
            catch (Exception exc)
            {
                ret.Error = exc.ToString();
            }
            return ret;
        }
    }

    public class BCPassword : IPasswordFinder
    {
        public string Password { get; private set; }

        public BCPassword(string password)
        {
            this.Password = password;
        }
        public char[] GetPassword()
        {
            return this.Password.ToCharArray();
        }
    }
}
