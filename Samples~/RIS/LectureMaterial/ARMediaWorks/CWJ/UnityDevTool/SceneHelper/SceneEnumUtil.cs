using System;

namespace CWJ.SceneHelper
{
    public static class SceneEnumUtil
    {
        /// <summary>
        /// <para><see cref="CWJ.SceneHelper.SceneEnum"/> 혹은 <see cref="EditorOnly.IncludeDisabledSceneEnum"/>, <see cref="EditorOnly.SortedByBuildSettingSceneEnum"/>만 사용가능</para>
        /// 공백, 하이픈가 포함되어있거나 첫글자가 숫자인 씬이름의 경우 이 함수로 이름을 변환해야함
        /// </summary>
        public static string ToSceneName<T>(this T sceneEnum) where T : Enum
        {
#if (CWJ_SCENEENUM_ENABLED)
            Type enumType = typeof(T);
            if (!enumType.Equals(typeof(SceneEnum))
                && !enumType.Equals(typeof(SceneEnum_IncludeDisabled.SceneEnum_BuildSettings))
                && !enumType.Equals(typeof(SceneEnum_IncludeDisabled.SceneEnum_BuildIndex))
                && !enumType.Equals(typeof(SceneEnum_IncludeDisabled.SceneEnum_Disabled)))
            {
                CWJ.DebugLogUtil.PrintLogException<NotSupportedException>($"{enumType.Name} != {nameof(SceneEnum)}", isPreventStackTrace: true);
                return null;
            }
#else
            return null;
#endif
            return SceneEnumDefine.ConvertValidChar(sceneEnum.ToString(), false);
        }

        public static string ToSceneName<T>(this T sceneEnum, out bool isValid) where T : Enum
        {
            string name = ToSceneName(sceneEnum);
            isValid = !string.IsNullOrEmpty(name);
            return name;
        }
    }
}
