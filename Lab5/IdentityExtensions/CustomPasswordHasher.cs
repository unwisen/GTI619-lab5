using System;
using System.Collections;
using System.Security.Cryptography;
using Microsoft.AspNet.Identity;

namespace Lab5.IdentityExtensions
{
    public class CustomPasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            if (password == null) throw new ArgumentNullException("password");

            const int saltSize = 16;
            const int passwordHashedSize = 32;
            const int iterations = 1000;

            byte[] salt;
            byte[] passwordHashed;

            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltSize, iterations))
            {
                salt = rfc2898DeriveBytes.Salt;
                passwordHashed = rfc2898DeriveBytes.GetBytes(passwordHashedSize);
            }

            byte[] passwordHashedWithSalt = new byte[saltSize + passwordHashedSize + 1];
            Buffer.BlockCopy((Array)salt, 0, (Array)passwordHashedWithSalt, 1, saltSize);
            Buffer.BlockCopy((Array)passwordHashed, 0, (Array)passwordHashedWithSalt, ( saltSize + 1 ), passwordHashedSize);
            return Convert.ToBase64String(passwordHashedWithSalt);
        }

        public PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            const int saltSize = 16;
            const int iterations = 1000;
            const int passwordHashedSize = 32;
            byte[] providedPasswordHashed;

            if (hashedPassword == null)
            {
                return PasswordVerificationResult.Failed;
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != ( saltSize + passwordHashedSize + 1 ) ) || (src[0] != 0))
            {
                return PasswordVerificationResult.Failed;
            }
            byte[] salt = new byte[saltSize];
            Buffer.BlockCopy(src, 1, salt, 0, saltSize);
            byte[] passwordHashed = new byte[passwordHashedSize];
            Buffer.BlockCopy(src, ( saltSize + 1 ), passwordHashed, 0, passwordHashedSize);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(providedPassword, salt, iterations))
            {
                providedPasswordHashed = bytes.GetBytes(passwordHashedSize);
            }
            if (ByteArraysEqual(passwordHashed, providedPasswordHashed))
            {

                return PasswordVerificationResult.Success;
            }
            return PasswordVerificationResult.Failed;
        }

        private bool ByteArraysEqual(byte[] a1, byte[] a2)
        {
            return StructuralComparisons.StructuralEqualityComparer.Equals(a1, a2);
        }
    }
}