using UnityEngine;

public class RoomConstructionManager : MonoBehaviour
{
    private GameObject selectedRoomPrefab; // Текущий выбранный для строительства префаб

    // Вызывается при выборе префаба комнаты для строительства
    public void SelectRoomToBuild(GameObject roomPrefab)
    {
        selectedRoomPrefab = roomPrefab; // Запоминаем выбранный префаб
        Debug.Log("Выбрана комната: " + roomPrefab.name);
    }

    // Метод для установки комнаты в сцене (например, по клику на ячейку)
    public void BuildRoom(Vector3 position)
    {
        if (selectedRoomPrefab != null)
        {
            // Строим комнату в заданной позиции
            Instantiate(selectedRoomPrefab, position, Quaternion.identity);
            Debug.Log("Комната построена: " + selectedRoomPrefab.name);
        }
    }
}
