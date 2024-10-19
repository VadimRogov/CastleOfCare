using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool isOccupied = false; // Занята ли ячейка

    // Метод для выделения ячейки
    public void HighlightCell(bool highlight)
    {
        if (highlight)
        {
            // Логика для выделения ячейки (например, изменение цвета или подсветка)
            // Например, включаем визуальный элемент выделения
            GetComponent<Renderer>().material.color = Color.green; // Замените на нужную вам логику
        }
        else
        {
            // Убираем выделение
            GetComponent<Renderer>().material.color = Color.white; // Вернуть к исходному состоянию
        }
    }
}
