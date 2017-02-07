using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Security.Cryptography;

namespace EntityFrameworkDatabaseFirst.Controllers
{
    public class myHwin
    {
        private const int SaltSize = 6;

        public byte[] GenerateNewSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[SaltSize];

                rng.GetBytes(randomNumber);

                return randomNumber;
            }
        }

        public byte[] ComputeHWIN_SHA256(byte[] data, byte[] salt)
        {
            using (var hwin = new HMACSHA256(salt))
            {
                return hwin.ComputeHash(data);
            }
        }

    }
}