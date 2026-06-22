#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EvertechMapSDK;

/// <summary>
/// Editor-only inspector for <see cref="MapSettingDef"/>. Shows only the fields that apply to
/// the chosen <see cref="MapSettingType"/> (Bool / Slider / Enum) instead of all of them at once.
///
/// This does NOT modify the SDK type or its serialized data — it only changes how one list entry
/// is drawn in the Inspector. It lives under an Editor/ folder, so it is never included in a build
/// or in a map AssetBundle. Safe to ship with the template.
/// </summary>
[CustomPropertyDrawer(typeof(MapSettingDef))]
public class MapSettingDefDrawer : PropertyDrawer
{
    const float Pad = 2f;
    static float Line => EditorGUIUtility.singleLineHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var keyProp = property.FindPropertyRelative("key");
        var typeProp = property.FindPropertyRelative("type");

        // Foldout header with a readable summary: "key  —  Type"
        string keyText = string.IsNullOrEmpty(keyProp.stringValue) ? "(no key)" : keyProp.stringValue;
        string summary = keyText + "   —   " + (MapSettingType)typeProp.enumValueIndex;
        var headerRect = new Rect(position.x, position.y, position.width, Line);
        property.isExpanded = EditorGUI.Foldout(headerRect, property.isExpanded, summary, true);

        if (!property.isExpanded)
        {
            EditorGUI.EndProperty();
            return;
        }

        EditorGUI.indentLevel++;
        float y = position.y + Line + Pad;

        // Always shown
        y = Draw(position, y, keyProp);
        y = Draw(position, y, property.FindPropertyRelative("label"));
        y = Draw(position, y, typeProp);

        // Only the fields relevant to the selected type
        switch ((MapSettingType)typeProp.enumValueIndex)
        {
            case MapSettingType.Bool:
                y = Draw(position, y, property.FindPropertyRelative("defaultBool"), "Default Value");
                break;
            case MapSettingType.Slider:
                y = Draw(position, y, property.FindPropertyRelative("min"));
                y = Draw(position, y, property.FindPropertyRelative("max"));
                y = Draw(position, y, property.FindPropertyRelative("step"), "Step (0 = smooth)");
                y = Draw(position, y, property.FindPropertyRelative("defaultFloat"), "Default Value");
                break;
            case MapSettingType.Enum:
                y = Draw(position, y, property.FindPropertyRelative("options"));
                y = Draw(position, y, property.FindPropertyRelative("defaultOption"), "Default Option");
                break;
        }

        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

    static float Draw(Rect pos, float y, SerializedProperty prop, string overrideLabel = null)
    {
        float h = EditorGUI.GetPropertyHeight(prop, true);
        var r = new Rect(pos.x, y, pos.width, h);
        if (overrideLabel != null)
            EditorGUI.PropertyField(r, prop, new GUIContent(overrideLabel), true);
        else
            EditorGUI.PropertyField(r, prop, true);
        return y + h + Pad;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded)
            return Line;

        var typeProp = property.FindPropertyRelative("type");
        float h = Line + Pad;                 // foldout header
        h += H(property, "key");
        h += H(property, "label");
        h += H(property, "type");

        switch ((MapSettingType)typeProp.enumValueIndex)
        {
            case MapSettingType.Bool:
                h += H(property, "defaultBool");
                break;
            case MapSettingType.Slider:
                h += H(property, "min");
                h += H(property, "max");
                h += H(property, "step");
                h += H(property, "defaultFloat");
                break;
            case MapSettingType.Enum:
                h += H(property, "options");
                h += H(property, "defaultOption");
                break;
        }
        return h;
    }

    static float H(SerializedProperty parent, string rel)
    {
        return EditorGUI.GetPropertyHeight(parent.FindPropertyRelative(rel), true) + Pad;
    }
}
#endif
