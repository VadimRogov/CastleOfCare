using UnityEngine;
using UnityEngine.UI;

public class ShopPanelController : MonoBehaviour
{
    // Ссылка на панель магазина
    public GameObject shopPanel;

    // Список кнопок, которые нужно скрыть/показать
    public GameObject[] buttonsToHide;

    // Метод для активации панели магазина и скрытия кнопок
    public void OpenShopPanel()
    {
        shopPanel.SetActive(true);
        HideButtons();
    }

    // Метод для деактивации панели магазина и показа кнопок
    public void CloseShopPanel()
    {
        shopPanel.SetActive(false);
        ShowButtons();
    }

    // Метод для скрытия кнопок
    private void HideButtons()
    {
        foreach (GameObject button in buttonsToHide)
        {
            button.SetActive(false);
        }
    }

    // Метод для показа кнопок
    private void ShowButtons()
    {
        foreach (GameObject button in buttonsToHide)
        {
            button.SetActive(true);
        }
    }
}