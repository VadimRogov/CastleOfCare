using UnityEngine;

public class RoomBuilder : MonoBehaviour
{
    // ����� ��� �������� ������� � ��������� ������
    public void CreateRoomInCell(Cell cell, GameObject roomPrefab)
    {
        if (cell.isEmpty && roomPrefab != null)
        {
            // ������� ��������� ������� � ��������� ������
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