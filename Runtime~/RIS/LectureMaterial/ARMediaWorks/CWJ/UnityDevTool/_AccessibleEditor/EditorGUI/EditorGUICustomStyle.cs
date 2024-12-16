#if UNITY_EDITOR

using UnityEditor;

using UnityEngine;

namespace CWJ.AccessibleEditor
{
	/// <summary>
	/// 주의할점. EditorStyles는 lazy initialization방식이 아니라서 OnInspectorGUI에서만됨 안그러면 null반환함
	/// </summary>
	public static class EditorGUICustomStyle
	{
		public static readonly Color DefaultBackgroundColor = EditorGUIUtility.isProSkin
															   ? new Color32(56, 56, 56, 255)
															   : new Color32(194, 194, 194, 255);

		public static readonly Color DefaultTextColor = EditorStyles.label.normal.textColor;

		public static readonly Color DefaultLineColor = EditorGUIUtility.isProSkin
														   ? new Color32(26, 26, 26, 255)
														   : new Color32(116, 116, 116, 255);


		public static GUIStyle LargeLabelStyle;
		public static GUIStyle LargeBoldLabelStyles;

		public static GUIStyle Label_Centered;


		public static GUIStyle NonPaddingButton;

		public static GUIStyle InspectorBox;

		public static GUIStyle OuterBox;
		public static GUIStyle Box;
		public static GUIStyle TransparentBox;

		public static GUIStyle Foldout;
		public static GUIStyle FoldoutHeader_Big;

		public static GUIStyle Button_TextAlignmentLeft;
		public static GUIStyle Button_NoBorder;


		//public static GUIStyle Button;

		static EditorGUICustomStyle()
		{
			//Button = new GUIStyle(EditorStyles.miniButton);
			//Button.font = Font.CreateDynamicFontFromOSFont(new[] { "Terminus (TTF) for Windows", "Calibri" }, 17);

			//Text = new GUIStyle(EditorStyles.label);
			//Text.richText = true;
			//Text.contentOffset = new Vector2(0, 5);
			//Text.font = Font.CreateDynamicFontFromOSFont(new[] { "Terminus (TTF) for Windows", "Calibri" }, 14);

			LargeLabelStyle = new GUIStyle(EditorStyles.largeLabel);
			LargeLabelStyle.alignment = TextAnchor.MiddleCenter;
			LargeLabelStyle.fontSize = 14;
			LargeLabelStyle.richText = true;
			LargeLabelStyle.padding = new RectOffset(0, 0, 0, 0);

			LargeBoldLabelStyles = new GUIStyle(LargeLabelStyle);
			LargeBoldLabelStyles.fontStyle = FontStyle.Bold;
			LargeBoldLabelStyles.wordWrap = false;

			Label_Centered = new GUIStyle(GUI.skin.textField);
			Label_Centered.alignment = TextAnchor.MiddleCenter;

			FoldoutHeader_Big = new GUIStyle(
#if UNITY_2019_3_OR_NEWER
				EditorStyles.foldoutHeader
#else
				EditorStyles.foldout
#endif
				);

			Foldout = new GUIStyle(EditorStyles.foldout);
			InspectorBox = new GUIStyle(EditorStyles.helpBox);
			Box = new GUIStyle(EditorStyles.helpBox);
			OuterBox = new GUIStyle(EditorStyles.helpBox);
			
			Foldout.fontStyle = FontStyle.Bold;
#if UNITY_2019_3_OR_NEWER
			Foldout.overflow = new RectOffset(-10, 0, 0, 0);
            Foldout.padding = new RectOffset(15, 0, 0, 0);
			FoldoutHeader_Big.fontSize = Foldout.fontSize = 13;
			OuterBox.padding = new RectOffset(10, 10, 5, 6);
#else
			Foldout.overflow = new RectOffset(0, 0, -1, 0);
			Foldout.padding = new RectOffset(15, 0, 0, 0);
			FoldoutHeader_Big.fontSize = Foldout.fontSize = 12;
			OuterBox.padding = new RectOffset(10, 10, 5, 5);
#endif
			Box.padding = new RectOffset(0, 0, 0, 0);

			InspectorBox.padding = new RectOffset(18, 0, 0, 0);

			Button_TextAlignmentLeft = new GUIStyle(GUI.skin.button);
			Button_TextAlignmentLeft.alignment = TextAnchor.MiddleLeft;

			NonPaddingButton = new GUIStyle(GUI.skin.button);
			NonPaddingButton.padding = new RectOffset(0, 0, 0, 0);
			NonPaddingButton.margin = new RectOffset(0, 0, 0, 0);

			Button_NoBorder = new GUIStyle(GUI.skin.label);
			Button_NoBorder.margin = new RectOffset(0, 0, 2, 0);
			Button_NoBorder.padding = new RectOffset(0, 0, 0, 0);
			Button_NoBorder.border = new RectOffset(0, 0, 0, 0);
		}
	}
}

#endif