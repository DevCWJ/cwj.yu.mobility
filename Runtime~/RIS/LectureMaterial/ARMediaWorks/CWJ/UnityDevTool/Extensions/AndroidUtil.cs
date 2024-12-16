using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;
#if UNITY_2018_3_OR_NEWER
#if UNITY_ANDROID
using UnityEngine.Android;
#endif
#endif
namespace CWJ
{
    public static class AndroidUtil 
    {
        public static int GetApiVersion()
        {
            int apiVersion = 30;
#if UNITY_ANDROID
            int apiIndex = SystemInfo.operatingSystem.IndexOf(" API-");
            if (apiIndex >= 0)
            {
                Debug.Log($"API Index: {apiIndex}");
                string versionString = SystemInfo.operatingSystem.Substring(apiIndex + 5, 2);
                Debug.Log($"API Version String: {versionString}");
                if (!int.TryParse(versionString, out apiVersion))
                {
                    Debug.Log($"int parse failed: {versionString}");
                    apiVersion = 30;
                }
            }
#endif
            return apiVersion;
        }

        public static bool NeedRequestPermission(params string[] permissionStrList)
        {
            return _NeedRequestPermission(null, null, false, permissionStrList);
        }

        public static bool NeedRequestPermissionWithPopup((string title, string msg) popupData, params string[] permissionStrList)
        {
            return _NeedRequestPermission(popupData.title, popupData.msg, true, permissionStrList);
        }


        static bool _NeedRequestPermission(string title, string msg, bool isPopMsg, string[] permissionStrList)
        {
#if UNITY_ANDROID
            string[] needAuthList = permissionStrList.Where(p => !Permission.HasUserAuthorizedPermission(p)).ToArray();
            if (needAuthList.Length == 0)
            {
                return false;
            }

            UnityAction requestPermissionAct = () => needAuthList.Do(p => Permission.RequestUserPermission(p));

            if (isPopMsg)
            {
                UIHelper.Instance.PopupMsg(title, msg, isOnlyOkBtn: true,
                    closeAction: requestPermissionAct);
            }
            else
            {
                requestPermissionAct.Invoke();
            }
#endif
            return true;
        }

        public static bool CheckNeedBluetoothPermissions(bool isPopMsg)
        {
#if UNITY_ANDROID
            int apiVersion = GetApiVersion();

            Debug.Log($"API Version: {apiVersion}");

            var _popupSet = ("BLE-ERROR", "Bluetooth 권한 확인창이 나오면\n '앱 사용중에만 허용' 그리고\n'허용' 으로 선택해주세요");

            if (apiVersion >= 31)
            {
                return NeedRequestPermissionWithPopup(popupData: _popupSet,
                "android.permission.BLUETOOTH_SCAN",
                "android.permission.BLUETOOTH_CONNECT",
                "android.permission.BLUETOOTH_ADVERTISE",
                Permission.FineLocation);
            }
            else
            {
                //api 30까지 확인하면되지만 gps기능때는 확인해야함
                return NeedRequestPermissionWithPopup(popupData: _popupSet, Permission.FineLocation);
            }
#endif
            return true;
        }
    }
}
