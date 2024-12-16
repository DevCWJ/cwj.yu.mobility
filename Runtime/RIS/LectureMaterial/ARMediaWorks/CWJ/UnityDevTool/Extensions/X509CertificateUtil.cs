
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System;
using System.Text.RegularExpressions;

namespace CWJ
{
    public static class X509CertificateUtil
    {
        /// <summary>
        /// 
        /// </summary>
        public static X509Certificate2 ConvertToX509Certificate2(string strData, string password)
        {
            return new X509Certificate2(
                Convert.FromBase64String(Regex.Replace(Regex.Replace(strData, @"\s+", string.Empty), @"-+[^-]+-+", string.Empty))
                , password, X509KeyStorageFlags.Exportable);
        }

        const string BeginCert = "-----BEGIN CERTIFICATE-----";
        const string EndCert = "-----END CERTIFICATE-----";
        public static string ConvertPfxToStringData(string path, string password)
        {
            var certificate = new X509Certificate2(path, password, X509KeyStorageFlags.Exportable);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine(BeginCert);
            builder.AppendLine(Convert.ToBase64String(certificate.Export(X509ContentType.Pkcs12, password), Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine(EndCert);
            return builder.ToString();
        }
    }
}
