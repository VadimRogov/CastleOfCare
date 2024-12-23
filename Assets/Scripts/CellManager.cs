using UnityEngine;

public class CellManager : MonoBehaviour
{
    public Stage[] stages; // Массив всех стадий

    public Stage[] Stages => stages; // Свойство для доступа к стадиям

    public Cell FindCellForFirstStages() 
    {
        foreach (Stage stage in stages) 
        { 
            foreach (Cell cell in stage.Cells) 
            { 
                if (cell.IsEmpty) 
                { 
                    return cell; 
                } 
            } 
        }

        Debug.LogWarning("No free cell available for building.");
        return null; 
    }

    public Cell FindCellWithRoomByTag(string tag)
    {
        foreach (Stage stage in stages)
        {
            Cell cell = stage.FindCellWithRoomByTag(tag); // Используем метод Stage
            if (cell != null)
            {
                return cell; // Возвращаем первую найденную ячейку с указанным тегом комнаты
            }
        }

        Debug.LogWarning($"Ячейка с комнатой с тегом '{tag}' не найдена ни на одной стадии.");
        return null; // Возвращаем null, если не нашли ни на одной стадии
    }

    public Cell FindProcedureRoom(string roomTag)
    {
        foreach (Stage stage in Stages)
        {
            foreach (Cell cell in stage.Cells)
            {
                foreach (Transform room in cell.transform)
                {
                    if (room.CompareTag(roomTag)) 
                    {
                        foreach (Transform doctor in room.transform)
                        {
                            if(doctor.CompareTag(roomTag))
                            {
                                return cell;
                            }
                        }
                    }
                }
            }
        }
        return null;
    }

    public Cell FindRoomOnTag(string roomTag)
    {
        foreach (Stage stage in Stages)
        {
            foreach (Cell cell in stage.Cells)
            {
                foreach (Transform room in cell.transform)
                {
                    if (room.CompareTag(roomTag))
                    {
                        return cell;
                    }
                }
            }
        }
        return null;
    }
}
