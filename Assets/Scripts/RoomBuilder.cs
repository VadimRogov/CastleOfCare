using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    // Метод для создания комнаты в ячейке
    public void CreateRoomInCell(Cell cell, GameObject roomPrefab)
    {
        if (cell.isEmpty && roomPrefab != null)
        {
            // Создаем экземпляр комнаты в ячейке
            GameObject roomInstance = Instantiate(roomPrefab, cell.transform.position, Quaternion.identity);
            roomInstance.transform.SetParent(cell.transform, false);

            // После создания комнаты отмечаем ячейку как занятую
            cell.isEmpty = false;
            cell.UpdateCellState();
        }
        else
        {
            Debug.LogWarning("Room prefab is null or cell is not empty.");
        }
    }
}
