using UnityEngine;

public class CellManager : MonoBehaviour
{
    public GameObject hospice; // Ссылка на объект Hospice

    // Метод для выделения первой свободной ячейки
    public Cell HighlightFirstFreeCell()
    {
        Debug.Log("HighlightFirstFreeCell method called");

        // Получаем все ячейки в Hospice
        Cell[] cells = hospice.GetComponentsInChildren<Cell>();

        Debug.Log("Found " + cells.Length + " cells in Hospice");

        // Сначала ищем первую свободную ячейку на первом этаже
        Cell freeCell = FindFirstFreeCellOnFloor(cells, "Stage1");

        if (freeCell == null)
        {
            // Если все ячейки на первом этаже заняты, ищем на втором этаже
            freeCell = FindFirstFreeCellOnFloor(cells, "Stage2");
        }

        if (freeCell == null)
        {
            // Если все ячейки на первых двух этажах заняты, ищем на третьем этаже
            freeCell = FindFirstFreeCellOnFloor(cells, "Stage3");
        }

        if (freeCell != null)
        {
            Debug.Log("Found free cell: " + freeCell.name);

            // Активируем объект Open и деактивируем объект Closed только на первой свободной ячейке
            freeCell.Open.SetActive(true);
            freeCell.Closed.SetActive(false);
        }
        else
        {
            Debug.Log("No free cells found in Hospice");
        }

        return freeCell;
    }

    // Метод для поиска первой свободной ячейки на указанном этаже
    private Cell FindFirstFreeCellOnFloor(Cell[] cells, string floorName)
    {
        foreach (Cell cell in cells)
        {
            if (cell.isEmpty && cell.transform.parent.name == floorName)
            {
                return cell;
            }
        }
        return null;
    }
}
