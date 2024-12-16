#if UNITY_EDITOR

using UnityEditor;

using UnityEngine;

namespace CWJ.AccessibleEditor.Function
{
    [CustomEditor(typeof(MissingObjectFindManager))]
    public class MissingObjectFindManager_Inspector : InspectorBehaviour<MissingObjectFindManager>
    {
        protected override void DrawInspector()
        {
            if (Target.missingObjs.LengthSafe() == 0)
            {
                WriteSubTitle("Find Missing Objects In This Scene");
                if (GUILayout.Button("Find"))
                {
                    Target.missingObjs = MissingObjectFinder_Function.GetMissingObjsInScene();
                }
                return;
            }

            EditorGUILayout.BeginHorizontal();
            WriteSubTitle("Past the Copied Component");

            if (Target.copyComp == null)
            {
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.HelpBox("copyComponent에 component를 넣으려면 : " +
                                        "\n1.빈오브젝트에 복사를 원하는 component 추가 후 프리팹으로 만듬" +
                                        "\n2.Project탭에서 프리팹을 클릭후 Inpsector창에서 해당 component를 드래그로 옮겨서 copyComponent에 할당", MessageType.Error);
            }
            else
            {
                if (GUILayout.Button("Paste"))
                {
                    if (DisplayDialogUtil.DisplayDialogReflection($"Warning!\n씬의 모든 Missing Component가 지워집니다\nMissing compoent를 모두 지우고,\ncopyComponent를 붙여넣으시겠습니까?", "Ok", "Cancel"))
                    {
                        MissingObjectFinder_Function.PasteComponentInMissingScene(Target.missingObjs, Target.copyComp);
                    }
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.HelpBox("주의! copyComponent를 붙여넣을땐 missingObject가 모두 삭제되어야함\nPaste버튼 누르면 missing component들 모두 삭제되고나서 붙여넣어짐", MessageType.Warning);
            }
            BeginError(Target.copyComp == null);
            Draw(nameof(Target.copyComp));
            EndError();

            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            WriteSubTitle($"Missing Objects In Scene");
            if (Target.missingObjs[0] == null)
            {
                if (GUILayout.Button("Update"))
                {
                    Target.missingObjs = MissingObjectFinder_Function.GetMissingObjsInScene();
                }
            }
            else
            {
                if (GUILayout.Button("Destroy All"))
                {
                    if (DisplayDialogUtil.DisplayDialogReflection($"Warning!\n씬의 모든 Missing Component를 삭제하시겠습니까?", "Ok", "Cancel"))
                    {
                        MissingObjectFinder_Function.DestroyMissingComp(Target.missingObjs);
                    }
                    Target.missingObjs = MissingObjectFinder_Function.GetMissingObjsInScene();
                }
            }

            EditorGUILayout.EndHorizontal();

            Draw(nameof(Target.missingObjs));

        }
    }
}

#endif