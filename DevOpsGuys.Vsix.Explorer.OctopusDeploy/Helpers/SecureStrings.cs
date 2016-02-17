// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SecureStrings.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Helper class for secure strings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Helpers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Security;

    /// <summary>
    /// Helper class for secure strings.
    /// </summary>
    internal static class SecureStrings
    {
        /// <summary>
        /// The entropy.
        /// </summary>
        private static readonly byte[] Entropy = System.Text.Encoding.Unicode.GetBytes("doglabs@devopsguys.com (contact us)");

        /// <summary>
        /// Converts to an unsecure string.
        /// </summary>
        /// <param name="secureString">The secure string.</param>
        /// <returns>
        /// A plain text string.
        /// </returns>
        public static string ToUnsecureString(this SecureString secureString)
        {
            Guard.ArgumentNotNull(secureString, "securePassword");

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        /// Encrypts the string.
        /// </summary>
        /// <param name="secureString">The secure string.</param>
        /// <returns>An encrypted string.</returns>
        public static string ToEncryptedString(this SecureString secureString)
        {
            var encryptedData =
                System.Security.Cryptography.ProtectedData.Protect(
                    System.Text.Encoding.Unicode.GetBytes(ToUnsecureString(secureString)),
                    Entropy,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// Decrypts the string to a secure string.
        /// </summary>
        /// <param name="encryptedData">The encrypted data.</param>
        /// <returns>A secure string.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed")]
        public static SecureString ToDecryptedSecureString(this string encryptedData)
        {
            try
            {
                var decryptedData = System.Security.Cryptography.ProtectedData.Unprotect(
                    Convert.FromBase64String(encryptedData),
                    Entropy,
                    System.Security.Cryptography.DataProtectionScope.CurrentUser);
                return ToSecureString(System.Text.Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        /// <summary>
        /// Plain text to a secure string.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>
        /// A secure string.
        /// </returns>
        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Reviewed")]
        private static SecureString ToSecureString(this string plainText)
        {
            Guard.ArgumentNotNull(plainText, "plainText");

            var secure = new SecureString();
            foreach (var c in plainText)
            {
                secure.AppendChar(c);
            }

            secure.MakeReadOnly();
            return secure;
        }
    }
}
