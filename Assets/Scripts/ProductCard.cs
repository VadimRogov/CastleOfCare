using UnityEngine;
using UnityEngine.UI;

public class ProductCard : MonoBehaviour
{
    public GameObject productPrefab; // Префаб продукта/комнаты
    public Button productButton; // Кнопка для выбора продукта

    void Start()
    {
        if (productButton != null)
        {
            productButton.onClick.AddListener(OnProductButtonClicked);
        }
    }

   private void OnProductButtonClicked()
   {
       Debug.Log($"Product {productPrefab.name} selected for building.");
       
       ShopPanelControll shopPanelControll = FindObjectOfType<ShopPanelControll>();
       
       if (shopPanelControll != null)
       {
           shopPanelControll.SelectProductCard(this); // Передаем ссылку на текущую карточку продукта в контроллер магазина.
       }
       else 
       {
           Debug.LogError("ShopPanelControll not found in the scene.");
       }
   }
}