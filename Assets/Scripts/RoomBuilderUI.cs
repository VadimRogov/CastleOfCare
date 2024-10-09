using UnityEngine;
using UnityEngine.UI;

public class RoomBuilderUI : MonoBehaviour
{
    public RoomGridManager gridManager;
    public GameObject[] roomPrefabs; // Массив префабов комнат
    public Dropdown roomDropdown; // Выпадающий список для выбора комнаты
    public Dropdown locationDropdown; // Выпадающий список для выбора места
    public Button buildRoomButton; // Кнопка для постройки комнаты

    private int selectedRoomIndex;
    private int selectedLocationIndex;

    void Start()
    {
        // Инициализация выпадающих списков
        InitializeRoomDropdown();
        InitializeLocationDropdown();

        // Подписка на событие нажатия кнопки
        buildRoomButton.onClick.AddListener(BuildRoom);
    }

    void InitializeRoomDropdown()
    {
        roomDropdown.ClearOptions();
        foreach (GameObject roomPrefab in roomPrefabs)
        {
            roomDropdown.options.Add(new Dropdown.OptionData(roomPrefab.name));
        }
        roomDropdown.onValueChanged.AddListener(SetSelectedRoom);
    }

    void InitializeLocationDropdown()
    {
        locationDropdown.ClearOptions();
        for (int i = 0; i < gridManager.gridWidth * gridManager.gridHeight * gridManager.gridDepth; i++)
        {
            locationDropdown.options.Add(new Dropdown.OptionData($"Location {i}"));
        }
        locationDropdown.onValueChanged.AddListener(SetSelectedLocation);
    }

    void SetSelectedRoom(int index)
    {
        selectedRoomIndex = index;
    }

    void SetSelectedLocation(int index)
    {
        selectedLocationIndex = index;
    }

    void BuildRoom()
    {
        int x = selectedLocationIndex % gridManager.gridWidth;
        int y = (selectedLocationIndex / gridManager.gridWidth) % gridManager.gridHeight;
        int z = (selectedLocationIndex / (gridManager.gridWidth * gridManager.gridHeight)) % gridManager.gridDepth;

        GameObject cell = gridManager.GetGridCell(x, y, z);
        if (cell != null)
        {
            Vector3 position = cell.transform.position;
            Instantiate(roomPrefabs[selectedRoomIndex], position, Quaternion.identity);
        }
    }
}