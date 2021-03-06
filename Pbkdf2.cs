﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace KudosNetCore
{
    // Copied and Adapted from https://cmatskas.com/-net-password-hashing-using-pbkdf2/
    public class Pbkdf2
    {
        public const int SaltByteSize = 32;
        public const int HashByteSize = 32;
        public const int Pbkdf2Iterations = 10_000;

        public static (byte[] salt, byte[] hash) HashPassword(string password, int iterations = Pbkdf2Iterations)
        {
            var cryptoProvider = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, iterations, HashByteSize);
            return (salt, hash);
        }

        public static bool ValidatePassword(string passwordCandidate, byte[] salt, byte[] hash, int iterations = Pbkdf2Iterations)
        {
            var testHash = GetPbkdf2Bytes(passwordCandidate, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++) {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputSize)
            => new Rfc2898DeriveBytes(password, salt, iterations).GetBytes(outputSize);
    }
}
