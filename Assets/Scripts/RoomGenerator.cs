using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab; // Префаб комнаты
    public Transform castle; // Родительский объект для комнат
    public int castleWidth = 60;
    public int castleDepth = 15;
    public int castleHeight = 3;
    public int roomWidth = 12;
    public int roomDepth = 15;
    public int roomHeight = 7;

    private bool[,,] grid; // Сетка для отслеживания занятости мест

    void Start()
    {
        // Инициализация сетки
        grid = new bool[castleWidth, castleHeight, castleDepth];
    }

    public void CreateRoom()
    {
        // Поиск свободного места для комнаты
        for (int x = 0; x <= castleWidth - roomWidth; x += roomWidth)
        {
            for (int y = 0; y < castleHeight; y++)
            {
                for (int z = 0; z <= castleDepth - roomDepth; z += roomDepth)
                {
                    if (IsFreeSpace(x, y, z))
                    {
                        // Создание комнаты
                        GameObject room = Instantiate(roomPrefab, new Vector3(x, y * roomHeight, z), Quaternion.identity, castle);
                        room.transform.localScale = new Vector3(roomWidth, roomHeight, roomDepth);

                        // Заполнение сетки
                        for (int i = x; i < x + roomWidth; i++)
                        {
                            for (int j = y; j < y + 1; j++)
                            {
                                for (int k = z; k < z + roomDepth; k++)
                                {
                                    grid[i, j, k] = true;
                                }
                            }
                        }

                        return;
                    }
                }
            }
        }

        Debug.Log("Нет свободного места для комнаты.");
    }

    private bool IsFreeSpace(int x, int y, int z)
    {
        for (int i = x; i < x + roomWidth; i++)
        {
            for (int j = y; j < y + 1; j++)
            {
                for (int k = z; k < z + roomDepth; k++)
                {
                    if (grid[i, j, k])
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}