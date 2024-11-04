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
    private AudioSource audioSource; // Компонент AudioSource для воспроизведения звуков

    public GameObject shopPersonalPanel; // Панель для отображения персонала
    public GameObject[] buttonsToHide; // Кнопки, которые нужно скрыть при открытии магазина
    public Transform personnelContent; // Область контента для отображения персонала

    public List<PersonnelCategory> personnelCategories = new List<PersonnelCategory>();

    private Button currentSelectedCategoryButton;
    private Color selectedColor = new Color(0.5f, 1f, 0f);
    private Color defaultColor = Color.white;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Добавляем AudioSource к объекту
        audioSource.clip = backgroundMusic; // Устанавливаем фоновую музыку
        audioSource.loop = true; // Зацикливаем музыку

        foreach (PersonnelCategory category in personnelCategories)
        {
            PersonnelCategory capturedCategory = category; // Захватываем ссылку для слушателя
            category.categoryButton.onClick.AddListener(() => OnPersonnelCategoryButtonClicked(capturedCategory));
        }

        CloseShopPersonalPanel(); // Убедимся, что панель закрыта в начале
    }

    public void OpenShopPersonalPanel()
    {
        shopPersonalPanel.SetActive(true);
        HideButtons();
        ShowPersonnelForCategory(personnelCategories[0].path); // Показываем первую категорию по умолчанию
        audioSource.Play(); // Запускаем фоновую музыку
    }

    public void CloseShopPersonalPanel()
    {
        shopPersonalPanel.SetActive(false);
        ShowButtons();
        audioSource.Stop(); // Останавливаем музыку при закрытии
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
        
        PlayClickSound(); // Проигрываем звук клика
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
        // Очищаем существующий персонал
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
                personnelName.text = prefab.name; // Устанавливаем имя персонала
            }

            Button personnelButton = personnelInstance.GetComponentInChildren<Button>();
            if (personnelButton != null)
            {
                personnelButton.onClick.AddListener(() => OnPersonnelButtonClicked(personnelInstance));
                PlayClickSound(); // Проигрываем звук клика при создании кнопки
            }
        }
    }

    private void OnPersonnelButtonClicked(GameObject personnelInstance)
    {
        Debug.Log("Personnel button clicked: " + personnelInstance.name);
        
        // Логика обработки выбора персонала...
        
        CloseShopPersonalPanel(); // Закрываем панель магазина после выбора
        
      PlayClickSound(); // Проигрываем звук клика при выборе персонала
      
      // Дополнительная логика может быть добавлена здесь в зависимости от требований вашей игры
   }

   private void PlayClickSound()
   {
       audioSource.PlayOneShot(clickSound); // Проигрываем звук клика
   }
}