
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CWJ
{
    public static class ArrayUtil
    {
        public static int LengthSafe<T>(this T[] array)
        {
            if (array == null)
                return 0;
            return array.Length;
        }
        public static int LengthSafe(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;
            return str.Length;
        }
        public static void SplitLast(this string array, int index, out string leftArr, out string rightArr)
        {
            leftArr = new string(array.SkipLast(index).ToArray());
            rightArr = new string(array.TakeLast(index).ToArray());
        }
        public static void SplitLast<T>(this IEnumerable<T> array, int index, out T[] leftArr, out T[] rightArr)
        {
            leftArr = array.SkipLast(index).ToArray();
            rightArr = array.TakeLast(index).ToArray();
        }
        public static void SplitLast<T>(this IList<T> array, int index, out T[] leftArr, out T[] rightArr)
        {
            leftArr = array.SkipLast(index).ToArray();
            rightArr = array.TakeLast(index).ToArray();
        }
        public static void Split<T>(this IEnumerable<T> array, int index, out T[] leftArr, out T[] rightArr)
        {
            leftArr = array.Take(index).ToArray();
            rightArr = array.Skip(index).ToArray();
        }
        public static void Split<T>(this IList<T> array, int index, out T[] leftArr, out T[] rightArr)
        {
            leftArr = array.Take(index).ToArray();
            rightArr = array.Skip(index).ToArray();
        }



        public static int CountSafe<T>(this List<T> list)
        {
            if (list == null)
                return 0;
            return list.Count;
        }

        public static int CountSafe(this IList list)
        {
            if (list == null)
                return 0;
            return list.Count;
        }

        public static string ConvertStringJoin<T>(this List<T> list, Converter<T,string> converter, string separator = ", ")
        {
            return string.Join(separator, list.ConvertAll(converter));
        }

        public static string ConvertStringJoin<T>(this T[] array, Converter<T, string> converter, string separator = ", ")
        {
            return string.Join(separator, Array.ConvertAll(array, converter));
        }

        public static T[] ToArrayOnlyValues<T, TKey>(this Dictionary<TKey, T> dic)
        {
            if (dic == null || dic.Count == 0) return null;
            return dic.Values?.ToArray();
        }

        public static T[] ToArrayFromMulti<T>(this T[][] multidimensionalArray)
        {
            return multidimensionalArray.SelectMany(e => e).ToArray();
        }
        public static T[] ToArrayFromMulti<T>(this List<T>[] multidimensionalArray)
        {
            return multidimensionalArray.SelectMany(e => e).ToArray();
        }
        public static T[] ToArrayFromMulti<T> (this List<List<T>> multidimensionalList)
        {
            return multidimensionalList.SelectMany(e => e).ToArray();
        }
        public static T[] ToArrayFromMulti<T>(this List<T[]> multidimensionalList)
        {
            return multidimensionalList.SelectMany(e => e).ToArray();
        }

        public static T[] InitArray<T>(T initValue, int length)
        {
            return Enumerable.Repeat(initValue, length).ToArray();
        }
        public static List<T> InitList<T>(T initValue, int length)
        {
            return Enumerable.Repeat(initValue, length).ToList();
        }

        public static bool ArrayEquals<T>(this IList<T> lhs, IList<T> rhs, Func<T, T, bool> equalsPredicate)
        {
            if (lhs == null || rhs == null)
                return lhs == rhs;

            int length = lhs.Count;
            if (length != rhs.Count)
                return false;

            for (int i = 0; i < length; ++i)
            {
                var l = lhs[i]; var r = rhs[i];
                bool isLExists = l != null;
                bool isRExists = r != null;
                if (isLExists != isRExists)
                {
                    return false;
                }

                if (isLExists && !l.Equals(r))
                {
                    return false;
                }
                if (isLExists && isRExists && !equalsPredicate(l, r))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ArrayEquals<T>(this IList<T> lhs, IList<T> rhs)
        {
            if (lhs == null || rhs == null)
                return lhs == rhs;

            int length = lhs.Count;
            if (length != rhs.Count)
                return false;

            for (int i = 0; i < length; ++i)
            {
                bool isLExists = lhs[i] != null;
                bool isRExists = rhs[i] != null;
                if (isLExists != isRExists)
                {
                    return false;
                }

                if (isLExists && !lhs[i].Equals(rhs[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ArrayEqualsByDifferentType(IList lhs, IList rhs)
        {
            if (lhs == null || rhs == null)
                return lhs == rhs;

            int length = lhs.Count;
            if (length != rhs.Count)
                return false;

            for (int i = 0; i < length; ++i)
            {
                bool isLExists = lhs[i] != null;
                bool isRExists = rhs[i] != null;
                if (isLExists != isRExists)
                {
                    return false;
                }

                if (isLExists && !lhs[i].Equals(rhs[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool ObjectPowerfulEquals<T>(T a, T b)
        {
            bool isANull = a == null; bool isBNull = b == null;
            if (isANull != isBNull)
            {
                return false;
            }
            if(isANull && isBNull)
            {
                return true;
            }
            if (!a.Equals(b))
            {
                return false;
            }

            Type type = a.GetType();
            if (type != null && type.IsClassOrStructOrTuple())
            {
                foreach (var prop in type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    if (!ObjectPowerfulEquals(prop.GetValue(a), prop.GetValue(b)))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool ArrayReferenceEquals<T>(this IList<T> lhs, IList<T> rhs)
        {
            if (lhs == null || rhs == null)
                return lhs == rhs;

            int length = lhs.Count;
            if (length != rhs.Count)
                return false;

            for (int i = 0; i < length; ++i)
            {
                bool isLExists = lhs[i] != null;
                bool isRExists = rhs[i] != null;
                if (isLExists != isRExists)
                {
                    return false;
                }

                if (isLExists && !object.ReferenceEquals(lhs[i], rhs[i]))
                {
                    return false;
                }
            }
            return true;
        }
        public static bool IsAllNull<T>(this IList<T> list)
        {
            if (list != null)
            {
                int cnt = list.Count;
                for (int i = 0; i < cnt; i++)
                {
                    if (list[i] != null
#if UNITY_EDITOR
                        && !list[i].Equals(null)
#endif
                        )
                    {
                        return false;
                    }
                }
            }
            return true;
        }



        /// <summary>
        /// string 의 subString과 같은기능
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static T[] SubArray<T>(this IList<T> source, int startIndex, int length)
        {
            T[] result = new T[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = source[i + startIndex];
            }

            return result;
        }

        /// <summary>
        /// 배열에서 lenght개의 elements를 랜덤하게 뽑아냄
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="length"></param>
        /// <param name="isUnique"></param>
        /// <returns></returns>
        public static T[] GetShuffledArray<T>(this IList<T> source, int startIndex, int length, bool isUnique = false)
        {
            return SubArray(source.GetShuffled(isUnique: isUnique), startIndex, length);
        }

        /// <summary>
        /// 배열을 랜덤한 순서로 섞음
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="isUnique"></param>
        /// <returns></returns>
        public static IList<T> GetShuffled<T>(this IList<T> source, bool isUnique = false)
        {
            System.Random pseudoRnd = (isUnique ? new System.Random(Guid.NewGuid().GetHashCode()) : new System.Random());

            int count = source.Count;
            for (int i = 0; i < count - 1; i++)
            {
                int randomIndex = pseudoRnd.Next(i, count);
                T tmp = source[randomIndex];
                source[randomIndex] = source[i];
                source[i] = tmp;
            }
            
            return source;
        }

        public static bool IsExists<T>(this IList<T> array, T item)
        {
            return array.IndexOf(item) >= 0;
        }

        public static bool IsExists<T>(this IList<T> array, Predicate<T> predicate)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (predicate(array[i]))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// List처럼 Add기능. 이런 기능을 자주 써야한다면 List가 나음
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        public static void Add<T>(ref T[] array, T value)
        {
#if UNITY_WEBGL
            var list = array.ToList();
            list.Add(value);
            array = list.ToArray();
#else

            int arrayLength = 0;

            if (array == null || array.Length == 0)
            {
                array = new T[1];
            }
            else
            {
                arrayLength = array.Length;
                Array.Resize(ref array, arrayLength + 1);
            }

            array[arrayLength] = value;
#endif
        }

        /// <summary>
        /// List처럼 AddRange기능. 이런 기능을 자주 써야한다면 동적배열이 나음
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="values"></param>
        public static void AddRange<T>(ref T[] array, params T[] values)
        {
            if (values == null || values.Length == 0) return;

#if UNITY_WEBGL
            var list = array.ToList();
            list.AddRange(values);
            array = list.ToArray();
#else
            int arrayLength = 0;

            if (array == null || array.Length == 0)
            {
                array = new T[values.Length];
            }
            else
            {
                arrayLength = array.Length;
                Array.Resize(ref array, arrayLength + values.Length);
            }

            for (int i = 0; i < values.Length; ++i)
            {
                array[arrayLength + i] = values[i];
            }
#endif
        }
        public static bool AddRange<T>(this HashSet<T> source, IEnumerable<T> items)
        {
            bool allAdded = true;
            foreach (T item in items)
            {
                allAdded &= source.Add(item);
            }
            return allAdded;
        }

        public static bool RemoveRange<T>(this HashSet<T> source, IEnumerable<T> items)
        {
            bool allRemoved = true;
            foreach (T item in items)
            {
                allRemoved &= source.Remove(item);
            }
            return allRemoved;
        }
        public static void Insert<T>(ref T[] array, int index, T item)
        {
            var a = new System.Collections.ArrayList();
            a.AddRange(array);
            a.Insert(index, item);
            array = a.ToArray(typeof(T)) as T[];
        }

        /// <summary>
        /// List처럼 Remove기능. 자주쓰는 배열이라면 List가 나음
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="removeValue"></param>
        public static void Remove<T>(ref T[] array, T removeValue)
        {
            if (array == null) return;
            if (array.Length == 0)
            {
                return;
            }
            RemoveRange(ref array, Array.FindIndex(array, (item) => item.Equals(removeValue)), 1);
        }

        /// <summary>
        /// List처럼 RemoveAt기능. 자주쓰는 배열이라면 List가 나음
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="value"></param>
        public static void RemoveAt<T>(ref T[] array, int removeIndex)
        {
            RemoveRange(ref array, removeIndex, 1);
        }

        public static void RemoveRange<T>(ref T[] array, int index, int count)
        {
            if (array == null) return;

            if (array.Length == 0 || index < 0 || (index + count) > array.Length)
            {
                return;
            }
#if UNITY_WEBGL
            var list = array.ToList();
            list.RemoveRange(index, count);
            array = list.ToArray();
#else
            for (int i = index + count; i < array.Length; ++i)
            {
                array[i - count] = array[i];
            }

            Array.Resize(ref array, array.Length - count);
#endif
        }

        public static void Clear<T>(ref T[] array)
        {
            Array.Clear(array, 0, array.Length);
            array = new T[0];
        }

        public static T Find<T>(this T[] array, Predicate<T> match)
        {
            int index = Array.FindIndex(array, match);
            
            return index >= 0 ? array[index] : default(T);
        }

        public static T Find<T>(this IEnumerable<T> array, Predicate<T> match)
        {
            foreach (var item in array)
            {
                if (match(item))
                {
                    return item;
                }
            }
            return default(T);
        }

        public static bool IsExists<T>(this IEnumerable<T> array, Predicate<T> match, out T result)
        {
            result = default(T);
            foreach (var item in array)
            {
                if (match(item))
                {
                    result = item;
                    return true;
                }
            }
            return false;
        }


        public static int IndexOf<T>(this IEnumerable<T> array, Predicate<T> match)
        {
            int index = 0;
            foreach (var item in array)
            {
                if (match(item))
                {
                    break;
                }
                index++;
            }

            return index;
        }

        public static int FindIndex<T>(this T[] array, Predicate<T> match)
        {
            return Array.FindIndex(array, match);
        }

        public static int[] FindAllIndex<T>(this IList<T> container, Predicate<T> match)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < container.Count; i++)
            {
                if (match(container[i]))
                {
                    indexes.Add(i);
                }
            }

            return indexes.ToArray();
        }

        public static T[] FindAll<T>(this IList<T> array, Predicate<T> match)
        {
            return (from item in array
                    where match(item)
                    select item).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="firstMatch"></param>
        /// <param name="secondMatch"></param>
        /// <param name="index"></param>
        /// <param name="isFirstMatched"></param>
        /// <returns>any matched <see langword="true"/></returns>
        public static bool FindAOrBIndex<T>(this IList<T> array, Predicate<T> commonMatch, Predicate<T> firstMatch, Predicate<T> secondMatch, out int index, out bool isFirstMatched)
        {
            index = -1;
            isFirstMatched = false;

            int length = array.Count;
            if (length == 0)
            {
                return false;
            }

            for (int i = 0; i < length; i++)
            {
                var elm = array[i];
                if (!commonMatch(elm))
                {
                    continue;
                }

                if (firstMatch(elm))
                {
                    index = i;
                    isFirstMatched = true;
                    return true;
                }
                else if (secondMatch(elm))
                {
                    index = i;
                    return true;
                }
            }
            return false;
        }

        public static T[] FindAllWithMisMatch<T>(this IEnumerable<T> array, Predicate<T> match, out T[] misMatches, int startIndex = 0)
        {
            int cnt = array.Count();
            var matchList = new List<T>(capacity: cnt);
            var misMatchList = new List<T>(capacity: cnt);

            foreach (var item in array)
            {
                if (match(item)) matchList.Add(item);
                else misMatchList.Add(item);
            }

            misMatches = misMatchList.ToArray();

            return matchList.ToArray();
        }

        public static T[] FindAllWithMisMatch<T>(this IList<T> array, Predicate<T> match, out T[] misMatches, int startIndex = 0)
        {
            var matchList = new List<T>();
            var misMatchList = new List<T>();

            int length = array.Count;
            for (int i = startIndex; i < length; ++i)
            {
                if (match(array[i])) matchList.Add(array[i]);
                else misMatchList.Add(array[i]);
            }

            misMatches = misMatchList.ToArray();

            return matchList.ToArray();
        }
        public static T[] FindAll<T>(this IList<T> array, Predicate<T> match, Predicate<T> breakMatch = null, int startIndex = 0, bool isMatchIsCorrectOrBreak = false)
        {
            var matchList = new List<T>();

            if (isMatchIsCorrectOrBreak)
            {
                for (int i = startIndex; i < array.Count; ++i)
                {
                    if (match(array[i]))
                        matchList.Add(array[i]);
                    else
                        break;

                }
            }
            else
            {
                for (int i = startIndex; i < array.Count; ++i)
                {
                    T item = array[i];

                    if (breakMatch != null && breakMatch.Invoke(item))
                        break;

                    if (match(item))
                        matchList.Add(item);
                }
            }



            return matchList.ToArray();
        }

        public static int Count<T>(this T[] array, Predicate<T> match, int startIndex, int endIndex)
        {
            int cnt = 0;

            for (int i = startIndex; i < endIndex; ++i)
            {
                if (match(array[i])) cnt++;
            }

            return cnt;
        }

        public static int IndexOf<T>(this T[] array, T value)
        {
            return Array.IndexOf(array, value);
        }

        public static int IndexOf<T>(this IList<T> list, Predicate<T> match)
        {
            int cnt = list.Count;
            for (int i = 0; i < cnt; i++)
            {
                if (match.Invoke(list[i]))
                    return i;
            }
            return -1;
        }

        public static int IndexOf<T>(this T[] array, Predicate<T> match, int startIndex, int endIndex, Predicate<T> breakMatch = null)
        {
            for (int i = startIndex; i < endIndex; ++i)
            {
                if (breakMatch != null && breakMatch.Invoke(array[i]))
                    break;

                if (match.Invoke(array[i]))
                    return i;
            }
            return -1; //couldnt find
        }

        public static int LastIndexOf<T>(this T[] array, T value)
        {
            return Array.LastIndexOf(array, value);
        }

        public static int LastIndexOf<T>(this T[] array, Predicate<T> match, int startIndex, int endIndex, Predicate<T> breakMatch=null)
        {
            for (int i = startIndex; i > endIndex; --i)
            {
                if (breakMatch != null && breakMatch.Invoke(array[i]))
                    break;

                if (match.Invoke(array[i]))
                    return i;
            }
            return -1; //couldnt find
        }

        public static T[] Merge<T>(IList<T> lhs, IList<T> rhs)
        {
            T[] all = new T[lhs.Count + rhs.Count];

            Array.Copy(lhs.ToArray(), 0, all, 0, lhs.Count);
            Array.Copy(rhs.ToArray(), 0, all, lhs.Count, rhs.Count);

            return all;
        }


        public static T[] ForEach<T>(this T[] array, Action<T> action, int startIndex = 0)
        {
            if (array == null || action == null) return array;

            int arrCnt = array.Length;
            for (int i = startIndex; i < arrCnt; i++)
            {
                action.Invoke(array[i]);
            }
            return array;
        }
        /// <summary>
        /// 구조체에는 안쓰도록 조심하기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            if (array == null || action == null) return array;
            foreach (var item in array)
            {
                action.Invoke(item);
            }
            return array;
        }
        /// <summary>
        /// 구조체에는 안쓰도록 조심하기
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="action"></param>
        public static void Do<T>(this IList<T> array, Action<T, int> action, int startIndex = 0)
        {
            if (array == null || action == null) return;
            int arrCnt = array.Count;
            for (int i = startIndex; i < arrCnt; i++)
            {
                action.Invoke(array[i], i);
            }
        }

        public static void Do<T>(this List<T> array, Action<T, int> action, int startIndex = 0)
        {
            if (array == null || action == null) return;
            int arrCnt = array.Count;
            for (int i = startIndex; i < arrCnt; i++)
            {
                action.Invoke(array[i], i);
            }
        }

        public static void Do<T>(this IEnumerable<T> array, Action<T> action, int startIndex = 0)
        {
            if (array == null || action == null) return;
            int arrCnt = array.Count();
            foreach (var item in array)
            {
                action.Invoke(item);
            }
        }
        public static void Copy<T>(out T[] destination, T[] source)
        {
            destination = new T[source.Length];
            Array.Copy(source, destination, destination.Length);
        }

        public static void AddRangeNotOverlap<T>(this List<T> list, params T[] adds)
        {
            list.AddRangeNotOverlap(addList: adds);
        }

        public static void AddRangeNotOverlap<T>(this List<T> list, IList<T> addList)
        {
            list.AddRange(addList.FindAll((e) => !list.Contains(e)));
        }

        public static void RemoveRange<T>(this List<T> list, params T[] removes)
        {
            list.RemoveRange(removeList: removes);
        }

        public static void RemoveRange<T>(this List<T> list, IList<T> removeList)
        {
            int listCnt = removeList.Count;
            for (int i = 0; i < listCnt; i++)
            {
                int index = list.IndexOf(removeList[i]);
                if (index >= 0)
                {
                    list.RemoveAt(index);
                }
            }
        }

        public static T Max<T>(this IList<T> array, Func<T, int> selector, Action<T> callbackForSmall = null)
        {
            int length = array?.Count ?? 0;

            if (length == 0) return default;

            T maxElem = array[0];
            int maxValue = selector(maxElem);

            for (int i = 1; i < length; ++i)
            {
                int value = selector(array[i]);
                if (value > maxValue)
                {
                    callbackForSmall?.Invoke(maxElem);
                    maxValue = value;
                    maxElem = array[i];
                }
                else
                {
                    callbackForSmall?.Invoke(array[i]);
                }
            }
            return maxElem;
        }

        public static T Min<T>(this IList<T> array, Func<T, int> selector, Action<T> callbackForBig = null)
        {
            int length = array?.Count ?? 0;

            if (length == 0) return default;

            T minElem = array[0];
            int minValue = selector(minElem);

            for (int i = 1; i < length; ++i)
            {
                int value = selector(array[i]);
                if (value < minValue)
                {
                    callbackForBig?.Invoke(minElem);
                    minValue = value;
                    minElem = array[i];
                }
                else
                {
                    callbackForBig?.Invoke(array[i]);
                }
            }
            return minElem;
        }


        public static int MaxIndex<T>(this IList<T> array, Func<T, int> selector)
        {
            int length = array?.Count ?? 0;

            if (length == 0) return -1;

            int maxValue = selector(array[0]);
            int maxIndex = 0;

            for (int i = 1; i < length; ++i)
            {
                int value = selector(array[i]);
                if (value > maxValue)
                {
                    maxValue = value;
                    maxIndex = i;
                }
            }
            return maxIndex;
        }

        public static int MinIndex<T>(this IList<T> array, Func<T, int> selector)
        {
            int length = array?.Count ?? 0;

            if (length == 0) return -1;

            int minValue = selector(array[0]);
            int minIndex = 0;

            for (int i = 1; i < length; ++i)
            {
                int value = selector(array[i]);
                if (value < minValue)
                {
                    minValue = value;
                    minIndex = i;
                }
            }
            return minIndex;
        }

        public static TOutput[] ConvertAll<T,TOutput>(this T[] array, Converter<T, TOutput> converter)
        {
            return Array.ConvertAll(array, converter);
        }

        public static IEnumerable<TOutput> ConvertAll<T, TOutput>(this IEnumerable<T> array, Func<T, TOutput> converter)
        {
            return array.Select(converter);
        }

        public static IList<T> ModifyEach<T>(this IList<T> sources, Func<T, T> modifyFunc)
        {
            int length = sources.Count;
            for (int i = 0; i < length; i++)
            {
                sources[i] = modifyFunc(sources[i]);
            }
            return sources;
        }

        /// <summary>
        /// 순서 상관없이 <seealso langword="targetList"/> 안에 <seealso langword="elements"/> 요소들이 존재하는지 체크하는 함수
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetList"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
        public static bool ContainsList<T> (this IList<T> targetList, IList<T> elements)
        {
            if (targetList.Count < elements.Count)
            {
                return false;
            }

            //int length = elements.Count;
            //for (int i = 0; i < length; i++)
            //{
            //    if (!targetList.Contains(elements[i]))
            //    {
            //        return false;
            //    }
            //}

            return !elements.Any(e => !targetList.Contains(e));
        }
    }
}