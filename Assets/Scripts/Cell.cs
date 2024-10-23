using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject Closed; // Объект Closed
    public GameObject Open;   // Объект Open
    public bool isEmpty = false; // Поле для определения, свободна ли ячейка

    void Start()
    {
        UpdateCellState();
    }

    // Метод для обновления состояния ячейки
    public void UpdateCellState()
    {
        if (Closed == null || Open == null)
        {
            Debug.LogError("Closed or Open object is not assigned in the inspector for cell: " + gameObject.name);
            return;
        }

        Closed.SetActive(!isEmpty);
        Open.SetActive(isEmpty);
    }

    // Обработчик события нажатия на ячейку
    private void OnMouseDown()
    {
        if (isEmpty)
        {
            // Вызываем метод для создания комнаты в этой ячейке
            CreateRoomInCell();
        }
    }

    // Метод для создания комнаты в ячейке
    private void CreateRoomInCell()
    {
        // Находим скрипт RoomBuilder и вызываем метод для создания комнаты
        RoomBuilder roomBuilder = FindObjectOfType<RoomBuilder>();
        if (roomBuilder != null)
        {
            // Предположим, что у вас есть префаб комнаты, который вы хотите использовать
            GameObject roomPrefab = Resources.Load<GameObject>("Prefabs/RoomPrefab");
            if (roomPrefab != null)
            {
                roomBuilder.CreateRoomInCell(this, roomPrefab);
            }
            else
            {
                Debug.LogError("Room prefab not found in Resources folder.");
            }
        }
        else
        {
            Debug.LogError("RoomBuilder not found in the scene.");
        }
    }
}