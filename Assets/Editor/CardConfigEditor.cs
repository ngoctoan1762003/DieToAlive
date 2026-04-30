using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CardConfig))]
public class CardConfigDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty statType = property.FindPropertyRelative("statType");
        SerializedProperty cardType = property.FindPropertyRelative("cardType");
        SerializedProperty chooseBetterDice = property.FindPropertyRelative("chooseBetterDice");
        SerializedProperty damageType = property.FindPropertyRelative("damageType");
        SerializedProperty weaponID = property.FindPropertyRelative("weaponID");
        SerializedProperty haste = property.FindPropertyRelative("haste");

        Rect fieldRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        
        EditorGUI.LabelField(fieldRect, label, EditorStyles.boldLabel);
        fieldRect.y += EditorGUIUtility.singleLineHeight + 2;

        void DrawField(SerializedProperty prop, bool includeChildren = false) {
            EditorGUI.PropertyField(fieldRect, prop, includeChildren);
            fieldRect.y += EditorGUI.GetPropertyHeight(prop) + 2;
        }

        DrawField(property.FindPropertyRelative("cardID"));
        DrawField(statType, true);
        DrawField(property.FindPropertyRelative("actionNeed"));
        DrawField(cardType);
        DrawField(weaponID);
        DrawField(haste);

        if (statType.arraySize >= 2)
        {
            DrawField(chooseBetterDice);
        }

        if (cardType.enumValueIndex == (int)CardType.Offensive ||
            cardType.enumValueIndex == (int)CardType.ThrowWeapon)
        {
            DrawField(damageType);
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight + 4; // Header
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cardID")) + 2;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("statType")) + 2;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("actionNeed")) + 2;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("cardType")) + 2;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("weaponID")) + 2;
        height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("haste")) + 2;

        if (property.FindPropertyRelative("statType").arraySize >= 2)
            height += EditorGUIUtility.singleLineHeight + 2;

        if (property.FindPropertyRelative("cardType").enumValueIndex == (int)CardType.Offensive ||
            property.FindPropertyRelative("cardType").enumValueIndex == (int)CardType.ThrowWeapon)
            height += EditorGUIUtility.singleLineHeight + 2;

        return height;
    }
}