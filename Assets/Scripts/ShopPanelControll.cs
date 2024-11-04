using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopPanelControll : MonoBehaviour
{
   [System.Serializable]
   public class CategoryPath
   {
       public Button categoryButton;
       public string path;
   }

   public GameObject shopPanel; 
   public GameObject[] buttonsToHide;
   public Transform productContent;

   public List<CategoryPath> categoryPaths = new List<CategoryPath>();

   private Button currentSelectedCategoryButton;
   private Color selectedColor = new Color(0.5f, 1f, 0f); 
   private Color defaultColor = Color.white;

   private ProductCard selectedProductCard;

   void Start()
   {
       foreach (CategoryPath categoryPath in categoryPaths)
       {
           CategoryPath capturedCategoryPath = categoryPath;
           categoryPath.categoryButton.onClick.AddListener(() => OnCategoryButtonClicked(capturedCategoryPath));
       }

       CloseShopPanel(); 
   }

   public void OpenShopPanel()
   {
       shopPanel.SetActive(true);
       HideButtons();
       ShowProductsForCategory(categoryPaths[0].path); 
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

    if (productInstance.CompareTag("Build")) // Проверяем, является ли продукт строителем
    {   
        ProductCard productCard = productInstance.GetComponent<ProductCard>();
        if(productCard != null) 
        { 
            selectedProductCard = productCard;
            CellManager cellManager = FindObjectOfType<CellManager>();
            if(cellManager == null) 
            { 
                Debug.LogError("CellManager not found in the scene."); 
                return; 
            } 

            Cell freeCell = cellManager.FindCellForFirsStages(); 

            if(freeCell != null) 
            { 
                RoomBuilder roomBuilder = FindObjectOfType<RoomBuilder>(); 
                roomBuilder.BuildRoomInCell(freeCell, selectedProductCard.productPrefab, productInstance.tag); // Передаем тег продукта
            } 
            else 
            { 
                Debug.LogWarning("No free cell available for building."); 
            } 
        } 
    }  
}

  public void SelectProductCard(ProductCard card) 
  { 
      selectedProductCard = card; 
  } 

  private void ResetAllCells() 
  { 
      CellManager cellManager = FindObjectOfType<CellManager>(); 
      if(cellManager != null) 
      { 
          foreach(Stage stage in cellManager.Stages) 
          {  
              foreach(Cell cell in stage.Cells) 
              {  
                  cell.SetCellReturnBuild();  
              }  
          }  
      }  
  }  

  public ProductCard GetSelectedProductCard()  
  {  
      return selectedProductCard;  
  }  
}