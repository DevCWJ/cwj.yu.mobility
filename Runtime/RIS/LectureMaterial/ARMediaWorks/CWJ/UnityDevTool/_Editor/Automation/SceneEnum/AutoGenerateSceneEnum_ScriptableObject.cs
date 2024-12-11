namespace CWJ.SceneHelper
{
    public sealed class AutoGenerateSceneEnum_ScriptableObject : Initializable_ScriptableObject
    {
        public override bool IsAutoReset => false;
        public string[] editorAllScenes;

        public string[] enableScenes;
        public string[] disableScenes;

        public override void OnReset(bool isNeedSave = false)
        {
            this.enableScenes = new string[0];
            this.disableScenes = new string[0];
            this.editorAllScenes = new string[0];
            base.OnReset(isNeedSave);
        }
    }
}