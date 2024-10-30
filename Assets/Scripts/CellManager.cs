using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField] private Stage[] stages;

    [Header("Room Position Offsets")]
    public float offsetX = 0f; // �������� �� ��� X
    public float offsetY = -50f; // �������� �� ��� Y (�� ��������� ����)
    public float offsetZ = 0f; // �������� �� ��� Z

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
            // ������� ������� � ������ ������
            GameObject roomInstance = Instantiate(roomPrefab, freeCell.transform.position, Quaternion.identity);

            // �������� Transform ��� ������������� �������
            Transform roomTransform = roomInstance.transform;

            // ������������� ������� � ������ ��������
            roomTransform.position = freeCell.transform.position + new Vector3(offsetX, offsetY, offsetZ);

            // ������������ ������ closed ��� ������� ������
            if (freeCell.closed != null) // ������ ��� ��������� ����������
            {
                Debug.Log("Deactivating closed object: " + freeCell.closed.name);
                freeCell.closed.SetActive(false); // ������������ �������� ������
                Debug.Log("Closed object deactivated.");
            }
            else
            {
                Debug.LogWarning("Closed object is not assigned in the inspector.");
            }

            // �������� ������ ��� �������
            freeCell.SetEmpty(false);
        }
        else
        {
            Debug.LogWarning("No free cell available for building.");
        }
    }
}