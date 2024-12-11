#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace CWJ.YU.Mobility
{
	public class AnimationVariableRenamer : EditorWindow
	{
		private string oldVariableName = "";
		private string newVariableName = "";
		private string targetComponentType = "RendererEnabledSync"; // 변경하려는 컴포넌트 타입

		[MenuItem("CWJ/Tools/Animation Variable Renamer")]
		static void Init()
		{
			AnimationVariableRenamer window = (AnimationVariableRenamer)GetWindow(typeof(AnimationVariableRenamer));
			window.Show();
		}

		void OnGUI()
		{
			GUILayout.Label("애니메이션 변수명 변경기", EditorStyles.boldLabel);
			targetComponentType = EditorGUILayout.TextField("대상 컴포넌트 타입", targetComponentType);
			oldVariableName = EditorGUILayout.TextField("기존 변수명", oldVariableName);
			newVariableName = EditorGUILayout.TextField("새 변수명", newVariableName);

			if (GUILayout.Button("변수명 변경"))
			{
				if (string.IsNullOrEmpty(oldVariableName) || string.IsNullOrEmpty(newVariableName))
				{
					EditorUtility.DisplayDialog("오류", "기존 변수명과 새 변수명을 모두 입력하세요.", "확인");
				}
				else
				{
					RenameAnimationVariables();
				}
			}
		}

		void RenameAnimationVariables()
		{
			string[] animationGuids = AssetDatabase.FindAssets("t:AnimationClip");
			int totalClips = animationGuids.Length;
			int modifiedClips = 0;

			for (int i = 0; i < totalClips; i++)
			{
				string guid = animationGuids[i];
				string path = AssetDatabase.GUIDToAssetPath(guid);
				AnimationClip animClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);

				EditorCurveBinding[] curveBindings = AnimationUtility.GetCurveBindings(animClip);
				bool hasModified = false;

				for (int j = 0; j < curveBindings.Length; j++)
				{
					EditorCurveBinding binding = curveBindings[j];

					if (binding.type.Name == targetComponentType && binding.propertyName == oldVariableName)
					{
						AnimationCurve curve = AnimationUtility.GetEditorCurve(animClip, binding);

						EditorCurveBinding newBinding = binding;
						newBinding.propertyName = newVariableName;

						AnimationUtility.SetEditorCurve(animClip, binding, null);
						AnimationUtility.SetEditorCurve(animClip, newBinding, curve);

						hasModified = true;
					}
				}

				if (hasModified)
				{
					EditorUtility.SetDirty(animClip);
					modifiedClips++;
					Debug.Log($"애니메이션 클립 '{animClip.name}'에서 변수명을 변경했습니다.");
				}

				// 진행 상황 표시
				EditorUtility.DisplayProgressBar("변수명 변경 중...", $"{i + 1}/{totalClips} 처리 중", (float)(i + 1) / totalClips);
			}

			EditorUtility.ClearProgressBar();
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			EditorUtility.DisplayDialog("완료", $"총 {modifiedClips}개의 애니메이션 클립에서 변수명을 변경했습니다.", "확인");
		}
	}
}

#endif
