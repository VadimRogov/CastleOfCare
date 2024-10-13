using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class Category
    {
        public string categoryName; // Название категории
        public List<GameObject> items; // Префабы товаров
        public List<string> filters; // Список фильтров для этой категории (например, "Стулья", "Столы")
    }

    public GameObject categoryButtonPrefab;  // Префаб кнопки для категории
    public Transform categoryButtonContainer; // Контейнер для категорий (левая часть)
    public GameObject filterButtonPrefab;    // Префаб кнопки для фильтров
    public Transform filterButtonContainer;  // Контейнер для фильтров (правая часть)
    public GameObject itemButtonPrefab;      // Префаб кнопки для товаров
    public Transform itemButtonContainer;    // Контейнер для товаров (правая часть)
    public List<Category> categories;        // Список категорий

    private void Start()
    {
        PopulateCategories(); // Заполняем категории при старте
    }

    // Метод для заполнения категорий
    private void PopulateCategories()
    {
        foreach (Category category in categories)
        {
            GameObject categoryButton = Instantiate(categoryButtonPrefab, categoryButtonContainer);
            categoryButton.GetComponentInChildren<Text>().text = category.categoryName;
            categoryButton.GetComponent<Button>().onClick.AddListener(() => OnCategorySelected(category));
        }
    }

    // Метод, вызываемый при выборе категории
    private void OnCategorySelected(Category category)
    {
        // Очищаем старые кнопки фильтров и товаров
        foreach (Transform child in filterButtonContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in itemButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Если у категории есть фильтры, создаём кнопки фильтров
        if (category.filters != null && category.filters.Count > 0)
        {
            foreach (string filter in category.filters)
            {
                GameObject filterButton = Instantiate(filterButtonPrefab, filterButtonContainer);
                filterButton.GetComponentInChildren<Text>().text = filter;
                filterButton.GetComponent<Button>().onClick.AddListener(() => OnFilterSelected(category, filter));
            }
        }
        else
        {
            // Если фильтров нет, показываем все товары категории
            PopulateItems(category.items);
        }
    }

    // Метод, вызываемый при выборе фильтра
    private void OnFilterSelected(Category category, string filter)
    {
        // Очищаем старые товары
        foreach (Transform child in itemButtonContainer)
        {
            Destroy(child.gameObject);
        }

        // Здесь можно отфильтровать товары по какому-то условию, например по тегу или имени
        List<GameObject> filteredItems = category.items.FindAll(item => item.tag == filter);

        // Показываем отфильтрованные товары
        PopulateItems(filteredItems);
    }

    // Метод для отображения товаров
    private void PopulateItems(List<GameObject> items)
    {
        foreach (GameObject item in items)
        {
            GameObject itemButton = Instantiate(itemButtonPrefab, itemButtonContainer);
            itemButton.GetComponentInChildren<Text>().text = item.name;
            itemButton.GetComponent<Button>().onClick.AddListener(() => OnItemSelected(item));
        }
    }

    // Метод, вызываемый при выборе товара
    private void OnItemSelected(GameObject item)
    {
        Debug.Log("Выбран товар: " + item.name);
        // Здесь добавить логику покупки товара
    }
}
