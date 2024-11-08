using UnityEngine;

public class MoveLift : MonoBehaviour
{
    public bool isCabin = false;
    [SerializeField] private Cell[] listLift;
    public GameObject cellWithCabin;

    public bool FindCabineLift()
    {
        foreach (Cell cell in listLift)
        {
            foreach (Transform room in cell.transform)
            {
                if (room.CompareTag("Lift"))
                {
                    GameObject roomCell = room.gameObject;
                    foreach (Transform cabin in roomCell.transform)
                    {
                        if (cabin.CompareTag("Cabin"))
                            {
                                cellWithCabin = cabin.gameObject;
                                isCabin = true;
                                return isCabin;
                        }
                    }
                    
                }
            }
        }
        return isCabin;
    }

    public void MoveCabin()
    {
        // ���������� ��� ������
        for (int i = 0; i < listLift.Length - 1; i++)
        {
            Cell currentCell = listLift[i];
            Cell nextCell = listLift[i + 1];

            // ���������, ���� �� ���� � ������� � ��������� �������
            bool currentLiftExists = HasLift(currentCell);
            bool nextLiftExists = HasLift(nextCell);

            // ���� ���� ���� � ������� � ��������� �������, ����� ������������
            if (currentLiftExists && nextLiftExists)
            {
                // ������ ����������� �� ��������� ����
                Debug.Log($"���� ����� ��������� � ����� {i + 1} �� ���� {i + 2}");
                // ����� ����� �������� ��� ��� ������������ ����������� �����
            }
        }
    }

    // ����� ��� �������� ������� ����� � ������
    private bool HasLift(Cell cell)
    {
        for (int j = 0; j < cell.transform.childCount; j++)
        {
            Transform room = cell.transform.GetChild(j);
            if (room.CompareTag("Lift"))
            {
                return true; // ���� ������
            }
        }
        return false; // ���� �� ������
    }
}