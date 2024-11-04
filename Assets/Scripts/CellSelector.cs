using UnityEngine;

public class CellSelector : MonoBehaviour
{
    private Camera mainCamera;
    private Cell currentHighlightedCell; // Храним ссылку на текущую выделенную ячейку

    void Start()
    {
        mainCamera = Camera.main; // Получаем основную камеру
    }

    void Update()
    {
        // Проверяем, была ли нажата левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Создаем луч из позиции курсора
            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // Визуализируем луч

            if (Physics.Raycast(ray, out hit)) // Проверяем пересечение с объектами
            {
                Cell cell = hit.collider.GetComponent<Cell>(); // Получаем компонент Cell из объекта
                if (cell != null)
                {
                    // Проверяем, является ли ячейка пустой
                    if (cell.IsEmpty)
                    {
                        Debug.LogWarning("Cannot highlight an empty cell."); // Предупреждение о том, что ячейка пуста
                        return; // Выходим из метода, если ячейка пуста
                    }

                    if (currentHighlightedCell != null && currentHighlightedCell != cell) 
                    {
                        currentHighlightedCell.RemoveHighlight(); // Снимаем выделение с предыдущей ячейки
                    }

                    HighlightCell(cell); // Выделяем новую ячейку
                    currentHighlightedCell = cell; // Обновляем текущую выделенную ячейку
                }
                else
                {
                    Debug.LogWarning("Clicked object is not a cell."); // Отладочное сообщение
                }
            }
            else
            {
                Debug.LogWarning("Raycast did not hit any object."); // Отладочное сообщение
            }
        }
    }

    private void HighlightCell(Cell cell)
    {
        cell.HighlightCell(true); // Вызываем метод HighlightCell из класса Cell
    }
}