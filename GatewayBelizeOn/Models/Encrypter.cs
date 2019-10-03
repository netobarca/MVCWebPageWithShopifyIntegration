using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace GatewayBelizeOn.Models
{
    public class Encrypter
    {
        /// <summary>
        /// Method to Encrypt the password with SHA256 before to search the user
        /// </summary>
        /// <param name="value">password</param>
        /// <returns>string</returns>
        public static string EncryptSHA256(string value)
        {
            if (value != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(value);
                SHA256Managed sha256hashString = new SHA256Managed();

                byte[] hash = sha256hashString.ComputeHash(bytes);
                string hashString = string.Empty;

                foreach (byte x in hash)
                {
                    hashString += String.Format("{0:x2}", x);
                }
                return hashString;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}