using UnityEngine;
using System.Collections.Generic;

public class CharacterSelector : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject currentSelectedCharacter; // Храним ссылку на текущего выбранного персонажа
    private bool isCharacterSelected = false; // Флаг, указывающий на то, выбран ли персонаж
    private Dictionary<SpriteRenderer, Color> originalColors = new Dictionary<SpriteRenderer, Color>(); // Словарь для хранения исходных цветов

    public bool IsCharacterSelected => isCharacterSelected; // Свойство для доступа к флагу выбора персонажа

    void Start()
    {
        mainCamera = Camera.main; // Получаем основную камеру
    }

    void Update()
    {
        // Проверяем, была ли нажата правая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // Создаем луч из позиции курсора
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f); // Визуализируем луч
            Debug.Log("Ray created from mouse position.");

            RaycastHit[] hits = Physics.RaycastAll(ray); // Получаем все объекты, с которыми пересекается луч
            GameObject clickedCharacter = null;

            foreach (RaycastHit hit in hits)
            {
                GameObject clickedObject = hit.collider.gameObject; // Получаем объект под курсором

                // Проверяем, является ли объект персонажем
                if (clickedObject.CompareTag("Character") || 
                    clickedObject.CompareTag("Psych") || 
                    clickedObject.CompareTag("LFK")) 
                {
                    clickedCharacter = clickedObject;
                    break; // Выходим из цикла, если нашли персонажа
                }
            }

            if (clickedCharacter != null)
            {
                if (currentSelectedCharacter != null)
                {
                    DeselectCharacter(currentSelectedCharacter);
                }

                SelectCharacter(clickedCharacter);
                currentSelectedCharacter = clickedCharacter; // Обновляем текущего выбранного персонажа
                isCharacterSelected = true; // Устанавливаем флаг выбора персонажа

                // Сбрасываем выделение ячейки при выборе персонажа
                FindObjectOfType<CellSelector>().DeselectCell();
            }
            else
            {
                // Если не нашли персонажа, проверяем ячейки
                foreach (RaycastHit hit in hits)
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

                            if (currentSelectedCharacter != null)
                            {
                                DeselectCharacter(currentSelectedCharacter); // Сбрасываем выбор персонажа
                            }

                            FindObjectOfType<CellSelector>().HighlightCell(cell); // Выделяем ячейку
                            isCharacterSelected = false; // Сбрасываем флаг выбора персонажа
                            break; // Выходим из цикла, если нашли ячейку
                        }
                    }
                }
            }
        }
    }

    private void SelectCharacter(GameObject character)
    {
        SpriteRenderer[] spriteRenderers = character.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null)
            {
                originalColors[spriteRenderer] = spriteRenderer.color; // Сохраняем исходный цвет
                spriteRenderer.color = Color.yellow; // Изменяем цвет на желтый при выделении
            }
            else
            {
                Debug.LogWarning($"Character {character.name} does not have a SpriteRenderer component."); // Предупреждение о том, что компонент отсутствует
            }
        }

        Debug.Log($"Character {character.name} selected.");
    }

    public void DeselectCharacter()
    {
        if (currentSelectedCharacter != null)
        {
            DeselectCharacter(currentSelectedCharacter);
        }
    }

    private void DeselectCharacter(GameObject character)
    {
        if (character == null)
        {
            Debug.LogWarning("Attempted to deselect a null character."); // Предупреждение о том, что объект равен null
            return; // Выходим из метода
        }

        SpriteRenderer[] spriteRenderers = character.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            if (spriteRenderer != null && originalColors.ContainsKey(spriteRenderer))
            {
                spriteRenderer.color = originalColors[spriteRenderer]; // Восстанавливаем исходный цвет
                originalColors.Remove(spriteRenderer); // Удаляем сохраненный цвет из словаря
            }
            else
            {
                Debug.LogWarning($"Character {character.name} does not have a SpriteRenderer component."); // Предупреждение о том, что компонент отсутствует
            }
        }

        Debug.Log($"Character {character.name} deselected.");
        isCharacterSelected = false; // Сбрасываем флаг выбора персонажа после снятия выделения
    }
}