using UnityEngine;
using UnityEngine.UI;

public class PersonalCard : MonoBehaviour
{
    public GameObject personalPrefab; // Префаб персонала

    public Button personalButton; // Кнопка карточки персонала

    private void Start() {
        if (personalButton != null)
        {
            personalButton.onClick.AddListener(OnPersonalButtonClicked);
        }
    }

    private void OnPersonalButtonClicked()
    {
        Debug.Log($"Product {personalPrefab.name} selected for created.");
       
       ShopPersonalController shopPersonalController = FindObjectOfType<ShopPersonalController>();
       
       if (shopPersonalController != null)
       {
           shopPersonalController.SelectPersonalCard(this); // Передаем ссылку на текущую карточку продукта в контроллер магазина.
       }
       else 
       {
           Debug.LogError("ShopPersonalController not found in the scene.");
       }
   }
}
