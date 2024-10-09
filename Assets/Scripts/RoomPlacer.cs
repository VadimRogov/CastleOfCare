using UnityEngine;

public class RoomPlacer : MonoBehaviour
{
    public GameObject roomPrefab; // Префаб комнаты
    public RoomGridManager gridManager;

    void Start()
    {
        PlaceRooms();
    }

    void PlaceRooms()
    {
        // Размещение комнат на первом этаже
        PlaceRoom(0, 0, 0);
        PlaceRoom(1, 0, 0);
        PlaceRoom(2, 0, 0);
        PlaceRoom(3, 0, 0);
        PlaceRoom(4, 0, 0);

        // Размещение комнат на втором этаже
        PlaceRoom(0, 1, 0);
        PlaceRoom(1, 1, 0);
        PlaceRoom(2, 1, 0);
        PlaceRoom(3, 1, 0);
        PlaceRoom(4, 1, 0);

        // Размещение комнат на третьем этаже
        PlaceRoom(0, 2, 0);
        PlaceRoom(1, 2, 0);
        PlaceRoom(2, 2, 0);
        PlaceRoom(3, 2, 0);
    }

    void PlaceRoom(int x, int y, int z)
    {
        GameObject cell = gridManager.GetGridCell(x, y, z);
        if (cell != null)
        {
            Vector3 position = cell.transform.position;
            Instantiate(roomPrefab, position, Quaternion.identity);
        }
    }
}