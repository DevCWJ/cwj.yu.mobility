namespace CWJ.AccessibleEditor.DebugSetting
{
#if NET_STANDARD_2_0 || NET_STANDARD_2_1
#if UNITY_2021_1_OR_NEWER
#error [1. 상단 탭 Edit > Project Settings 클릭]
#error [2. 우측상단 검색란: 'API Compatibility Level' 입력]
#error [3. 'API Compatibility Level'를 '.NET Framework'로 설정]
#error UnityDevTool is incapable of compiling source code against the .NET Standard 2.1 API surface. You can change the API Compatibility Level to '.Net Framework' in the Player settings.
#else
#error (Project Settings>Player>Other Settings 'API Compatibility Level' Set '.Net 4.x') UnityDevTool is incapable of compiling source code against the .NET Standard 2.0 API surface. You can change the API Compatibility Level to '.Net 4.x' in the Player settings.
#endif
#endif

    /// <summary>
    /// <see langword="Do Not Edit"/>
    /// <para>Define Symbol Names</para>
    /// </summary>
    public class CWJDefineSymbols
    {
        public const string CWJ_UNITYDEVTOOL = nameof(CWJ_UNITYDEVTOOL);

        //

        public const string CWJ_LOG_DISABLED = nameof(CWJ_LOG_DISABLED);

        public const string CWJ_LOG_DISABLED_IN_BUILD = nameof(CWJ_LOG_DISABLED_IN_BUILD);

        public const string UNITY_ASSERTIONS = nameof(UNITY_ASSERTIONS); //Debug.Assert기능 관련 Define. 에디터에서는 등록되어있다가 빌드되면 유니티자체적으로 undef 되기때문에 LogSave 활성화 되면 자동으로 등록되게 해놓음

        public const string CWJ_LOG_SAVE = nameof(CWJ_LOG_SAVE);

        public const string CWJ_EDITOR_DEBUG_ENABLED = nameof(CWJ_EDITOR_DEBUG_ENABLED);

        //

        public const string CWJ_LOG_ENABLED = "CWJ_FAKE_LOG_ENABLED_CHOWOOJEONG"; //절대로 Scripting Define Symbols에 등록 될 일이 없는 이름이여야함

        //

        public const string CWJ_RUNTIMEDEBUGGING_DISABLED = nameof(CWJ_RUNTIMEDEBUGGING_DISABLED);

        //

        // Exists
        public const string CWJ_EXISTS_CWJVR = nameof(CWJ_EXISTS_CWJVR);

        public const string CWJ_EXISTS_STEAMVR = nameof(CWJ_EXISTS_STEAMVR);

        public const string CWJ_EXISTS_RUNTIMENAVMESH = nameof(CWJ_EXISTS_RUNTIMENAVMESH);

        public const string CWJ_EXISTS_NEWINPUTSYSTEM = nameof(CWJ_EXISTS_NEWINPUTSYSTEM); // = !ENABLE_LEGACY_INPUT_MANAGER

        public const string CWJ_EXISTS_UNIRX = nameof(CWJ_EXISTS_UNIRX);

        public const string CWJ_EXISTS_EDITORCOROUTINE = nameof(CWJ_EXISTS_EDITORCOROUTINE);

        public const string CWJ_EXISTS_URP = nameof(CWJ_EXISTS_URP);

        public const string CWJ_EXISTS_RUNTIMEDEBUGGING = nameof(CWJ_EXISTS_RUNTIMEDEBUGGING);

        public const string CWJ_EXISTS_ADDON = nameof(CWJ_EXISTS_ADDON);

        public const string CWJ_DEVELOPMENT_BUILD = nameof(CWJ_DEVELOPMENT_BUILD);

        //

        // Manager
        public const string CWJ_MULTI_DISPLAY = nameof(CWJ_MULTI_DISPLAY);

#if CWJ_LOG_DISABLED || (!UNITY_EDITOR && CWJ_LOG_DISABLED_IN_BUILD)
        private const string DEVELOPED_BY_CWJ_CHECK = Debug.DEVELOPED_BY_CWJ;
        //^오류뜨면 'Debug.cs'의 첫줄이 '#if CWJ_LOG_DISABLED || (!UNITY_EDITOR && CWJ_LOG_DISABLED_IN_BUILD)' 가 아닌거나 없는거임
#endif
    }
}
