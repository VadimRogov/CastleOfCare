using UnityEngine;
using UnityEngine.UI;

public class ProductCard : MonoBehaviour
{
    public string productName; // Имя товара
    public GameObject productPrefab; // Ссылка на префаб товара
    public Button productButton; // Ссылка на кнопку товара

    void Start()
    {
        // Добавляем обработчик события для кнопки товара
        if (productButton != null)
        {
            productButton.onClick.AddListener(() => OnProductButtonClicked());
        }
        else
        {
            Debug.LogWarning("Product button not found in prefab: " + productName);
        }
    }

    // Обработчик для клика по кнопке продукта
    private void OnProductButtonClicked()
    {
        Debug.Log("Product button clicked: " + productName);

        // Находим скрипт ShopPanelControll и вызываем метод для создания товара
        ShopPanelControll shopPanelControll = FindObjectOfType<ShopPanelControll>();
        if (shopPanelControll != null)
        {
            shopPanelControll.CreateProduct(productPrefab);
        }
        else
        {
            Debug.LogError("ShopPanelControll not found in the scene.");
        }
    }
}