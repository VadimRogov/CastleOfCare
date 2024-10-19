using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CategoryPath
{
    public Button categoryButton; // Ссылка на кнопку категории
    public string path; // Путь к директории с префабами товаров
}

public class ShopPanelControll : MonoBehaviour
{
    public GameObject shopPanel;  // Панель магазина
    public GameObject[] buttonsToHide; // Кнопки, которые нужно скрывать при открытии магазина
    public Transform productContent; // Контейнер для карточек товаров

    // Список категорий и их путей
    public List<CategoryPath> categoryPaths = new List<CategoryPath>();

    // Переменная для хранения ссылки на текущую выбранную категорию
    private Button currentSelectedCategoryButton;

    // Цвет для активной категории
    private Color selectedColor = new Color(0.5f, 1f, 0f); // Салатовый цвет (7FFF00)
    private Color defaultColor = Color.white; // Цвет по умолчанию для кнопок

    // Кнопка для категории "Magic" (установите в инспекторе)
    public Button magicCategoryButton;

    void Start()
    {
        // Добавляем слушатели на кнопки категорий
        foreach (CategoryPath categoryPath in categoryPaths)
        {
            // Необходимо захватить локальную копию categoryPath, чтобы избежать замыкания
            CategoryPath capturedCategoryPath = categoryPath;
            categoryPath.categoryButton.onClick.AddListener(() => OnCategoryButtonClicked(capturedCategoryPath));
        }
    }

    // Открыть панель магазина и скрыть кнопки
    public void OpenShopPanel()
    {
        shopPanel.SetActive(true);
        HideButtons();

        // Автоматически нажимаем на кнопку "Magic"
        if (magicCategoryButton != null)
        {
            magicCategoryButton.onClick.Invoke();
        }
    }

    // Закрыть панель магазина и показать кнопки
    public void CloseShopPanel()
    {
        shopPanel.SetActive(false);
        ShowButtons();
    }

    // Скрыть кнопки
    private void HideButtons()
    {
        foreach (GameObject button in buttonsToHide)
        {
            button.SetActive(false);
        }
    }

    // Показать кнопки
    private void ShowButtons()
    {
        foreach (GameObject button in buttonsToHide)
        {
            button.SetActive(true);
        }
    }

    // Обработчик для клика по кнопке категории
    private void OnCategoryButtonClicked(CategoryPath categoryPath)
    {
        // Изменить цвет предыдущей выбранной кнопки на стандартный
        if (currentSelectedCategoryButton != null)
        {
            ChangeButtonColor(currentSelectedCategoryButton, defaultColor);
        }

        // Изменить цвет текущей кнопки на салатовый
        currentSelectedCategoryButton = categoryPath.categoryButton;
        ChangeButtonColor(currentSelectedCategoryButton, selectedColor);

        // Показать продукты для выбранной категории
        ShowProductsForCategory(categoryPath.path);
    }

    // Изменить цвет кнопки
    private void ChangeButtonColor(Button button, Color color)
    {
        // Получаем Image компонента кнопки и меняем цвет
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }
    }

    // Отображение товаров для выбранной категории
    public void ShowProductsForCategory(string path)
    {
        // Очищаем предыдущие продукты
        foreach (Transform child in productContent)
        {
            Destroy(child.gameObject);
        }

        // Загружаем префабы товаров из директории внутри Resources
        GameObject[] productPrefabs = Resources.LoadAll<GameObject>(path);

        if (productPrefabs.Length == 0)
        {
            Debug.LogWarning($"No product prefabs found in the path: {path}");
            return; // Если нет префабов, ничего не отображаем
        }

        // Создаем карточки товаров и настраиваем их
        foreach (GameObject prefab in productPrefabs)
        {
            GameObject productInstance = Instantiate(prefab, productContent);

            // Настройка карточки товара
            Text productName = productInstance.GetComponentInChildren<Text>();
            if (productName != null)
            {
                // Здесь устанавливается имя товара (используем имя префаба)
                productName.text = prefab.name;
            }

            // Пример: если есть картинка, обновите её
            Image productImage = productInstance.GetComponentInChildren<Image>();
            if (productImage != null)
            {
                // Если у вас есть соответствующие изображения для товаров, вы можете их применить
                // productImage.sprite = вашСпрайт;
            }
        }
    }
}
