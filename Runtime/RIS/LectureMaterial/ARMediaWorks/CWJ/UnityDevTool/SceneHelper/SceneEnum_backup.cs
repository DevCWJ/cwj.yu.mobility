//namespace CWJ.SceneHelper
//{
//    /// <summary>
//    /// <para><see langword="1. Update Condition"/> : Build Settings에서 씬 목록이 수정된 후 <see langword="and"/> (5초 후 <see langword="or"/> ctrl+s 누른 직후 <see langword="or"/> compile 직후 <see langword="or"/> 빌드하기 직전) 자동 업데이트.</para>
//    /// <para><see langword="2. Scene Naming Rule"/> : 공백, hyphen이 포함된 네이밍은 지양.</para>
//    /// <para>공백 또는 hyphen이 포함된 Scene 이름이 있을 경우 string으로 변환 할 때 <see cref="SceneEnumUtil.ToSceneName"/> 을 사용할것.</para>
//    /// <para>(공백이나 hyphen은 <see cref="SceneEnum"/>에 저장될때 유효한 기호로 변환되어서 저장되기 때문)</para>
//    /// <para>...</para>
//    /// <para><see langword="3. How To Disable"/> : Enum이 Scene과 동기화되는 기능을 끄고싶다면 'CWJ/Custom Define symbol setting' 에서 'isSceneEnumSync' 비활성화.</para>
//    /// </summary>
//    public enum SceneEnum
//    {
//    }

//#if UNITY_EDITOR
//    public class EditorOnly
//    {
//        /// <summary>
//        /// EDITOR 전용 (비활성화되어있는 씬까지 모두 포함)
//        /// <para>enable부터 기입되어있어서 build index와 일치함.</para>
//        /// </summary>
//        public enum AllSceneEnum
//        {
//        }

//        /// <summary>
//        /// EDITOR 전용 (비활성화되어있는 씬까지 모두 포함)
//        /// <para>Build Settings의 순서와 비교용</para>
//        /// </summary>
//        public enum ReadonlySceneEnum
//        {
//        }
//    }
//#endif

//}
