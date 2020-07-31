using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MB.Utilities.Exceptions;
using System;
using System.Security.Cryptography;

namespace MB.Utilities
{
    /// <summary>
    /// Contains functionality for generating and verifying hashes.
    /// </summary>
    public static class HashUtil
    {
        private const int SALT_BIT_LENGTH = 128;
        private const int HASH_BIT_LENGTH = 256;
        private const int KEY_GENERATION_ITERATION_COUNT = 10000;

        /// <summary>
        /// Generates a hash from the provided key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The generated has as a base64 string.</returns>
        /// <exception cref="ArgumentNullException">When the provided key parameter is null or empty.</exception>
        public static string Generate(string key)
        {
            // check provided key
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "The provided key parameter is empty.");
            }

            // generate a random salt
            byte[] salt = new byte[SALT_BIT_LENGTH / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // calculate the hase
            byte[] hash = KeyDerivation.Pbkdf2(key, salt, KeyDerivationPrf.HMACSHA1, KEY_GENERATION_ITERATION_COUNT, HASH_BIT_LENGTH / 8);

            // combine the salt + hash and return as a base64 string
            byte[] saltPlusHash = new byte[salt.Length + hash.Length];
            salt.CopyTo(saltPlusHash, 0);
            hash.CopyTo(saltPlusHash, salt.Length);
            return Convert.ToBase64String(saltPlusHash);
        }

        /// <summary>
        /// Verify the key from the provided hash.
        /// </summary>
        /// <param name="key">The key to verify.</param>
        /// <param name="hashedKey">The hashed key.</param>
        /// <returns>True when the key is verified with the provided hash. Otherwise false.</returns>
        /// <exception cref="ArgumentNullException">When the provided key and/or hashedKey parameter is null or empty.</exception>
        /// <exception cref="UnexpectedHashLengthException">When the provided hashedKey parameter does not have the expected length.</exception>
        public static bool Verify(string key, string hashedKey)
        {
            // check provided key
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "The provided key parameter is empty.");
            }

            // check provided hash
            if (string.IsNullOrEmpty(hashedKey))
            {
                throw new ArgumentNullException("hashedKey", "The provided hashedKey parameter is empty.");
            }

            // check provided hash length
            int expectedHashLength = (SALT_BIT_LENGTH + HASH_BIT_LENGTH) / 8;
            byte[] saltPlusHash;
            try
            {
                saltPlusHash = Convert.FromBase64String(hashedKey);
                if (saltPlusHash == null || saltPlusHash.Length != expectedHashLength)
                {
                    throw new UnexpectedHashLengthException("The provided hash doesn't have the expected length.");
                }
            }
            catch (FormatException)
            {
                throw new UnexpectedHashLengthException("The provided hash doesn't have the expected length.");
            }

            // get the salt and the hash from the hashed key
            byte[] salt = new byte[SALT_BIT_LENGTH / 8];
            byte[] hash = new byte[HASH_BIT_LENGTH / 8];
            Buffer.BlockCopy(saltPlusHash, 0, salt, 0, salt.Length);
            Buffer.BlockCopy(saltPlusHash, salt.Length, hash, 0, hash.Length);

            // calculate the hash for the provided key
            byte[] hashFromKey = KeyDerivation.Pbkdf2(key, salt, KeyDerivationPrf.HMACSHA1, KEY_GENERATION_ITERATION_COUNT, HASH_BIT_LENGTH / 8);

            // compare both hashes
            return Equals(hash, hashFromKey);
        }

        /// <summary>
        /// Compares 2 byte arrays on equality.
        /// </summary>
        /// <param name="array1">First array</param>
        /// <param name="array2">Second array</param>
        /// <returns>true when both array's are equal, ohterwise false.</returns>
        private static bool Equals(byte[] array1, byte[] array2)
        {
            if (array1 == array2)
            {
                return true;
            }

            if (array1 == null || array2 == null)
            {
                return false;
            }

            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int index = 0; index < array1.Length; index++)
            {
                if (array1[index] != array2[index])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
