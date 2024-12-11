using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using CWJ.SceneHelper;

namespace CWJ
{
#if UNITY_EDITOR
    using UnityEditor;
    using CWJ.AccessibleEditor;

    public enum BuildVersionEnum
    {
        A,
        B,
        C
    }

    public static class BuildVersionMngr_Editor
    {
        //[InitializeOnLoadMethod]
        public static void InitializeOnLoad()
        {
            //BuildEventSystem.DisplayDialogEvent += OnBuildEventSystem_DisplayDialog;
        }

        private static void OnBuildEventSystem_DisplayDialog()
        {
            //List<SceneEnum_IncludeDisabled.SceneEnum_BuildSettings> originVersionScenes = new List<SceneEnum_IncludeDisabled.SceneEnum_BuildSettings>(30)
            //{ 
            //    //Intro, 공간인지훈련 외의 씬들
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.intro,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.main,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.Game_Shoot,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.home_empty,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.home_furniture,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.Mart_interior,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.Mountain,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.Practice,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.village,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.downtown,
            //    SceneEnum_IncludeDisabled.SceneEnum_BuildSettings.Basic_Classroom
            //};
            ////자기중심훈련외의 씬들추가
            //originVersionScenes.AddRange(SpaceAware.SpaceAwareDefine.AllocentricScenes.ConvertAll(s => (EditorOnly.IncludeDisabledSceneEnum)s));
            //originVersionScenes.AddRange(SpaceAware.SpaceAwareDefine.PracticeScenes.ConvertAll(s => (EditorOnly.IncludeDisabledSceneEnum)s));

            //EditorOnly.IncludeDisabledSceneEnum[] liteVersionScene = new EditorOnly.IncludeDisabledSceneEnum[]
            //{

            //};

            //int[] originVersionIndexs =  EditorBuildSettings.scenes.FindIndex(s => System.IO.Path.GetFileNameWithoutExtension(s.path).Equals(CWJ.SceneHelper.SceneEnum.Intro.ToSceneName()));
            //int[] disabledIntroSceneIndex = 

            //if (DisplayDialogUtil.DisplayDialog<BuildPlayerOptions>("Space Awareness - 자기중심 Lite버전 빌드입니까?", ok: "예", cancel: "아니오"))
            //{

            //}

            //EditorBuildSettings.scenes[introSceneIndex].enabled = true;
        }
    }
#endif

    public class BuildVersionMngr : CWJ.Singleton.SingletonBehaviourDontDestroy<BuildVersionMngr>, CWJ.Singleton.IDontAutoCreatedWhenNull
    {

    }
}