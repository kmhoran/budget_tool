using System;
using System.Text;
using Extentions.Text;

namespace Sheet.Common.Models
{
    public class GoogleConfig
    {
        // secrets currently pulled from config... gonna change this
        public string ServiceEmail { get; set; }
        public string PrivateKey { get; set; }
        private string _transformedPrivateKey;
        // config values not coming in as UTF-8 characters... hopefully remove
        public string TransformedPrivateKey { get{
return _transformedPrivateKey;
        } 
        set{
            byte[] bytesTest = new byte[4999];
            char[] charsTest = new char[5000];
            bytesTest = Encoding.Default.GetBytes(value);
            bytesTest = bytesTest.Replace(new byte[] { 0x5C, 0x6E }, new byte[] { 0x0A });
            bytesTest = bytesTest.Replace(new byte[] { 0x22 }, new byte[] { });
            int writtenTest = Encoding.Default.GetChars(bytesTest, 0, bytesTest.Length, charsTest, 0);
            _transformedPrivateKey = new string(charsTest, 0, 5000).Trim();

        } }
    }
}