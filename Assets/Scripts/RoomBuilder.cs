using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    // ћетод дл€ создани€ комнаты в выбранной €чейке
    public void CreateRoomInCell(Cell cell, GameObject roomPrefab)
    {
        if (cell.isEmpty && roomPrefab != null)
        {
            // —оздаем экземпл€р комнаты в выбранной €чейке
            GameObject roomInstance = Instantiate(roomPrefab, cell.transform.position, Quaternion.identity);
            roomInstance.transform.SetParent(cell.transform, false);
            cell.isEmpty = true;
            cell.UpdateCellState();
        }
        else
        {
            Debug.LogWarning("Room prefab is null or cell is not empty.");
        }
    }
}