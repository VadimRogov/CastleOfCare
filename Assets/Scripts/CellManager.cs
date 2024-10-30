using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField] private Stage[] stages;

    [Header("Room Position Offsets")]
    public float offsetX = 0f; // Смещение по оси X
    public float offsetY = -50f; // Смещение по оси Y (по умолчанию вниз)
    public float offsetZ = 0f; // Смещение по оси Z

    public Stage[] Stages
    {
        get { return stages; }
        private set { stages = value; }
    }

    public Cell FindFirstStage()
    {
        foreach (Stage stage in stages)
        {
            Cell firstCellEmpty = stage.FindFirstCell();
            if (firstCellEmpty != null)
            {
                return firstCellEmpty;
            }
        }
        return null;
    }

    public void CreateRoomInEmptyCell(GameObject roomPrefab)
    {
        Cell freeCell = FindFirstStage();

        if (freeCell != null)
        {
            // Создаем комнату в пустой ячейке
            GameObject roomInstance = Instantiate(roomPrefab, freeCell.transform.position, Quaternion.identity);

            // Получаем Transform для корректировки позиции
            Transform roomTransform = roomInstance.transform;

            // Устанавливаем позицию с учетом смещения
            roomTransform.position = freeCell.transform.position + new Vector3(offsetX, offsetY, offsetZ);

            // Деактивируем объект closed для текущей ячейки
            if (freeCell.closed != null) // Теперь это публичная переменная
            {
                Debug.Log("Deactivating closed object: " + freeCell.closed.name);
                freeCell.closed.SetActive(false); // Деактивируем закрытый объект
                Debug.Log("Closed object deactivated.");
            }
            else
            {
                Debug.LogWarning("Closed object is not assigned in the inspector.");
            }

            // Отмечаем ячейку как занятую
            freeCell.SetEmpty(false);
        }
        else
        {
            Debug.LogWarning("No free cell available for building.");
        }
    }
}