using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Cell[] cells; // Массив всех ячеек

    public Cell[] Cells
    {
        get { return cells; }
        private set { cells = value; }
    }

    public Cell FindFirstCell()
    {
        foreach (Cell cell in cells)
        {
            Debug.Log($"Checking cell: {cell.name}, IsEmpty: {cell.IsEmpty}"); // Отладочное сообщение
            if (cell.IsEmpty) // Используем свойство IsEmpty
            {
                cell.SetCellEmpty(false, true);
                return cell;
            }
        }
        return null;
    }
}