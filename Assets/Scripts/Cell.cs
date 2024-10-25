using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public GameObject Closed; // Объект, который показывается, когда ячейка занята
    public GameObject Open;   // Объект, который показывается, когда ячейка свободна
    public bool isEmpty = true; // Изначально ячейка свободна

    void Start()
    {
        UpdateCellState();

        // Добавляем обработчик события нажатия на кнопку Open
        Button openButton = Open.GetComponent<Button>();
        if (openButton != null)
        {
            openButton.onClick.AddListener(OnOpenButtonClicked);
        }
        else
        {
            Debug.LogError("Button component not found on Open object in cell: " + gameObject.name);
        }
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

    // Метод для сброса состояния ячейки
    public void ResetCellState()
    {
        isEmpty = true; // Ячейка становится пустой
        UpdateCellState();
    }

    // Обработчик события нажатия на кнопку Open
    private void OnOpenButtonClicked()
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
        RoomBuilder roomBuilder = FindObjectOfType<RoomBuilder>();
        if (roomBuilder != null)
        {
            // Загружаем префаб комнаты из ресурсов
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
