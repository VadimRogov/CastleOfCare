using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopPersonalController : MonoBehaviour
{
   [System.Serializable]
   public class PersonnelCategory
   {
       public Button categoryButton;
       public string path; // Путь для загрузки префабов персонала
   }

   public AudioClip clickSound; // Звук при нажатии кнопки
   public AudioClip backgroundMusic; // Фоновая музыка

   private AudioSource audioSource; 

   public GameObject shopPersonalPanel; 

   public GameObject[] buttonsToHide; 

   public Transform personnelContent; 

   public List<PersonnelCategory> personnelCategories = new List<PersonnelCategory>();

   private Button currentSelectedCategoryButton;
   private Color selectedColor = new Color(0.5f, 1f, 0f);
   private Color defaultColor = Color.white;

   private PersonalCard selectedPersonalCard;

   void Start()
   {
       audioSource = gameObject.AddComponent<AudioSource>(); 

       audioSource.clip = backgroundMusic; 

       audioSource.loop = true;

       foreach (PersonnelCategory category in personnelCategories)
       {
           PersonnelCategory capturedCategory = category; 
           category.categoryButton.onClick.AddListener(() => OnPersonnelCategoryButtonClicked(capturedCategory));
       }

       CloseShopPersonalPanel(); 
   }

   public void OpenShopPersonalPanel()
   {
       shopPersonalPanel.SetActive(true);
       HideButtons();
       ShowPersonnelForCategory(personnelCategories[0].path); 
       audioSource.Play(); 
   }

   public void CloseShopPersonalPanel()
   {
       shopPersonalPanel.SetActive(false);
       ShowButtons();
       audioSource.Stop(); 
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

   private void OnPersonnelCategoryButtonClicked(PersonnelCategory category)
   {
       if (currentSelectedCategoryButton != null)
       {
           ChangeButtonColor(currentSelectedCategoryButton, defaultColor);
       }

       currentSelectedCategoryButton = category.categoryButton;
       ChangeButtonColor(currentSelectedCategoryButton, selectedColor);

       ShowPersonnelForCategory(category.path);
       
       PlayClickSound(); 
   }

   private void ChangeButtonColor(Button button, Color color)
   {
      Image buttonImage = button.GetComponent<Image>();
      
      if (buttonImage != null)
      {
          buttonImage.color = color;
      }
  }

  public void ShowPersonnelForCategory(string path)
  {
      foreach (Transform child in personnelContent)
      {
          Destroy(child.gameObject);
      }

      GameObject[] personnelPrefabs = Resources.LoadAll<GameObject>(path);

      if (personnelPrefabs.Length == 0)
      {
          Debug.LogWarning($"No personnel prefabs found in the path: {path}");
          return;
      }

      foreach (GameObject prefab in personnelPrefabs)
      {
          GameObject personnelInstance = Instantiate(prefab, personnelContent);

          Text personnelName = personnelInstance.GetComponentInChildren<Text>();
          if (personnelName != null)
          {
              personnelName.text = prefab.name; 
          }

          Button personnelButton = personnelInstance.GetComponentInChildren<Button>();
          if (personnelButton != null)
          {
              personnelButton.onClick.AddListener(() => OnPersonnelButtonClicked(personnelInstance));
              PlayClickSound(); 
          }
      }
  }

private void OnPersonnelButtonClicked(GameObject personnelInstance)
{
    Debug.Log("Personnel button clicked: " + personnelInstance.name);
    
    CloseShopPersonalPanel(); 
    PlayClickSound(); 

    // Проверяем, является ли объект "психологом" или "ЛФК"
    if (personnelInstance.CompareTag("Psych") || personnelInstance.CompareTag("LFK"))
    {
        PersonalCard personalCard = personnelInstance.GetComponent<PersonalCard>();
        if (personalCard != null)
        {
            selectedPersonalCard = personalCard; // Сохраняем выбранную карточку

            CellManager cellManager = FindObjectOfType<CellManager>();
            if (cellManager == null)
            {
                Debug.LogError("CellManager not found in the scene.");
                return;
            }

            // Находим ячейку, которая содержит комнату с тегом, совпадающим с тегом personnelInstance
            Cell cellWithRoom = cellManager.FindCellWithRoomByTag(personnelInstance.tag);
            if (cellWithRoom != null)
            {
                // Проверяем наличие комнаты с нужным тегом в найденной ячейке
                Transform room = cellWithRoom.FindRoomInCellByTag(personnelInstance.tag);
                if (room != null)
                {
                    // Проверяем, есть ли уже персонал с тем же тегом в комнате
                    bool hasSameTagPersonnel = false;
                    foreach (Transform child in room) // Перебираем всех дочерних объектов в комнате
                    {
                        if (child.CompareTag(personnelInstance.tag)) // Проверяем тег каждого дочернего объекта
                        {
                            hasSameTagPersonnel = true; // Найден персонал с таким же тегом
                            break; // Выходим из цикла, если нашли совпадение
                        }
                    }

                    if (hasSameTagPersonnel)
                    {
                        Debug.LogWarning($"Нельзя создать нового персонажа. В комнате '{room.name}' уже есть персонал с тегом '{personnelInstance.tag}'.");
                        return; // Выходим из метода, если персонал с таким же тегом уже есть
                    }

                    // Создаем персонажа в комнате на основе префаба из PersonalCard
                    GameObject characterPrefab = personalCard.personalPrefab; // Получаем префаб персонажа из карточки
                    if (characterPrefab != null)
                    {
                        // Создаем экземпляр персонажа без родителя
                        GameObject characterInstance = Instantiate(characterPrefab);

                        // Устанавливаем родителем комнату
                        characterInstance.transform.SetParent(room);

                        // Устанавливаем позицию 
                        characterInstance.transform.localPosition = new Vector3(3, 0, -4); // Позиция 
                        characterInstance.transform.localRotation = Quaternion.Euler(0, 180, 0); 

                        Debug.Log($"Character '{characterPrefab.name}' created in room '{room.name}' at position Y +10.");
                    }
                    else
                    {
                        Debug.LogWarning("Character prefab is not assigned in PersonalCard.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Комната с тегом '{personnelInstance.tag}' не найдена в ячейке '{cellWithRoom.name}'.");
                }
            }
            else
            {
                Debug.LogWarning($"Ячейка с комнатой с тегом '{personnelInstance.tag}' не найдена.");
            }
        }
        else
        {
            Debug.LogWarning("No PersonalCard component found on the personnel instance.");
        }
    }
}

private void PlayClickSound()
{
     audioSource.PlayOneShot(clickSound); 
}

public void SelectPersonalCard(PersonalCard card) 
  { 
      selectedPersonalCard = card; 
  } 

  public PersonalCard GetSelectedProductCard()  
  {  
      return selectedPersonalCard;  
  }  
}