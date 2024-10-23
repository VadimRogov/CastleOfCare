using UnityEngine;
using UnityEngine.UI;

public class ProductCard : MonoBehaviour
{
    public string productName; // ��� ������
    public GameObject productPrefab; // ������ �� ������ ������
    public Button productButton; // ������ �� ������ ������

    void Start()
    {
        // ��������� ���������� ������� ��� ������ ������
        if (productButton != null)
        {
            productButton.onClick.AddListener(() => OnProductButtonClicked());
        }
        else
        {
            Debug.LogWarning("Product button not found in prefab: " + productName);
        }
    }

    // ���������� ��� ����� �� ������ ��������
    private void OnProductButtonClicked()
    {
        Debug.Log("Product button clicked: " + productName);

        // ������� ������ ShopPanelControll � �������� ����� ��� �������� ������
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