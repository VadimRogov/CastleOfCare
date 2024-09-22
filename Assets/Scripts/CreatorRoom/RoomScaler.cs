using UnityEngine;
using System.Collections.Generic;

public class RoomScaler : MonoBehaviour
{
    [Header("Масштабирование комнаты")]
    public float scaleWidth = 1f;
    public float scaleHeight = 1f;
    public float scaleDepth = 1f;

    [Header("Масштабирование толщины")]
    public float scaleWallThickness = 1f;
    public float scaleFloorThickness = 1f;
    public float scaleCeilingThickness = 1f;

    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();

    void OnValidate()
    {
        // Проверяем, что значения не равны нулю перед применением масштабирования
        if (scaleWidth != 0f && scaleHeight != 0f && scaleDepth != 0f &&
            scaleWallThickness != 0f && scaleFloorThickness != 0f && scaleCeilingThickness != 0f)
        {
            ApplyScaling();
        }
    }

    public void ApplyScaling()
    {
        // Получаем все дочерние объекты (стены, пол, потолок)
        foreach (Transform child in transform)
        {
            // Если исходные значения еще не сохранены, сохраняем их
            if (!originalScales.ContainsKey(child))
            {
                originalScales[child] = child.localScale;
                originalPositions[child] = child.localPosition;
            }

            // Масштабируем каждый дочерний объект
            Vector3 originalScale = originalScales[child];
            Vector3 newScale = originalScale;

            // Применяем масштабирование размеров комнаты
            newScale.x *= scaleWidth;
            newScale.y *= scaleHeight;
            newScale.z *= scaleDepth;

            // Применяем масштабирование толщины
            if (child.name.Contains("Wall"))
            {
                newScale.z *= scaleWallThickness;
            }
            else if (child.name.Contains("Floor"))
            {
                newScale.y *= scaleFloorThickness;
            }
            else if (child.name.Contains("Ceiling"))
            {
                newScale.y *= scaleCeilingThickness;
            }

            child.localScale = newScale;

            // Корректируем позицию, чтобы сохранить правильное расположение
            Vector3 originalPosition = originalPositions[child];
            Vector3 newPosition = originalPosition;

            // Корректировка позиции для стен
            if (child.name.Contains("Wall"))
            {
                newPosition.z = originalPosition.z * scaleDepth;
                if (child.name.Contains("Front"))
                {
                    newPosition.z += (newScale.z - originalScale.z) / 2;
                }
                else if (child.name.Contains("Back"))
                {
                    newPosition.z -= (newScale.z - originalScale.z) / 2;
                }
            }

            // Корректировка позиции для пола и потолка
            if (child.name.Contains("Floor") || child.name.Contains("Ceiling"))
            {
                newPosition.y = originalPosition.y * scaleHeight;
                if (child.name.Contains("Floor"))
                {
                    newPosition.y -= (newScale.y - originalScale.y) / 2;
                }
                else if (child.name.Contains("Ceiling"))
                {
                    newPosition.y += (newScale.y - originalScale.y) / 2;
                }
            }

            child.localPosition = newPosition;
        }
    }
}