using System;
using System.Collections;
using System.IO;

using UnityEngine;

#if UNITY_2019_2_OR_NEWER
using UnityEngine.Networking;
#endif

namespace CWJ
{
    public static class NetworkUtil
    {
        public static string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }

        public static string GetIp()
        {
            string ipValue = "127.0.0.1";

            System.Net.IPAddress[] ipaddress = System.Net.Dns.GetHostEntry(GetHostName()).AddressList;
            foreach (System.Net.IPAddress ip in ipaddress)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipValue = ip.ToString();
                }
            }

            return ipValue;
        }

        public static IEnumerator CO_CopyStreamingAssetsFileToPersistent(string filePath)
        {
            string sourcePath = PathUtil.Combine(Application.streamingAssetsPath, filePath);
            string destPath = PathUtil.Combine(Application.persistentDataPath, filePath);

            string msg = $"{filePath} 생성완료\n경로 : {destPath}";

            if (sourcePath.Contains("://"))
            {
                yield return CO_LoadFileViaWWW(sourcePath, (data) =>
                {
                    Create(data, destPath);
                    Debug.Log(msg);
                });
            }
            else
            {
                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, destPath, true);
                    Debug.Log(msg);
                }
            }
        }

        public static IEnumerator CO_LoadStreamingAssetsFile(string filePath, Action<byte[]> onLoad, Action onError = null)
        {
            if (onLoad == null)
            {
                yield break;
            }

            string path = PathUtil.Combine(Application.streamingAssetsPath, filePath);
            byte[] dataBytes = null;

            if (path.Contains("://")) //maybe AOS
            {
                yield return CO_LoadFileViaWWW(path, onLoad, onError);
            }
            else
            {
                try
                {
                    dataBytes = File.ReadAllBytes(path);
                    onLoad(dataBytes);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                    onError?.Invoke();
                }
            }
        }

        public static IEnumerator CO_LoadFileViaWWW(string path, Action<byte[]> onLoad, Action onError = null)
        {
            if (onLoad == null)
            {
                yield break;
            }

            byte[] data = null;

#if UNITY_2019_2_OR_NEWER
            using (var www = UnityWebRequest.Get(PathToUrl(path)))
            {
                yield return www.SendWebRequest();

                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError(www.error);
                    onError?.Invoke();
                    yield break;
                }
                data = www.downloadHandler.data;
            }
#else
#pragma warning disable 0618
            using (var www = new WWW(PathToUrl(path)))
            {
                while (!www.isDone)
                {
                    yield return null;
                }

                if (!string.IsNullOrEmpty(www.error))
                {
                    Debug.LogError(www.error);
                    onError?.Invoke();
                    yield break;
                }
                data = www.bytes;
            }
#pragma warning restore 0618
#endif
            onLoad(data);
            yield break;
        }

        public static string PathToUrl(string path)
        {
            if (string.IsNullOrEmpty(path) || path.StartsWith("jar:file://") || path.StartsWith("file://") || path.StartsWith("http://") || path.StartsWith("https://"))
            {
                return path;
            }
            if (Application.platform == RuntimePlatform.OSXEditor ||
                Application.platform == RuntimePlatform.OSXPlayer ||
                Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.Android)
            {
                path = "file://" + path;
            }
            else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                path = "file:///" + path;
            }
            return path;
        }

        public static bool Delete(string path)
        {
            if (File.Exists(path))
            {
                new FileStream(path, FileMode.Truncate).Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void Create(byte[] data, string destPath)
        {
            File.Create(destPath).Dispose();

            File.WriteAllBytes(destPath, data);
        }

        //public static void Create(string path, string[] data)
        //{
        //    File.Create(path).Dispose();

        //    File.WriteAllLines(path, data);
        //}

        public static byte[] Load(string filename)
        {
            string path = Application.persistentDataPath + Path.DirectorySeparatorChar + filename;

            if (File.Exists(path))
            {
                return File.ReadAllBytes(path);
            }
            else
            {
                return null;
            }
        }

        public static void Save(string filename, byte[] data)
        {
            string path = Application.persistentDataPath + Path.DirectorySeparatorChar + filename;

            File.WriteAllBytes(path, data);
        }

        public static string GetFileName(string path)
        {
            int lastIndex = path.LastIndexOf(Path.AltDirectorySeparatorChar) + 1;

            if (lastIndex == 0)
            {
                lastIndex = path.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            }

            return path.Substring(lastIndex, path.Length - lastIndex);
        }
    }
}