using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomScaler))]
public class RoomScalerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomScaler roomScaler = (RoomScaler)target;

        // Используем GUIContent для отображения надписей на русском языке
        GUIContent scaleWidthLabel = new GUIContent("Ширина комнаты");
        GUIContent scaleHeightLabel = new GUIContent("Высота комнаты");
        GUIContent scaleDepthLabel = new GUIContent("Глубина комнаты");
        GUIContent scaleWallThicknessLabel = new GUIContent("Толщина стен");
        GUIContent scaleFloorThicknessLabel = new GUIContent("Толщина пола");
        GUIContent scaleCeilingThicknessLabel = new GUIContent("Толщина потолка");

        // Отображаем свойства с русскими метками
        roomScaler.scaleWidth = EditorGUILayout.FloatField(scaleWidthLabel, roomScaler.scaleWidth);
        roomScaler.scaleHeight = EditorGUILayout.FloatField(scaleHeightLabel, roomScaler.scaleHeight);
        roomScaler.scaleDepth = EditorGUILayout.FloatField(scaleDepthLabel, roomScaler.scaleDepth);
        roomScaler.scaleWallThickness = EditorGUILayout.FloatField(scaleWallThicknessLabel, roomScaler.scaleWallThickness);
        roomScaler.scaleFloorThickness = EditorGUILayout.FloatField(scaleFloorThicknessLabel, roomScaler.scaleFloorThickness);
        roomScaler.scaleCeilingThickness = EditorGUILayout.FloatField(scaleCeilingThicknessLabel, roomScaler.scaleCeilingThickness);

        // Применяем изменения
        if (GUI.changed)
        {
            EditorUtility.SetDirty(roomScaler);
            roomScaler.ApplyScaling(); // Применяем масштабирование прямо из редактора
        }
    }
}