//(Obsolete)
//폐기되었지만 지우면 안됨
//using UnityEngine;
//using UnityEditor;
//using System.Linq;
//using System.Collections.Generic;

//SceneEnum 자동 생성 기능 만들어서 필요없어짐
//namespace CWJ.EditorOnly
//{
//    [CustomPropertyDrawer(typeof(SceneNameAttribute))]
//    public class SceneNameAttribute_Editor : PropertyDrawer
//    {
//        private SceneNameAttribute sceneNameAttribute => (SceneNameAttribute)attribute;

//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            string[] sceneNames = GetEnabledSceneNames();

//            if (sceneNames.Length == 0)
//            {
//                EditorGUI.LabelField(position, ObjectNames.NicifyVariableName(property.name), "Scene is Empty");
//                return;
//            }

//            int[] sceneNumbers = new int[sceneNames.Length];

//            InitSceneNumbers(sceneNumbers, sceneNames);

//            if (!string.IsNullOrEmpty(property.stringValue))
//                sceneNameAttribute.selectedValue = GetIndex(sceneNames, property.stringValue);

//            sceneNameAttribute.selectedValue = EditorGUI.IntPopup(position, label.text, sceneNameAttribute.selectedValue, sceneNames, sceneNumbers);

//            property.stringValue = sceneNames[sceneNameAttribute.selectedValue];
//        }

//        string[] GetEnabledSceneNames()
//        {
//            List<EditorBuildSettingsScene> scenes = (sceneNameAttribute.enableOnly ? EditorBuildSettings.scenes.Where(scene => scene.enabled) : EditorBuildSettings.scenes).ToList();
//            HashSet<string> sceneNames = new HashSet<string>();
//            scenes.ForEach(scene =>
//            {
//                sceneNames.Add(scene.path.Substring(scene.path.LastIndexOf("/") + 1).Replace(".unity", string.Empty));
//            });
//            return sceneNames.ToArray();
//        }

//        void InitSceneNumbers(int[] sceneNumbers, string[] sceneNames)
//        {
//            for (int i = 0; i < sceneNames.Length; ++i)
//            {
//                sceneNumbers[i] = i;
//            }
//        }

//        int GetIndex(string[] sceneNames, string sceneName)
//        {
//            int result = 0;
//            for (int i = 0; i < sceneNames.Length; ++i)
//            {
//                if (sceneName == sceneNames[i])
//                {
//                    result = i;
//                    break;
//                }
//            }
//            return result;
//        }
//    }
//}