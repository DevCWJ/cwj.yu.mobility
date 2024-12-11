using System;
using System.Collections.Generic;
using System.Linq;

namespace CWJ
{
    public static class DelegateUtil
    {
        public static List<T> GetAllDelegateList<T>(this T @delegate) where T : Delegate
        {
            return GetAllDelegateEnumerable(@delegate).ToList();
        }

        public static T[] GetAllDelegateArray<T>(this T @delegate) where T : Delegate
        {
            return GetAllDelegateEnumerable(@delegate).ToArray();
        }

        public static IEnumerable<T> GetAllDelegateEnumerable<T>(this T @delegate) where T : Delegate
        {
            if (@delegate == null) return new T[0];
            return @delegate.GetInvocationList().OfType<T>();
        }

        public static T ManyConditions<T>(params Func<T>[] funcs) => ManyConditions(funcs, null);
        public static T ManyConditions<T>(Predicate<T> checkNotNull, params Func<T>[] funcs) => ManyConditions(funcs, checkNotNull);

        private static T ManyConditions<T>(Func<T>[] funcs, Predicate<T> checkNotNull)
        {
            if (checkNotNull == null)
                checkNotNull = (t) => t != null && !t.Equals(default(T));

            for (int i = 0; i < funcs.Length; i++)
            {
                var value = funcs[i].Invoke();
                if (checkNotNull(value))
                {
                    return value;
                }
            }
            return default(T);
        }
    }
}
