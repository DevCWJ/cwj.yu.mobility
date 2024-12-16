
using UnityEngine;

namespace CWJ
{
    using CWJ.AccessibleEditor;

    public class SecurityManager : CWJ.Singleton.SingletonBehaviour<SecurityManager>, CWJ.Singleton.IDontAutoCreatedWhenNull
    {
        protected override void _Reset()
        {
            Debug.LogError(gameObject.name);
            Debug.LogError("isLoaded:" + gameObject.scene.isLoaded);
            Debug.LogError("isSubScene:" + gameObject.scene.isSubScene);
        }
//        public const string CWJ_APP_EXPIRY_DATE = nameof(CWJ_APP_EXPIRY_DATE);

        //#if !CWJ_MULTI_DISPLAY
        //        [InvokeButton]
        //#endif
        //        private void SetDefineSymbol()
        //        {
        //            DefineSymbolUtil.AddCustomDefineSymbol(CWJ_APP_EXPIRY_DATE, true);
        //            Ping();
        //        }

        //        protected override void _Reset()
        //        {
        //#if UNITY_EDITOR
        //            SetDefineSymbol();
        //#endif
        //        }

        //#if CWJ_APP_EXPIRY_DATE
        //        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //        private static void OnBeforeSceneLoad()
        //        {
        //            if (CheckIsDeadline())
        //            {
        //                AppQuit();
        //            }
        //        }
        //#endif

        //            [CWJ.VisualizeProperty] static int expirationRemainDay => 60;
        //        [CWJ.InvokeButton]
        //        static bool CheckIsDeadline()
        //        {
        //            int elapsedDay;
        //            bool isDeadLine = !CWJ.BuildTimeStamp.BuildTimeStampMngr.GetElapsdedDayFromBuildTime(out elapsedDay) || elapsedDay > expirationRemainDay;
        //            if (isDeadLine)
        //            {
        //                CWJ.BuildTimeStamp.BuildTimeStampMngr.CreateDeadlineLogTxt();
        //                Debug.LogError("Is Deadline. Forced App Quit");
        //            }
        //            else
        //            {
        //                System.IO.File.Delete(CWJ.BuildTimeStamp.BuildTimeStampMngr.DeadlineLogTxtPath);
        //                Debug.Log($"Deadline is {expirationRemainDay - elapsedDay} day remain");
        //            }
        //            return isDeadLine;
        //        }

        //        static void AppQuit()
        //        {
        //            Application.Quit();
        //#if UNITY_EDITOR
        //            UnityEditor.EditorApplication.isPlaying = false;
        //#endif
        //        }
    }

}