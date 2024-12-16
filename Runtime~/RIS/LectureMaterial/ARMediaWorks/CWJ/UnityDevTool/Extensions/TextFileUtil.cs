using System;
using System.IO;
using System.Linq;
using System.Text;

using UnityEngine;

namespace CWJ
{
    public static class TextFileUtil
    {
        /// <summary>
        /// 사용가능한것인지 | (읽을권한이 있는지, 사용중이진 않은지)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsAccessAbleFile(this string path)
        {
            FileStream fs = null;
            try
            {
                fs = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"{path}\n해당 파일은 사용중입니다\n{e.ToString()}");
                return false;
            }
            finally
            {
                fs?.Close();
                fs = null;
            }

            return true;
        }

        /// <summary>
        /// txt 파일읽어서 라인별로 배열나눔
        /// </summary>
        /// <param name="path">경로</param>
        /// <param name="initContents">경로에 파일이 없을 시 text파일 내부 default 내용</param>
        /// <param name="removeSpaces">불러올때부터 공백없앨것인지</param>
        /// <param name="printLog"></param>
        /// <returns></returns>
        public static string[] GetTxtLines(this string path, string initContents = "", bool isForciblyInit = false, bool removeSpaces = false, bool printLog = false)
        {
            if (path.IsFileExists(true) && isForciblyInit)
            {
                File.Delete(path);
                Debug.LogWarning(path + "\n해당 파일을 초기화 하기위해 삭제함");
            }

            if (!File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.WriteLine(initContents);
                        sw.Close();
                    }
                }

                Debug.LogWarning(path + "\n해당 파일이 존재하지않아서 생성/초기화함\n" + initContents);
            }

            //접근가능여부 체크
            //if (!path.IsAccessAbleFile())
            //{
            //    return null;
            //}

            string[] txtLines = null;
            StreamReader sr = null;
            try
            {
                using (sr = new StreamReader(path, Encoding.UTF8))
                {
                    txtLines = sr.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (removeSpaces)
                    {
                        txtLines.RemoveAllSpaces();
                    }

                    sr.Close();
                    if (printLog)
                    {
                        Debug.LogWarning(path + "\nis Loaded");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }
            finally
            {
                sr?.Close();
                sr = null;
            }

            return txtLines;
        }

        public static string[] GetValuesByTxtLine(this string[] txtLines, bool removeSpaces = false, params string[] keys)
        {
            int keyLength = keys.Length;
            string[] returnValues = ArrayUtil.InitArray<string>(null, keyLength);

            int lineLength = txtLines?.Length ?? 0;
            if (lineLength == 0)
            {
                return returnValues;
            }

            string[] tmpArray = new string[lineLength];
            ArrayUtil.Copy(out tmpArray, txtLines);

            if (removeSpaces)
            {
                tmpArray.RemoveAllSpaces();
            }

            for (int i = 0; i < lineLength; i++)
            {
                string[] spl = tmpArray[i].Split(':');
                if (spl.Length < 2)
                {
                    continue;
                }

                for (int j = 0; j < keyLength; j++)
                {
                    if (keys[j] == "")
                    {
                        continue;
                    }

                    if (keys[j].Equals(spl[0]))
                    {
                        returnValues[j] = (spl[1] == null) ? "" : spl[1];

                        keys[j] = "";
                        break;
                    }
                }
            }

            return returnValues;
        }

        /// <summary>
        /// <para>배열중에 key값에 해당하는 라인을 가져와서 콜론(:)뒤의 내용으로 값을 반환</para>
        /// <para>(공백제거는 원본을 건드리진않음)</para>
        /// null인것은 key가 존재하지않았다는뜻
        /// </summary>
        /// <param name="txtLines"></param>
        /// <param name="key"></param>
        /// <param name="removeSpaces"></param>
        ///// <returns></returns>
        //public static string GetValueByTxtLine(this string[] txtLines, string key, bool removeSpaces = false)
        //{
        //    string returnValue = null;

        //    int lineLength = txtLines?.Length ?? 0;
        //    if (lineLength == 0)
        //    {
        //        return returnValue;
        //    }

        //    string[] tmpArray = new string[lineLength];
        //    ArrayUtil.ShallowCopy(out tmpArray, txtLines);

        //    if (removeSpaces)
        //    {
        //        tmpArray.RemoveAllSpaces();
        //    }

        //    for (int i = 0; i < tmpArray.Length; i++)
        //    {
        //        string[] spl = tmpArray[i].Split(':');
        //        if (spl.Length < 2)
        //        {
        //            continue;
        //        }

        //        if (key.Equals(spl[0]))
        //        {
        //            returnValue = string.IsNullOrEmpty(spl[1]) ? "" : spl[1];
        //            break;
        //        }
        //    }
        //    return returnValue;
        //}
    }
}