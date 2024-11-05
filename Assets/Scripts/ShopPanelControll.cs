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

    
    public AudioClip buttonClickSound;
    public AudioClip backgroundMusic;
    private AudioSource audioSource;
    private AudioSource musicSource; 

    private bool isShopPanelOpen = false; 

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        
   
        musicSource.clip = backgroundMusic;
        musicSource.loop = true; 

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

       
        if (!musicSource.isPlaying)
        {
            musicSource.Play();
            isShopPanelOpen = true;
        }
    }

    public void CloseShopPanel()
    {
        shopPanel.SetActive(false);
        ShowButtons();

   
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
            isShopPanelOpen = false; 
        }
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
        if (isShopPanelOpen) 
        {
            PlayButtonClickSound(); 
        }

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
       if (isShopPanelOpen) 
       {
           PlayButtonClickSound(); 
       }

       Debug.Log("Product button clicked: " + productInstance.name);
       CloseShopPanel();

       // Проверяем, является ли продукт допустимым типом комнаты
       if (productInstance.CompareTag("Lift") || 
           productInstance.CompareTag("Chamber") || 
           productInstance.CompareTag("Psych") || 
           productInstance.CompareTag("LFK"))
       {   
           ProductCard productCard = productInstance.GetComponent<ProductCard>();
           if (productCard != null) 
           { 
               selectedProductCard = productCard;
               CellManager cellManager = FindObjectOfType<CellManager>();
               if (cellManager == null) 
               { 
                   Debug.LogError("CellManager not found in the scene."); 
                   return; 
               } 

               Cell freeCell = cellManager.FindCellForFirstStages(); 

               if (freeCell != null) 
               { 
                   RoomBuilder roomBuilder = FindObjectOfType<RoomBuilder>(); 
                   roomBuilder.BuildRoomInCell(freeCell, selectedProductCard.productPrefab, productInstance.tag); 
               } 
               else 
               { 
                   Debug.LogWarning("No free cell available for building."); 
               } 
           } 
       }  
       else
       {
           Debug.LogWarning($"Product {productInstance.name} is not a valid type for building.");
       }
   }

   private void PlayButtonClickSound()
   {
       if (buttonClickSound != null && audioSource != null)
       {
           audioSource.PlayOneShot(buttonClickSound);
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