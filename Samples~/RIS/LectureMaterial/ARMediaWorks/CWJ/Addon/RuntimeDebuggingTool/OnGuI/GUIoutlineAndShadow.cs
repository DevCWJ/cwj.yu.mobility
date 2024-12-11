using UnityEngine;

public class GUIoutlineAndShadow : MonoBehaviour
{
    public static void LabelAndOutline(Rect rect, string text, GUIStyle style, Color outlineColor, float outLineSize)
    {
        float halfSize = outLineSize * 0.5F;
        GUIStyle backupStyle = new GUIStyle(style);
        Color backupGlobalColor = GUI.color;//

        Color labelColor = style.normal.textColor;
        style.normal.textColor = outlineColor;
        GUI.color = outlineColor;//

        rect.x -= halfSize;
        GUI.Label(rect, text, style);

        rect.x += outLineSize;
        GUI.Label(rect, text, style);

        rect.x -= halfSize;
        rect.y -= halfSize;
        GUI.Label(rect, text, style);

        rect.y += outLineSize;
        GUI.Label(rect, text, style);

        rect.y -= halfSize;
        style.normal.textColor = labelColor;
        GUI.color = backupGlobalColor;//
        GUI.Label(rect, text, style);

        style = backupStyle;
    }

    public static void ContentAndShadow(Rect rect, GUIContent content, GUIStyle style, Color shadowColor,
                                    Vector2 direction)
    {
        GUIStyle backupStyle = style;
        Color labelColor = style.normal.textColor;
        style.normal.textColor = shadowColor;
        rect.x += direction.x;
        rect.y += direction.y;
        GUI.Label(rect, content, style);

        style.normal.textColor = labelColor;
        rect.x -= direction.x;
        rect.y -= direction.y;
        GUI.Label(rect, content, style);

        style = backupStyle;
    }

    public static void LabelAndShadow(Rect rect, string text, GUIStyle style, Color shadowColor,
                                Vector2 direction)
    {
        GUIStyle backupStyle = style;
        Color labelColor = style.normal.textColor;
        style.normal.textColor = shadowColor;
        rect.x += direction.x;
        rect.y += direction.y;
        GUI.Label(rect, text, style);

        style.normal.textColor = labelColor;
        rect.x -= direction.x;
        rect.y -= direction.y;
        GUI.Label(rect, text, style);

        style = backupStyle;
    }

    public static void DrawLayoutShadow(GUIContent content, GUIStyle style, Color shadowColor,
                                    Vector2 direction, params GUILayoutOption[] options)
    {
        ContentAndShadow(GUILayoutUtility.GetRect(content, style, options), content, style, shadowColor, direction);
    }

    public static bool DrawButtonWithShadow(Rect r, GUIContent content, GUIStyle style, float shadowAlpha, Vector2 direction)
    {
        GUIStyle letters = new GUIStyle(style);
        letters.normal.background = null;
        letters.hover.background = null;
        letters.active.background = null;
        letters.normal.textColor = r.Contains(Event.current.mousePosition) ? letters.hover.textColor : letters.normal.textColor;

        bool result = GUI.Button(r, content, style);

        ContentAndShadow(r, content, letters, new Color(0f, 0f, 0f, shadowAlpha), direction);

        return result;
    }

    public static bool DrawLayoutButtonWithShadow(GUIContent content, GUIStyle style, float shadowAlpha,
                                                   Vector2 direction, params GUILayoutOption[] options)
    {
        return DrawButtonWithShadow(GUILayoutUtility.GetRect(content, style, options), content, style, shadowAlpha, direction);
    }
}