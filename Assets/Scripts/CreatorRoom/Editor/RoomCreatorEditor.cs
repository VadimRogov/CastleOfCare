using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomCreator))]
public class RoomCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RoomCreator roomCreator = (RoomCreator)target;

        // Draw custom inspector with Russian labels
        EditorGUILayout.LabelField("Настройки комнаты", EditorStyles.boldLabel);
        roomCreator.roomName = EditorGUILayout.TextField("Название комнаты", roomCreator.roomName);
        roomCreator.roomWidth = EditorGUILayout.FloatField("Ширина комнаты", roomCreator.roomWidth);
        roomCreator.roomHeight = EditorGUILayout.FloatField("Высота комнаты", roomCreator.roomHeight);
        roomCreator.roomDepth = EditorGUILayout.FloatField("Глубина комнаты", roomCreator.roomDepth);
        roomCreator.wallThickness = EditorGUILayout.FloatField("Толщина стен", roomCreator.wallThickness);
        roomCreator.floorThickness = EditorGUILayout.FloatField("Толщина пола", roomCreator.floorThickness);
        roomCreator.ceilingThickness = EditorGUILayout.FloatField("Толщина потолка", roomCreator.ceilingThickness);

        EditorGUILayout.LabelField("Материалы", EditorStyles.boldLabel);
        roomCreator.backWallMaterial = (Material)EditorGUILayout.ObjectField("Материал задней стены", roomCreator.backWallMaterial, typeof(Material), false);
        roomCreator.rightWallMaterial = (Material)EditorGUILayout.ObjectField("Материал правой стены", roomCreator.rightWallMaterial, typeof(Material), false);
        roomCreator.leftWallMaterial = (Material)EditorGUILayout.ObjectField("Материал левой стены", roomCreator.leftWallMaterial, typeof(Material), false);
        roomCreator.floorMaterial = (Material)EditorGUILayout.ObjectField("Материал пола", roomCreator.floorMaterial, typeof(Material), false);
        roomCreator.ceilingMaterial = (Material)EditorGUILayout.ObjectField("Материал потолка", roomCreator.ceilingMaterial, typeof(Material), false);

        // Add a button to create the room
        if (GUILayout.Button("Создать комнату"))
        {
            roomCreator.CreateRoom();
        }
    }
}