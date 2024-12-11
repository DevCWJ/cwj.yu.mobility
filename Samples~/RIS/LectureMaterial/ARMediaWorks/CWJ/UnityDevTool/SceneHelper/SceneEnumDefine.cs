using UnityEngine;
using UnityEngine.Playables;

namespace CWJ.SceneHelper
{
    public static class SceneEnumDefine
    {
#if UNITY_EDITOR
        public const string CWJ_SCENEENUM_ENABLED = "CWJ_SCENEENUM_ENABLED";

        public const string CWJ_SCENEENUM_DISABLED = "CWJ_SCENEENUM_DISABLED";

        public const string IsSceneEnumSync = "isSceneEnumSync";

        public const string ScriptName_SceneEnum = "SceneEnum_IncludeDisabled";

        public const string EnumName_BuildSceneEnum = "SceneEnum";

        //
        public const string ClassName_IncludeDisabledScene = "SceneEnum_IncludeDisabled";

        public const string EnumName_DisabledSceneEnum = "SceneEnum_Disabled";

        public const string EnumName_SortedByBuildIndex = "SceneEnum_BuildIndex";

        public const string EnumName_SortedByBuildSettings = "SceneEnum_BuildSettings";

        private static string _PATH = null;

        public static string PATH
        {
            get
            {
                if (_PATH == null)
                {
                    _PATH = PathUtil.MyRelativePath + $"{nameof(SceneHelper)}/{ScriptName_SceneEnum}.cs";
                }
                return _PATH;
            }
        }

        public const string Summary_OnlyEnabled =
                            ("/// <summary>\n" +
                            "/// 활성화된 씬만 존재.\n" +
                            "/// <para><see langword=\"1. How To Use\"/> : <see langword=\"string\"/>으로 변환 할 때 <see cref=\"" + nameof(SceneEnumUtil) + "." + nameof(SceneEnumUtil.ToSceneName) + "\"/> 을 사용할것.</para>\n" +
                            "/// <para>(<see cref=\"" + ScriptName_SceneEnum + "\"/>에 저장될때 공백'" + Space + "'은 '" + Space_Replaced + "'로 대체, hyphen'" + Hyphen + "'은 '" + Hyphen_Replaced + "'로 대체, 첫 글자가 숫자일땐 '" + ToPreventStartWithNumber + "'가 붙는 등 이름이 자동으로 수정되어 적용됨)</para>\n" +
                            "/// <para><see langword=\"2. Scene Naming Rule\"/> : <see cref=\"" + nameof(SceneEnumUtil) + "." + nameof(SceneEnumUtil.ToSceneName) + "\"/> 을 사용하면 되지만 그래도 특수문자나 공백은 Scene이름으로 사용을 지양.</para>\n" +
                            "/// <para><see langword=\"3. Update Condition\"/> : Build Settings에서 씬 목록 수정된 후 <see langword=\"and\"/> (5초 후 <see langword=\"or\"/> ctrl+s 누른 직후 <see langword=\"or\"/> compile 직후 <see langword=\"or\"/> 빌드 직전) 자동 업데이트.</para>\n" +
                            "/// <para><see langword=\"4. How To Disable\"/> : Enum이 Scene과 동기화되는 기능을 끄려면 '" + AccessibleEditor.CustomDefine.CustomDefine_Window.WindowMenuItemPath + "' 에서 '" + IsSceneEnumSync + "' 비활성화.</para>\n" +
                            "///CWJ - UnityDevTool (Version 10.7 [20.01.31])\n" +
                            "/// </summary>");

        public const string Summary_OnlyDisabledClass = ("/// <summary>\n" +
                                                    "/// 비활성화된 씬도 포함\n" +
                                                    "/// </summary>");

        public const string Summary_All_SortedByBuildSettings =
                                            ("/// <summary>\n" +
                                            "/// 활성화 and 비활성화 씬 존재.\n" +
                                            "/// <para>Build Settings의 순서와 동일하게 정렬.</para>\n" +
                                            "/// <para><see langword=\"string\"/>으로 변환 할 때 <see cref=\"" + nameof(SceneEnumUtil) + "." + nameof(SceneEnumUtil.ToSceneName) + "\"/> 을 사용할것.</para>\n" +
                                            "/// </summary>");

        public const string Summary_All_SortedByBuildIndex =
                                        ("/// <summary>\n" +
                                        "/// 활성화 and 비활성화 씬 존재.\n" +
                                        "/// <para>sceneBuildIndex를 기준으로 정렬.(활성화된 씬부터 기입되어있음)</para>\n" +
                                        "/// <para><see langword=\"string\"/>으로 변환 할 때 <see cref=\"" + nameof(SceneEnumUtil) + "." + nameof(SceneEnumUtil.ToSceneName) + "\"/> 을 사용할것.</para>\n" +
                                        "/// </summary>");

        public const string Summary_OnlyDisabledEnum =
                    ("/// <summary>\n" +
                    "/// 비활성화된 씬만 존재.\n" +
                    "/// <para><see langword=\"string\"/>으로 변환 할 때 <see cref=\"" + nameof(SceneEnumUtil) + "." + nameof(SceneEnumUtil.ToSceneName) + "\"/> 을 사용할것.</para>\n" +
                    "/// </summary>");
#endif

        public const string Space = " ";
        public const string Space_Replaced = "ˇ";

        public const string Hyphen = "-";
        public const string Hyphen_Replaced = "ㅡ";

        public const string Dot = ".";
        public const string Dot_Replaced = "ㆍ";

        public const string Comma = ",";
        public const string Comma_Replaced = "ㆎ";

        public const string Ampersand = "&";
        public const string Ampersand_Replaced = "ㆀ";

        public static readonly (string origin, string modify)[] InvalidCharModifySets = new (string origin, string modify)[]
        {
            (Space, Space_Replaced),
            (Hyphen, Hyphen_Replaced),
            (Dot, Dot_Replaced),
            (Comma, Comma_Replaced),
            (Ampersand, Ampersand_Replaced)
        };

        public static string ConvertValidChar(string str, bool isToValid)
        {
            if (isToValid)
            {
                if (char.IsNumber(str[0]))
                {
                    str = ToPreventStartWithNumber + str;
                }
                for (int i = 0; i < InvalidCharModifySets.Length; i++)
                {
                    str = str.Replace(InvalidCharModifySets[i].origin, InvalidCharModifySets[i].modify);
                }
            }
            else
            {
                str = str.ReplaceStart(ToPreventStartWithNumber, "");
                for (int i = 0; i < InvalidCharModifySets.Length; i++)
                {
                    str = str.Replace(InvalidCharModifySets[i].modify, InvalidCharModifySets[i].origin);
                }
            }
            return str;
        }

        public const string ToPreventStartWithNumber = "ⁿ";


    }
}