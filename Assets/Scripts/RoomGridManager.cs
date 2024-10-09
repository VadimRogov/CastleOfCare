using UnityEngine;

public class RoomGridManager : MonoBehaviour
{
    public GameObject[] stages; // Массив этажей (Stage1, Stage2, Stage3)
    public GameObject gridCellPrefab; // Префаб ячейки сетки

    public int gridWidth = 5; // Ширина сетки (количество комнат на этаже)
    public int gridHeight = 3; // Высота сетки (количество этажей)
    public int gridDepth = 2; // Глубина сетки (количество комнат в глубину на этаже)

    private GameObject[,,] grid;

    void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        grid = new GameObject[gridWidth, gridHeight, gridDepth];

        for (int y = 0; y < gridHeight; y++)
        {
            Transform stage = stages[y].transform;

            for (int x = 0; x < gridWidth; x++)
            {
                for (int z = 0; z < gridDepth; z++)
                {
                    Vector3 position = new Vector3(x * 15, y * 7.4f, z * 12); // Размеры комнаты 15x7.4x12
                    GameObject cell = Instantiate(gridCellPrefab, position, Quaternion.identity);
                    cell.transform.parent = stage;
                    grid[x, y, z] = cell;
                }
            }
        }
    }

    public GameObject GetGridCell(int x, int y, int z)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight && z >= 0 && z < gridDepth)
        {
            return grid[x, y, z];
        }
        return null;
    }
}