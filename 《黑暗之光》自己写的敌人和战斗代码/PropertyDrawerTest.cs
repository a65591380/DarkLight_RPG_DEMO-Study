using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class RangeAttribute : PropertyAttribute
{

    public float min;
    public float max;
    public string label;

    public RangeAttribute(float min, float max = 0, string label = "")
    {
        this.min = 0;
        switch (label)
        {
            case "MP":
                this.max = PlayerStatus._instance.mp_max;
                break;
            case "HP":
                this.max = 0;
                break;
            case "EXP":
                this.max = 0;
                break;
            default:
                this.max = max;
                break;
        }
        this.label = label;
    }
}


[CustomPropertyDrawer(typeof(RangeAttribute))]
public class RangeDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // First get the attribute since it contains the range for the slider
        RangeAttribute range =  attribute as RangeAttribute;

        // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
        if (property.propertyType == SerializedPropertyType.Float)
            EditorGUI.Slider(position, property, range.min, range.max, range.label);
        else if (property.propertyType == SerializedPropertyType.Integer)
            EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, range.label);
        else
            EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
    }
}