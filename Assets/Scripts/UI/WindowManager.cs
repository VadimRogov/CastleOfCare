using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    public Button userButton;  // Кнопка UserButton
    public GameObject userEdit; // Окно UserEdit
    public Button backButton;   // Кнопка для возврата в UserEdit

    void Start()
    {
        // Убедимся, что в начале кнопка видна, а окно скрыто
        userButton.gameObject.SetActive(true);
        userEdit.SetActive(false);

        // Подписываемся на событие нажатия на кнопки
        userButton.onClick.AddListener(OnUserButtonClick);
        backButton.onClick.AddListener(OnBackButtonClick);
    }

    // Метод, вызываемый при нажатии на UserButton
    void OnUserButtonClick()
    {
        // Отключаем кнопку и включаем окно UserEdit
        userButton.gameObject.SetActive(false);
        userEdit.SetActive(true);
    }

    // Метод, вызываемый при нажатии на кнопку возврата в UserEdit
    void OnBackButtonClick()
    {
        // Отключаем окно UserEdit и снова включаем UserButton
        userEdit.SetActive(false);
        userButton.gameObject.SetActive(true);
    }
}
