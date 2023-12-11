using System;
using System.Xml;

namespace VNet.Utility.Crypto
{
    public class AsymmetricKeyInfo
    {
        public AsymmetricPublicKey PublicKey { get; set; }
        public AsymmetricPrivateKey PrivateKey { get; set; }

        public AsymmetricKeyInfo()
        {
            this.PublicKey = new AsymmetricPublicKey();
            this.PrivateKey = new AsymmetricPrivateKey();
        }

        public void FromXml(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            this.PublicKey.Exponent = doc.SelectSingleNode("/RSAKeyValue/Exponent").InnerText;
            this.PublicKey.Modulus = doc.SelectSingleNode("/RSAKeyValue/Modulus").InnerText;

            this.PrivateKey.D = doc.SelectSingleNode("/RSAKeyValue/D").InnerText;
            this.PrivateKey.DP = doc.SelectSingleNode("/RSAKeyValue/DP").InnerText;
            this.PrivateKey.DQ = doc.SelectSingleNode("/RSAKeyValue/DQ").InnerText;
            this.PrivateKey.InverseQ = doc.SelectSingleNode("/RSAKeyValue/InverseQ").InnerText;
            this.PrivateKey.P = doc.SelectSingleNode("/RSAKeyValue/P").InnerText;
            this.PrivateKey.Q = doc.SelectSingleNode("/RSAKeyValue/Q").InnerText;
        }

        public string ToXml()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement root = doc.CreateElement("RSAKeyValue");
            doc.AppendChild(root);

            XmlElement modulus = doc.CreateElement("Modulus");
            modulus.InnerText = this.PublicKey.Modulus;
            root.AppendChild(modulus);

            XmlElement exponent = doc.CreateElement("Exponent");
            exponent.InnerText = this.PublicKey.Exponent;
            root.AppendChild(exponent);

            if(this.PrivateKey != null)
            {
                XmlElement p = doc.CreateElement("P");
                p.InnerText = this.PrivateKey.P;
                root.AppendChild(p);

                XmlElement q = doc.CreateElement("Q");
                q.InnerText = this.PrivateKey.Q;
                root.AppendChild(q);

                XmlElement dp = doc.CreateElement("DP");
                dp.InnerText = this.PrivateKey.DP;
                root.AppendChild(dp);

                XmlElement dq = doc.CreateElement("DQ");
                dq.InnerText = this.PrivateKey.DQ;
                root.AppendChild(dq);

                XmlElement inverseQ = doc.CreateElement("InverseQ");
                inverseQ.InnerText = this.PrivateKey.InverseQ;
                root.AppendChild(inverseQ);

                XmlElement d = doc.CreateElement("D");
                d.InnerText = this.PrivateKey.D;
                root.AppendChild(d);
            }

            return doc.InnerXml;
        }

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2001:AvoidCallingProblematicMethods", MessageId = "System.GC.Collect")]
        public AsymmetricKeyInfo ClonePublicKey()
        {
            AsymmetricKeyInfo publicKey = new AsymmetricKeyInfo();
            publicKey.PrivateKey = null;
            GC.Collect();

            publicKey.PublicKey.Exponent = this.PublicKey.Exponent;
            publicKey.PublicKey.Modulus = this.PublicKey.Modulus;

            return publicKey;
        }
    }
}
