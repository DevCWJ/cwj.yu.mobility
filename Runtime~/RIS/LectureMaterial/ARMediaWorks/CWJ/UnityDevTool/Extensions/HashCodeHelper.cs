namespace CWJ
{
    /// <summary>
    /// Helper class for generating hash codes suitable 
    /// for use in hashing algorithms and data structures like a hash table. 
    /// </summary>
    public static class HashCodeHelper
    {
        private static int GetHashCodeInternal(int key1, int key2)
        {
            unchecked
            {
                var num = 0x7e53a269;
                num = (-1521134295 * num) + key1;
                num += (num << 10);
                num ^= (num >> 6);

                num = ((-1521134295 * num) + key2);
                num += (num << 10);
                num ^= (num >> 6);

                return num;
            }
        }

        /// <summary>
        /// Returns a hash code for the specified objects
        /// </summary>
        /// <param name="arr">An array of objects used for generating the 
        /// hash code.</param>
        /// <returns>
        /// A hash code, suitable for use in hashing algorithms and data 
        /// structures like a hash table. 
        /// </returns>
        public static int GetHashCode(params object[] arr)
        {
            int hash = 0;
            foreach (var item in arr)
                hash = GetHashCodeInternal(hash, item.GetHashCode());
            return hash;
        }

        /// <summary>
        /// Returns a hash code for the specified objects
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <param name="obj3">The third object.</param>
        /// <param name="obj4">The fourth object.</param>
        /// <returns>
        /// A hash code, suitable for use in hashing algorithms and
        /// data structures like a hash table.
        /// </returns>
        public static int GetHashCode<T1, T2, T3, T4>(T1 obj1, T2 obj2, T3 obj3,
            T4 obj4)
        {
            return GetHashCode(obj1, GetHashCode(obj2, obj3, obj4));
        }

        /// <summary>
        /// Returns a hash code for the specified objects
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <param name="obj3">The third object.</param>
        /// <returns>
        /// A hash code, suitable for use in hashing algorithms and data 
        /// structures like a hash table. 
        /// </returns>
        public static int GetHashCode<T1, T2, T3>(T1 obj1, T2 obj2, T3 obj3)
        {
            return GetHashCode(obj1, GetHashCode(obj2, obj3));
        }

        /// <summary>
        /// Returns a hash code for the specified objects
        /// </summary>
        /// <param name="obj1">The first object.</param>
        /// <param name="obj2">The second object.</param>
        /// <returns>
        /// A hash code, suitable for use in hashing algorithms and data 
        /// structures like a hash table. 
        /// </returns>
        public static int GetHashCode<T1, T2>(T1 obj1, T2 obj2)
        {
            return GetHashCodeInternal(obj1.GetHashCode(), obj2.GetHashCode());
        }
    } 
}