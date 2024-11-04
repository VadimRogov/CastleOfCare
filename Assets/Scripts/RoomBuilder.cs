using UnityEngine;

public class RoomBuilder : MonoBehaviour
{

    

    public AudioClip selectSound; // Аудиоклип для звука выбора
    private AudioSource audioSource; // Компонент AudioSource для воспроизведения звука

    private void Start() {
         audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем AudioSource к ячейке
        audioSource.clip = selectSound; // Устанавливаем аудиоклип
    }

    public void BuildRoomInCell(Cell cell, GameObject roomPrefab, string productTag) // Принимаем тег карточки товара как параметр
    {
        // Проверяем, что ячейка свободна
        if (cell != null && cell.IsEmpty)
        {
            // Проверяем, является ли ячейка подходящей для строительства лифта
            if (productTag == "Lift") // Проверяем, является ли продукт лифтом
            {
                // Проверяем тег ячейки
                if (cell.CompareTag("Lift")) // Предположим, что у ячейки есть тег "Lift"
                {
                    // Создание комнаты в позиции ячейки с учетом смещения по оси Y
                    Vector3 cellPosition = cell.transform.position; // Получаем позицию ячейки
                    Vector3 roomPosition = new Vector3(cellPosition.x, cellPosition.y - 3.6f, cellPosition.z); // Устанавливаем позиционирование комнаты

                    GameObject roomInstance = Instantiate(roomPrefab, roomPosition, Quaternion.identity); // Создаем комнату с новой позицией
                    roomInstance.transform.SetParent(cell.transform); // Устанавливаем родителя

                    Debug.Log("Building lift in cell: " + cell.name);
                    cell.SetCellEmpty(false, false); // Устанавливаем ячейку как занятую
                    cell.isEmpty = false;
                }
                else
                {
                    Debug.LogWarning("Lift can only be built in a cell with the 'Lift' tag.");
                }
            }
            else
            {
                // Логика для других типов комнат (если необходимо)
                Vector3 cellPosition = cell.transform.position; 
                Vector3 roomPosition = new Vector3(cellPosition.x, cellPosition.y - 3.6f, cellPosition.z); 

                GameObject roomInstance = Instantiate(roomPrefab, roomPosition, Quaternion.identity); 
                roomInstance.transform.SetParent(cell.transform); 

                Debug.Log("Building room in cell: " + cell.name);
                cell.SetCellEmpty(false, false);
                cell.isEmpty = false; 
            }
        }
        else
        {
            Debug.LogWarning("Cell is not empty or null.");
        }
    }

    public void DestroyRoomInCell(Cell cell)
    {
    if (cell != null && !cell.IsEmpty) // Проверяем, что ячейка не пустая
    {
        // Получаем объект комнаты, который является дочерним объектом ячейки
        Transform roomTransform = cell.transform.GetChild(0); // Предполагаем, что комната - это первый дочерний объект

        if (roomTransform != null)
        {
            // Уничтожаем объект комнаты
            Destroy(roomTransform.gameObject);
            Debug.Log("Room destroyed in cell: " + cell.name);

            // Обновляем состояние ячейки
            cell.SetCellEmpty(true, false); // Устанавливаем ячейку как пустую
            cell.isEmpty = true;
        }
        else
        {
            Debug.LogWarning("No room found in the cell: " + cell.name);
        }
    }
    else
    {
        Debug.LogWarning("Cell is empty or null.");
    }
    }
}