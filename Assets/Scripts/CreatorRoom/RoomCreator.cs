using UnityEngine;

public class RoomCreator : MonoBehaviour
{
    [Header("Настройки комнаты")]
    public string roomName = "Новая комната";
    public float roomWidth = 10f;
    public float roomHeight = 5f;
    public float roomDepth = 10f;
    public float wallThickness = 0.1f;
    public float floorThickness = 0.1f;
    public float ceilingThickness = 0.1f;

    [Header("Материалы")]
    public Material backWallMaterial;
    public Material rightWallMaterial;
    public Material leftWallMaterial;
    public Material floorMaterial;
    public Material ceilingMaterial;

    public void CreateRoom()
    {
        // Create an empty GameObject to hold the room
        GameObject roomHolder = new GameObject(roomName);

        // Create walls, floor, and ceiling
        GameObject backWall = CreateWall(roomHolder.transform, new Vector3(0, roomHeight / 2, -roomDepth / 2 + wallThickness / 2), new Vector3(roomWidth, roomHeight, wallThickness), backWallMaterial, "BackWall"); // Back wall
        GameObject rightWall = CreateWall(roomHolder.transform, new Vector3(roomWidth / 2 - wallThickness / 2, roomHeight / 2, 0), new Vector3(wallThickness, roomHeight, roomDepth), rightWallMaterial, "RightWall"); // Right wall
        GameObject leftWall = CreateWall(roomHolder.transform, new Vector3(-roomWidth / 2 + wallThickness / 2, roomHeight / 2, 0), new Vector3(wallThickness, roomHeight, roomDepth), leftWallMaterial, "LeftWall"); // Left wall
        CreateFloor(roomHolder.transform, new Vector3(0, -floorThickness / 2, 0), new Vector3(roomWidth, floorThickness, roomDepth), floorMaterial); // Floor
        CreateCeiling(roomHolder.transform, new Vector3(0, roomHeight + ceilingThickness / 2, 0), new Vector3(roomWidth, ceilingThickness, roomDepth), ceilingMaterial); // Ceiling

        // Add RoomScaler component to the room holder
        roomHolder.AddComponent<RoomScaler>();
    }

    GameObject CreateWall(Transform parent, Vector3 position, Vector3 scale, Material material, string wallName)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = wallName;
        wall.transform.SetParent(parent);
        wall.transform.position = position;
        wall.transform.localScale = scale;
        if (material != null)
        {
            wall.GetComponent<Renderer>().material = material;
        }
        return wall;
    }

    void CreateFloor(Transform parent, Vector3 position, Vector3 scale, Material material)
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.SetParent(parent);
        floor.transform.position = position;
        floor.transform.localScale = scale;
        if (material != null)
        {
            floor.GetComponent<Renderer>().material = material;
        }
    }

    void CreateCeiling(Transform parent, Vector3 position, Vector3 scale, Material material)
    {
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.name = "Ceiling";
        ceiling.transform.SetParent(parent);
        ceiling.transform.position = position;
        ceiling.transform.localScale = scale;
        if (material != null)
        {
            ceiling.GetComponent<Renderer>().material = material;
        }
    }
}