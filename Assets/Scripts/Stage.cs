using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField] private Cell[] cells;

    public Cell[] Cells
    {
        get { return cells; }
        private set { cells = value; }
    }

    public Cell FindFirstCell()
    {
        foreach(Cell cell in cells)
        {
            if(cell.IsEmpty) {
                cell.SetCondition();
                return cell;
            }
        }
        return null;
    }
}
