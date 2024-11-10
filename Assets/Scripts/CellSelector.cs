using UnityEngine;

public class CellSelector : MonoBehaviour
{
    private Camera mainCamera;
    private Cell currentHighlightedCell; // Храним ссылку на текущую выделенную ячейку
    private bool isCellSelected = false; // Флаг, указывающий на то, выделена ли ячейка

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
                    // Проверяем, имеет ли объект тег GridCell или Lift
                    if (hit.collider.CompareTag("GridCell") || hit.collider.CompareTag("Lift"))
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
                        isCellSelected = true; // Устанавливаем флаг выделения ячейки

                        // Сбрасываем выделение персонажа при выборе новой ячейки
                        CharacterSelector characterSelector = FindObjectOfType<CharacterSelector>();
                        if (characterSelector != null && characterSelector.IsCharacterSelected)
                        {
                            characterSelector.DeselectCharacter();
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Clicked object is not a valid cell (GridCell or Lift)."); // Отладочное сообщение
                    }
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

    public void HighlightCell(Cell cell)
    {
        cell.HighlightCell(true); // Вызываем метод HighlightCell из класса Cell
    }

    public void DeselectCell() 
    {
        if (currentHighlightedCell != null) 
        {
            currentHighlightedCell.RemoveHighlight();
            currentHighlightedCell = null;
            isCellSelected = false; // Сбрасываем флаг выделения ячейки
        }
    }
}