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
        // Перебираем все ячейки
        for (int i = 0; i < listLift.Length - 1; i++)
        {
            Cell currentCell = listLift[i];
            Cell nextCell = listLift[i + 1];

            // Проверяем, есть ли лифт в текущей и следующей ячейках
            bool currentLiftExists = HasLift(currentCell);
            bool nextLiftExists = HasLift(nextCell);

            // Если есть лифт в текущей и следующей ячейках, можно перемещаться
            if (currentLiftExists && nextLiftExists)
            {
                // Логика перемещения на следующий этаж
                Debug.Log($"Лифт может двигаться с этажа {i + 1} на этаж {i + 2}");
                // Здесь можно добавить код для фактического перемещения лифта
            }
        }
    }

    // Метод для проверки наличия лифта в ячейке
    private bool HasLift(Cell cell)
    {
        for (int j = 0; j < cell.transform.childCount; j++)
        {
            Transform room = cell.transform.GetChild(j);
            if (room.CompareTag("Lift"))
            {
                return true; // Лифт найден
            }
        }
        return false; // Лифт не найден
    }
}