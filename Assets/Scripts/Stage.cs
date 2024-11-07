 using UnityEngine;

public class Stage : MonoBehaviour
{
   [SerializeField] private Cell[] cells; // Массив всех ячеек

   public Cell[] Cells => cells;

   public Cell FindFirstCell()
   {
       foreach (Cell cell in cells)
       {
           Debug.Log($"Checking cell: {cell.name}, IsEmpty: {cell.IsEmpty}"); 
           if (cell.IsEmpty) 
           { 
               cell.SetCellEmpty(false, true);
               return cell;
           }
       }
       
      Debug.LogWarning("No empty cells available.");
      return null;
   }

   public Cell FindCellWithRoomByTag(string tag)
    {
    foreach (Cell cell in cells)
    {
        Transform room = cell.FindRoomInCellByTag(tag); // Используем новый метод из Cell
        if (room != null) // Если комната найдена
        {
            return cell; // Возвращаем ячейку, содержащую комнату
        }
    }

    Debug.LogWarning($"Ячейка с комнатой с тегом '{tag}' не найдена.");
    return null; // Возвращаем null, если не нашли такую ячейку
    }

}