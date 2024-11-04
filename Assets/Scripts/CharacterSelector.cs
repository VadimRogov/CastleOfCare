using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private Camera mainCamera;
    private GameObject currentSelectedCharacter; // Храним ссылку на текущего выбранного персонажа

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

            if (Physics.Raycast(ray, out hit)) // Проверяем пересечение с объектами
            {
                GameObject character = hit.collider.gameObject; // Получаем объект персонажа
                if (character.CompareTag("Character")) // Проверяем, имеет ли объект тег "Character"
                {
                    // Снимаем выделение с предыдущего персонажа
                    if (currentSelectedCharacter != null && currentSelectedCharacter != character)
                    {
                        DeselectCharacter(currentSelectedCharacter);
                    }

                    // Выделяем нового персонажа
                    SelectCharacter(character);
                    currentSelectedCharacter = character; // Обновляем текущего выбранного персонажа
                }
                else
                {
                    Debug.LogWarning("Clicked object is not a character."); // Отладочное сообщение
                }
            }
            else
            {
                Debug.LogWarning("Raycast did not hit any object."); // Отладочное сообщение
            }
        }
    }

    private void SelectCharacter(GameObject character)
    {
        // Здесь можно добавить визуальные эффекты для выделения персонажа
        Renderer renderer = character.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow; // Изменяем цвет на желтый при выделении
        }

        Debug.Log($"Character {character.name} selected.");
    }

    private void DeselectCharacter(GameObject character)
    {
        // Сбрасываем визуальные эффекты выделения
        Renderer renderer = character.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.white; // Возвращаем цвет к белому при снятии выделения
        }

        Debug.Log($"Character {character.name} deselected.");
    }
}