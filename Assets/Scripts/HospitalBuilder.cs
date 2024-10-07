using UnityEngine;

public class HospitalBuilder : MonoBehaviour
{
    [Header("Настройки больницы")]
    public float hospitalWidth = 58.91f; // Ширина больницы
    public float hospitalDepth = 15.04f; // Глубина больницы
    public float wallThickness = 0.2f; // Толщина стен, пола и потолка

    public int numberOfFloors = 3; // Количество этажей
    public float floorHeight = 7f; // Высота этажа

    public float elevatorWidth = 10f; // Ширина лифта

    [ContextMenu("Build Hospital")]
    public void BuildHospital()
    {
        // Удаляем все предыдущие объекты
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Создаем каркас больницы
        BuildWalls();
        BuildFloors();
        BuildCeilings();

        // Размещаем комнаты
        PlaceRooms();
    }

    void BuildWalls()
    {
        // Создаем стены по периметру
        CreateWall(-hospitalWidth / 2f, 0f, 0f, 0f, numberOfFloors * floorHeight, hospitalDepth); // Левая стена
        CreateWall(hospitalWidth / 2f, 0f, 0f, 0f, numberOfFloors * floorHeight, hospitalDepth); // Правая стена
        CreateWall(0f, 0f, -hospitalDepth / 2f, hospitalWidth, numberOfFloors * floorHeight, 0f); // Передняя стена
        CreateWall(0f, 0f, hospitalDepth / 2f, hospitalWidth, numberOfFloors * floorHeight, 0f); // Задняя стена
    }

    void BuildFloors()
    {
        for (int i = 0; i < numberOfFloors; i++)
        {
            CreateFloor(0f, i * floorHeight, 0f, hospitalWidth, hospitalDepth);
        }
    }

    void BuildCeilings()
    {
        for (int i = 0; i < numberOfFloors; i++)
        {
            CreateCeiling(0f, (i + 1) * floorHeight, 0f, hospitalWidth, hospitalDepth);
        }
    }

    void PlaceRooms()
    {
        // Пример размещения комнат
        for (int i = 0; i < numberOfFloors; i++)
        {
            CreateRoom(0f, i * floorHeight, 0f);
        }
    }

    void CreateWall(float x, float y, float z, float width, float height, float depth)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.transform.localScale = new Vector3(width, height, depth);
        wall.transform.position = new Vector3(x, y, z);
        wall.transform.SetParent(transform);
        wall.name = "Wall";
    }

    void CreateFloor(float x, float y, float z, float width, float depth)
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.transform.localScale = new Vector3(width, wallThickness, depth);
        floor.transform.position = new Vector3(x, y, z);
        floor.transform.SetParent(transform);
        floor.name = "Floor";
    }

    void CreateCeiling(float x, float y, float z, float width, float depth)
    {
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.transform.localScale = new Vector3(width, wallThickness, depth);
        ceiling.transform.position = new Vector3(x, y, z);
        ceiling.transform.SetParent(transform);
        ceiling.name = "Ceiling";
    }

    void CreateRoom(float x, float y, float z)
    {
        GameObject room = GameObject.CreatePrimitive(PrimitiveType.Cube);
        room.transform.localScale = new Vector3(12f, 7f, 15f); // Размер комнаты
        room.transform.position = new Vector3(x, y + wallThickness, z);
        room.transform.SetParent(transform);
        room.name = "Room";
    }
}