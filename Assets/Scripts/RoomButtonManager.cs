using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class RoomButtonPrefab
{
    public Button roomButton; // Кнопка для создания комнаты
    public GameObject roomPrefab; // Префаб комнаты
}

public class RoomButtonManager : MonoBehaviour
{
    public List<RoomButtonPrefab> roomButtonPrefabs; // Список кнопок и их связанных префабов
    public Transform hospice; // Ссылка на объект Hospice
    public Cell[] cells; // Массив ячеек на этаже
    public GameObject shopPanel; // Ссылка на панель магазина (ShopPanel)

    void Start()
    {
        // Подписываемся на события нажатия на кнопки
        foreach (var buttonPrefab in roomButtonPrefabs)
        {
            if (buttonPrefab.roomButton != null && buttonPrefab.roomPrefab != null)
            {
                // Используем локальную переменную для правильного захвата префаба
                var prefab = buttonPrefab.roomPrefab;

                buttonPrefab.roomButton.onClick.AddListener(() => CreateRoom(prefab));
            }
        }
    }

    // Метод для создания комнаты
    private void CreateRoom(GameObject roomPrefab)
    {
        if (roomPrefab != null)
        {
            // Найдите первую свободную ячейку и выделите её
            Cell freeCell = FindFreeCell();
            if (freeCell != null)
            {
                // Выделить ячейку и убрать "Closed" стенку, если есть
                freeCell.HighlightCell(true);

                // Создаем комнату в ячейке
                Instantiate(roomPrefab, freeCell.transform.position, Quaternion.identity);

                // Устанавливаем ячейку как занятую
                freeCell.isOccupied = true;

                // Отключаем панель магазина
                if (shopPanel != null)
                {
                    shopPanel.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Нет свободных ячеек!");
            }
        }
        else
        {
            Debug.LogWarning("Префаб комнаты не назначен!");
        }
    }

    // Метод для поиска первой свободной ячейки
    private Cell FindFreeCell()
    {
        foreach (var cell in cells)
        {
            if (!cell.isOccupied)
            {
                return cell; // Возвращаем первую свободную ячейку
            }
        }
        return null; // Нет свободных ячеек
    }
}
