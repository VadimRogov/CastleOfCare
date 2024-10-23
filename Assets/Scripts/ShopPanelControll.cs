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
    public GameObject hospice; // Ссылка на объект Hospice

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

            // Добавляем обработчик события для кнопки продукта
            Button productButton = productInstance.GetComponentInChildren<Button>();
            if (productButton != null)
            {
                productButton.onClick.AddListener(() => OnProductButtonClicked(productInstance));
            }
            else
            {
                Debug.LogWarning("Product button not found in prefab: " + prefab.name);
            }
        }
    }

    // Обработчик для клика по кнопке продукта
    private void OnProductButtonClicked(GameObject productInstance)
    {
        Debug.Log("Product button clicked: " + productInstance.name);

        // Закрыть панель магазина
        CloseShopPanel();

        // Выделить первую свободную ячейку для строительства
        Cell freeCell = HighlightFirstFreeCell();

        if (freeCell != null)
        {
            // Создаем экземпляр товара в выбранной ячейке
            ProductCard productCard = productInstance.GetComponent<ProductCard>();
            if (productCard != null && productCard.productPrefab != null)
            {
                // Создаем экземпляр товара с сохранением исходной ориентации и размеров
                GameObject productInstanceInCell = Instantiate(productCard.productPrefab, freeCell.transform.position, Quaternion.identity);
                productInstanceInCell.transform.SetParent(freeCell.transform, false);
                freeCell.isEmpty = true;
                freeCell.UpdateCellState();
            }
            else
            {
                Debug.LogWarning("Product prefab not found for product: " + productInstance.name);
            }
        }
        else
        {
            Debug.Log("No free cells found in Hospice");
        }
    }

    // Метод для выделения первой свободной ячейки
    private Cell HighlightFirstFreeCell()
    {
        Debug.Log("HighlightFirstFreeCell method called");

        // Получаем все ячейки в Hospice
        Cell[] cells = hospice.GetComponentsInChildren<Cell>();

        Debug.Log("Found " + cells.Length + " cells in Hospice");

        // Сначала ищем первую свободную ячейку на первом этаже
        Cell freeCell = FindFirstFreeCellOnFloor(cells, "Stage1");

        if (freeCell == null)
        {
            // Если все ячейки на первом этаже заняты, ищем первую свободную ячейку на втором этаже
            freeCell = FindFirstFreeCellOnFloor(cells, "Stage2");
        }

        if (freeCell == null)
        {
            // Если все ячейки на первых двух этажах заняты, ищем первую свободную ячейку на третьем этаже
            freeCell = FindFirstFreeCellOnFloor(cells, "Stage3");
        }

        if (freeCell != null)
        {
            Debug.Log("Found free cell: " + freeCell.name);

            // Активируем объект Open и деактивируем объект Closed только на первой свободной ячейке
            freeCell.Open.SetActive(true);
            freeCell.Closed.SetActive(false);
        }
        else
        {
            Debug.Log("No free cells found in Hospice");
        }

        return freeCell;
    }

    // Метод для поиска первой свободной ячейки на указанном этаже
    private Cell FindFirstFreeCellOnFloor(Cell[] cells, string floorName)
    {
        foreach (Cell cell in cells)
        {
            if (!cell.isEmpty && cell.transform.parent.name == floorName)
            {
                return cell;
            }
        }
        return null;
    }

    // Метод для создания товара в выбранной ячейке
    public void CreateProduct(GameObject productPrefab)
    {
        // Выделить первую свободную ячейку для строительства
        Cell freeCell = HighlightFirstFreeCell();

        if (freeCell != null)
        {
            // Создаем экземпляр товара в выбранной ячейке
            if (productPrefab != null)
            {
                // Создаем экземпляр товара с сохранением исходной ориентации и размеров
                GameObject productInstanceInCell = Instantiate(productPrefab, freeCell.transform.position, Quaternion.identity);
                productInstanceInCell.transform.SetParent(freeCell.transform, false);
                freeCell.isEmpty = true;
                freeCell.UpdateCellState();
            }
            else
            {
                Debug.LogWarning("Product prefab is null.");
            }
        }
        else
        {
            Debug.Log("No free cells found in Hospice");
        }
    }
}