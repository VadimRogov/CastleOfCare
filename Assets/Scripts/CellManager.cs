using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField] private Stage[] stages; // Массив всех стадий

    public Stage[] Stages => stages; // Свойство для доступа к стадиям

    public Cell FindCellForFirsStages()
    {
        foreach (Stage stage in stages) // Перебираем все стадии для поиска свободной ячейки
        {
            foreach (Cell cell in stage.Cells) // Перебираем все ячейки в стадии
            {
                if (cell.IsEmpty) // Используем свойство IsEmpty вместо поля isEmpty
                {
                    return cell; // Возвращаем первую свободную ячейку
                }
            }
        }

        Debug.LogWarning("No free cell available for building.");
        return null; // Если свободных ячеек нет, возвращаем null
    }
}