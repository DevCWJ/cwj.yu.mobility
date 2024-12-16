using System;
using System.Text;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using CWJ.AccessibleEditor;
#endif
using System.Linq;

namespace CWJ.BuildTimeStamp
{
    public static class BuildTimeStampMngr
    {
#if UNITY_EDITOR
        //Build 시작시간을 StreamingAssets/.txt에 기록하고싶으면 주석풀기
        [InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            BuildEventSystem.BeforeBuildEvent += BuildEventSystem_BeforeBuildEvent;
        }
#endif
        public static readonly string TimeStampTxtPath = PathUtil.Combine(UnityEngine.Application.streamingAssetsPath, "CWJ_B_TimeStamp.txt");
        public static readonly string DeadlineLogTxtPath = PathUtil.Combine(UnityEngine.Application.streamingAssetsPath, "CWJ_D_Log.txt");

        public static void CreateDeadlineLogTxt()
        {
            CreateTxtFileOverwrite(DeadlineLogTxtPath, DateTime.Now.ToString());
        }

        private static void CreateTxtFileOverwrite(string path, string str)
        {
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string tempFilePath = Path.GetTempFileName();
            using (FileStream writer = new FileStream(tempFilePath, FileMode.Create))
            {
                using (FileStream reader = new FileStream(path, FileMode.OpenOrCreate))
                {
                    byte[] stringBytes = Encoding.UTF8.GetBytes(str);
                    writer.Write(stringBytes, 0, stringBytes.Length);
                    reader.CopyTo(writer);
                }
            }
            File.Copy(tempFilePath, path, true);
            File.Delete(tempFilePath);
        }

        private static void BuildEventSystem_BeforeBuildEvent()
        {
            CreateTxtFileOverwrite(TimeStampTxtPath, GetUniqueBuildTime() + Environment.NewLine);

        }

        public static string GetUniqueBuildTime()
        {
            return System.Guid.NewGuid().ToString().GetExtractNumber().Substring(0, 6) + DateTime.Now.ToString("ssddmmMMHHyy");
        }

        public static bool GetBuildTimeStamp(out DateTime buildTime)
        {
            buildTime = DateTime.MinValue;
            if (!PathUtil.IsFileExists(TimeStampTxtPath, false, false))
            {
                return false;
            }

            string txtLine;
            using (StreamReader reader = new StreamReader(TimeStampTxtPath))
            {
                txtLine = reader.ReadLine();
            }
            if (txtLine.Length != 18)
            {
                return false;
            }
            try
            {
                txtLine = txtLine.Remove(0, 8);
                var day = txtLine.Substring(0, 2);
                var min = txtLine.Substring(2, 2);
                var month = txtLine.Substring(4, 2);
                var hour = txtLine.Substring(6, 2);
                var year = txtLine.Substring(8, 2);
                buildTime = Convert.ToDateTime($"{year}-{month}-{day} {hour}:{min}");
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static string GetLastBuildTime()
        {
            DateTime buildTime;
            if (!GetBuildTimeStamp(out buildTime))
            {
                return null;
            }
            return buildTime.ToString();
        }
        public static bool GetElapsdedDayFromBuildTime(out int date)
        {
            date = 0;
            DateTime buildTime;
            if(!GetBuildTimeStamp(out buildTime))
            {
                return false;
            }
            date = DateTime.Now.Subtract(buildTime).Days;
            return true;
        }
    }
}
