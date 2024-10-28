using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CategoryPath
{
    public Button categoryButton;
    public string path;
}

public class ShopPanelControll : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject[] buttonsToHide;
    public Transform productContent;
    public List<CategoryPath> categoryPaths = new List<CategoryPath>();

    private Button currentSelectedCategoryButton;
    private Color selectedColor = new Color(0.5f, 1f, 0f);
    private Color defaultColor = Color.white;

    public Button magicCategoryButton;

    private ProductCard selectedProductCard;

    void Start()
    {
        foreach (CategoryPath categoryPath in categoryPaths)
        {
            CategoryPath capturedCategoryPath = categoryPath;
            categoryPath.categoryButton.onClick.AddListener(() => OnCategoryButtonClicked(capturedCategoryPath));
        }
    }

    public void OpenShopPanel()
    {
        shopPanel.SetActive(true);
        HideButtons();
        magicCategoryButton?.onClick.Invoke();
    }

    public void CloseShopPanel()
    {
        shopPanel.SetActive(false);
        ShowButtons();
    }

    private void HideButtons()
    {
        foreach (GameObject button in buttonsToHide)
        {
            button.SetActive(false);
        }
    }

    private void ShowButtons()
    {
        foreach (GameObject button in buttonsToHide)
        {
            button.SetActive(true);
        }
    }

    private void OnCategoryButtonClicked(CategoryPath categoryPath)
    {
        if (currentSelectedCategoryButton != null)
        {
            ChangeButtonColor(currentSelectedCategoryButton, defaultColor);
        }

        currentSelectedCategoryButton = categoryPath.categoryButton;
        ChangeButtonColor(currentSelectedCategoryButton, selectedColor);

        ShowProductsForCategory(categoryPath.path);
    }

    private void ChangeButtonColor(Button button, Color color)
    {
        Image buttonImage = button.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color;
        }
    }

    public void ShowProductsForCategory(string path)
    {
        foreach (Transform child in productContent)
        {
            Destroy(child.gameObject);
        }

        GameObject[] productPrefabs = Resources.LoadAll<GameObject>(path);
        if (productPrefabs.Length == 0)
        {
            Debug.LogWarning($"No product prefabs found in the path: {path}");
            return;
        }

        foreach (GameObject prefab in productPrefabs)
        {
            GameObject productInstance = Instantiate(prefab, productContent);
            Text productName = productInstance.GetComponentInChildren<Text>();
            if (productName != null)
            {
                productName.text = prefab.name;
            }

            Button productButton = productInstance.GetComponentInChildren<Button>();
            if (productButton != null)
            {
                productButton.onClick.AddListener(() => OnProductButtonClicked(productInstance));
            }
        }
    }

    private void OnProductButtonClicked(GameObject productInstance)
    {
        Debug.Log("Product button clicked: " + productInstance.name);
        CloseShopPanel();

        if (productInstance.CompareTag("Build"))
        {
            CellManager cellManager = FindObjectOfType<CellManager>();
            Cell freeCell = cellManager?.FindFirstStage();

            if (freeCell != null)
            {
                ProductCard productCard = productInstance.GetComponent<ProductCard>();
                if (productCard != null && productCard.productPrefab != null)
                {
                    selectedProductCard = productCard;
                }
            }
            else
            {
                Debug.LogWarning("No free cell available for building.");
            }
        }
    }

    public ProductCard GetSelectedProductCard()
    {
        return selectedProductCard;
    }
}
