using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    public bool isEmpty = true; // Состояние ячейки
    public GameObject closed; // Объект, представляющий закрытую ячейку
    public GameObject buildButton; // Кнопка для постройки
    public GameObject highlightPrefab; // Префаб для выделения
    
    public AudioClip selectSound; // Аудиоклип для звука выбора
    private AudioSource audioSource; // Компонент AudioSource для воспроизведения звука

    private GameObject highlightObject; // Объект для выделения

    public bool IsEmpty
    {
        get { return isEmpty; }
        private set { isEmpty = value; }
    }

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем AudioSource к ячейке
        audioSource.clip = selectSound; // Устанавливаем аудиоклип
        
        if (buildButton != null)
        {
            Button buttonComponent = buildButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.AddListener(OnBuildButtonClicked);
            }
        }

        // Создаем объект выделения и делаем его дочерним
        if (highlightPrefab != null)
        {
            highlightObject = Instantiate(highlightPrefab, transform); // Создаем объект выделения как дочерний
            highlightObject.SetActive(false); // Скрываем его по умолчанию

            // Устанавливаем позицию объекта выделения с учетом смещения
            Vector3 cellPosition = transform.position; // Получаем позицию ячейки
            highlightObject.transform.position = new Vector3(cellPosition.x, cellPosition.y + 0.01f, cellPosition.z - 7.6f); // Смещение по Z на 10 единиц и немного по Y для видимости
            
            // Устанавливаем масштаб (например, плоский)
            highlightObject.transform.localScale = new Vector3(1f, 0.01f, 1f); 
            
            // Устанавливаем правильный поворот
            highlightObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Поворачиваем на 90 градусов по оси X
        }
    }

    public void OnBuildButtonClicked()
    {
        RoomBuilder roomBuilder = FindObjectOfType<RoomBuilder>();
        if (roomBuilder != null)
        {
            ProductCard selectedProductCard = FindObjectOfType<ShopPanelControll>().GetSelectedProductCard();
            if (selectedProductCard != null)
            {
                roomBuilder.BuildRoomInCell(this, selectedProductCard.productPrefab, selectedProductCard.productPrefab.tag);
            }
            else
            {
                Debug.LogWarning("No product card selected.");
            }
        }
        else
        {
            Debug.LogError("RoomBuilder not found in the scene.");
        }
    }

    public void HighlightCell(bool highlight)
    {
        if (highlightObject != null)
        {
            highlightObject.SetActive(highlight); // Включаем или отключаем объект выделения
            Debug.Log($"Cell {name} highlighted: {highlight}"); // Отладочное сообщение
            
            if (highlight) 
            {
                PlaySelectSound(); // Воспроизводим звук при выделении ячейки
            }
        }
    }

    private void PlaySelectSound()
    {
        if (audioSource != null && selectSound != null)
        {
            audioSource.Play(); // Воспроизводим звук выбора
        }
    }

    public void RemoveHighlight()
    {
        HighlightCell(false); // Снимаем выделение
    }

    public void SetCellEmpty(bool isClosed, bool isBuild)
    {
        if (closed != null)
        {
            closed.SetActive(isClosed);
        }

        if (buildButton != null)
        {
            buildButton.SetActive(isBuild);
        }

        IsEmpty = !isBuild;
    }

    public void SetCellReturnBuild()
    {
        if (closed != null)
        {
            closed.SetActive(true);
        }

        if (buildButton != null)
        {
            buildButton.SetActive(false);
        }

        IsEmpty = true;
        HighlightCell(false); // Снимаем выделение при возврате ячейки
    }

    public Transform FindRoomInCellByTag(string tag)
    {
    foreach (Transform child in transform) // Перебираем все дочерние объекты ячейки
    {
        if (child.CompareTag(tag)) // Проверяем, совпадает ли тег
        {
            return child; // Возвращаем найденную комнату
        }
    }

    Debug.LogWarning($"Комната с тегом '{tag}' не найдена в ячейке '{name}'.");
    return null; // Если не нашли, возвращаем null
    }
}