using System.Security.Cryptography;
using System.Text;

namespace GiapDepTrai.Utilities
{
    public class Functions
    {
        public static int _UserID = 0;
        public static string _UserName = string.Empty;


        public static string MD5HashUp(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] resuit = md5.Hash;
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < resuit.Length; i++)
            {
                stringBuilder.Append(resuit[i].ToString("x2"));

            }
            return stringBuilder.ToString();

        }
        public static string MD5Password(string? text)
        {
            string str = MD5HashUp(text);
            for(int i = 0; i<= 5; i++)
            {
                str = MD5HashUp(str + str);
            }
            return str;
        }
    }
}
