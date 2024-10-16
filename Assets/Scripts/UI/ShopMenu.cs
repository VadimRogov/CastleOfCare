using UnityEngine;
using UnityEngine.UI;
public class ShopMenu : MonoBehaviour
{
    public Button shopButton;      // Кнопка для открытия магазина
    public GameObject shopPanel;   // Панель магазина
    public GameObject categoryButtonPrefab; // Префаб для кнопки категории
    public Transform categoryContent;       // Контейнер для динамических кнопок категорий
    public ScrollRect productScrollView;    // ScrollView для товаров
    public GameObject productItemPrefab;    // Префаб товара
    public Transform productContent;        // Контейнер для товаров внутри ScrollView

    // Пример категорий и товаров по ним (можно заменить на реальные данные)
    private string[] categories = { "Category A", "Category B", "Category C", "Category D", "Category E", "Category F", "Category G" };
    private string[][] products = new string[][]
    {
        new string[] { "Product 1A", "Product 2A", "Product 3A" },
        new string[] { "Product 1B", "Product 2B", "Product 3B" },
        new string[] { "Product 1C", "Product 2C" },
        new string[] { "Product 1D", "Product 2D", "Product 3D", "Product 4D" },
        new string[] { "Product 1E", "Product 2E", "Product 3E" },
        new string[] { "Product 1F", "Product 2F", "Product 3F" },
        new string[] { "Product 1G", "Product 2G", "Product 3G" }
    };

    void Start()
    {
        // Изначально магазин закрыт
        shopPanel.SetActive(false);

        // Подписываемся на нажатие кнопки открытия магазина
        shopButton.onClick.AddListener(OpenShop);

        // Динамически создаем кнопки для всех категорий
        CreateCategoryButtons();
    }

    // Открытие магазина
    void OpenShop()
    {
        shopPanel.SetActive(true);
        ShowProductsForCategory(0); // По умолчанию показываем первую категорию
    }

    // Динамическое создание кнопок для всех категорий
    void CreateCategoryButtons()
    {
        for (int i = 0; i < categories.Length; i++)
        {
            int categoryIndex = i;  // Локальная копия индекса для лямбда-выражения
            // Создаем новую кнопку из префаба
            GameObject newButton = Instantiate(categoryButtonPrefab, categoryContent);
            newButton.GetComponentInChildren<Text>().text = categories[i];  // Назначаем название категории

            // Добавляем слушатель для кнопки
            newButton.GetComponent<Button>().onClick.AddListener(() => ShowProductsForCategory(categoryIndex));
        }
    }

    // Показ товаров для выбранной категории
    void ShowProductsForCategory(int categoryIndex)
    {
        // Очищаем список товаров
        foreach (Transform child in productContent)
        {
            Destroy(child.gameObject);
        }

        // Заполняем товары для выбранной категории
        foreach (string product in products[categoryIndex])
        {
            GameObject productItem = Instantiate(productItemPrefab, productContent);
            productItem.GetComponentInChildren<Text>().text = product;
        }

        // Обновляем ScrollView
        productScrollView.verticalNormalizedPosition = 1;
    }
}
