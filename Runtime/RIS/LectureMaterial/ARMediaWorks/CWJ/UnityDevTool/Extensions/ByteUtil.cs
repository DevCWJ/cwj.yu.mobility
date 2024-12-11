using System;

namespace CWJ
{
    public static class ByteUtil
    {
        public static bool SplitInBytes(this byte[] src, byte[] foundBytes, out byte[] splitRightBytes)
        {
            splitRightBytes = null;

            if (src == null || foundBytes == null || src.Length == 0 || foundBytes.Length == 0 || foundBytes.Length > src.Length)
            {
                return false;
            }

            int srcLength = src.Length;
            int foundBLength = foundBytes.Length;

            int index = IndexOfInBytes(src, foundBytes, srcLength, foundBLength);

            if (index == -1)
            {
                return false;
            }

            splitRightBytes = new byte[srcLength - index - foundBLength];

            Array.Copy(src, index + foundBLength, splitRightBytes, 0, splitRightBytes.Length);

            return true;
        }

        public static bool SplitInBytes(this byte[] src, byte[] foundBytes, out byte[] leftBytes, out byte[] rightBytes)
        {
            leftBytes = null;
            rightBytes = null;

            if (src == null || foundBytes == null || src.Length == 0 || foundBytes.Length == 0 || foundBytes.Length > src.Length)
            {
                return false;
            }

            int srcLength = src.Length;
            int foundBLength = foundBytes.Length;

            int index = IndexOfInBytes(src, foundBytes, srcLength, foundBLength);

            if (index == -1)
            {
                leftBytes = src;
                return false;
            }

            leftBytes = new byte[index];
            rightBytes = new byte[srcLength - index - foundBLength];

            Array.Copy(src, 0, leftBytes, 0, index);
            Array.Copy(src, index + foundBLength, rightBytes, 0, rightBytes.Length);
            return true;
        }

        public static byte[] RemoveFromEnd(this byte[] src, byte[] removeBytes, out int resultLength)
        {
            resultLength = -1;
            if (src == null || removeBytes == null || src.Length == 0 || removeBytes.Length == 0 || removeBytes.Length > src.Length)
            {
                return src;
            }

            int srcLength = src.Length;
            int removeBLength = removeBytes.Length;

            int index = LastIndexOfInBytes(src, removeBytes, srcLength, removeBLength);

            if (index == -1)
            {
                return src; 
            }

            resultLength = index;
            byte[] result = new byte[resultLength];
            Array.Copy(src, 0, result, 0, resultLength);

            return result;
        }

        public static int LastIndexOfInBytes(this byte[] src, byte[] foundBytes, int srcLength, int foundBLength)
        {
            // src 배열에서 foundBytes 배열을 뒤에서부터 찾는 메서드
            for (int i = srcLength - foundBLength; i >= 0; i--)
            {
                bool found = true;
                for (int j = 0; j < foundBLength; j++)
                {
                    if (src[i + j] != foundBytes[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return i;
                }
            }

            return -1; // 못 찾은 경우
        }

        public static int IndexOfInBytes(this byte[] src, byte[] foundBytes, int srcLength = -1, int foundBLength = -1)
        {
            if (srcLength == -1)
                srcLength = src.Length;
            if (foundBLength == -1)
                foundBLength = foundBytes.Length;

            int limit = srcLength - foundBLength;

            for (int i = 0; i <= limit; i++)
            {
                bool found = true;
                for (int j = 0; j < foundBLength; j++)
                {
                    if (src[i + j] != foundBytes[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return i;
                }
            }

            return -1; 
        }

        public static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] replace)
        {
            if (replace == null) return src;
            int index = FindBytes(src, search);
            if (index < 0) return src;
            byte[] dst = new byte[src.Length - search.Length + replace.Length];
            Buffer.BlockCopy(src, 0, dst, 0, index);
            Buffer.BlockCopy(replace, 0, dst, index, replace.Length);
            Buffer.BlockCopy(src, index + search.Length, dst, index + replace.Length, src.Length - (index + search.Length));
            return dst;
        }

        public static int FindBytes(byte[] src, byte[] find)
        {
            if (src == null || find == null || src.Length == 0 || find.Length == 0 || find.Length > src.Length) return -1;
            for (int i = 0; i < src.Length - find.Length + 1; i++)
            {
                if (src[i] == find[0])
                {
                    for (int m = 1; m < find.Length; m++)
                    {
                        if (src[i + m] != find[m]) break;
                        if (m == find.Length - 1) return i;
                    }
                }
            }
            return -1;
        }
    }
}
