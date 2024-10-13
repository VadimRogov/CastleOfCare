using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoomSelectionUI : MonoBehaviour
{
    public GameObject roomButtonPrefab;  // Префаб кнопки для комнаты
    public Transform roomButtonContainer;  // Контейнер для кнопок (например, внутри ScrollRect)
    public List<GameObject> roomPrefabs;  // Список префабов комнат, которые можно строить
    public RoomConstructionManager roomConstructionManager;  // Ссылка на скрипт строительства

    void Start()
    {
        // Создаем кнопки для каждой комнаты
        foreach (GameObject roomPrefab in roomPrefabs)
        {
            GameObject roomButton = Instantiate(roomButtonPrefab, roomButtonContainer);
            roomButton.GetComponentInChildren<Text>().text = roomPrefab.name;  // Название комнаты на кнопке

            // Добавляем обработчик события для кнопки
            roomButton.GetComponent<Button>().onClick.AddListener(() => SelectRoom(roomPrefab));
        }
    }

    // Выбор комнаты
    public void SelectRoom(GameObject roomPrefab)
    {
        roomConstructionManager.SelectRoomToBuild(roomPrefab);
    }
}
