using UnityEngine;
using TMPro; // Не забудьте подключить пространство имен для TextMeshPro

public class GameTimer : MonoBehaviour
{
    private float elapsedTime = 0f;
    private bool isTimerRunning = false;

    [SerializeField] private TextMeshProUGUI timerText; // Поле для TextMeshPro

    void Start()
    {
        // Обнуляем время при старте игры
        elapsedTime = 0f;
        isTimerRunning = true; // Запускаем таймер при старте игры
    }

    void Update()
    {
        if (isTimerRunning)
        {
            elapsedTime += Time.deltaTime; // Увеличиваем время
            UpdateTimerText(); // Обновляем текст на экране
        }
    }

    private void OnApplicationQuit()
    {
        // Сохраняем текущее время при выходе из игры (если нужно сохранить для других целей)
        PlayerPrefs.SetFloat("ElapsedTime", elapsedTime);
        PlayerPrefs.Save();
    }

    // Метод для получения прошедшего времени
    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    // Метод для обновления текста таймера на экране
    private void UpdateTimerText()
    {
        timerText.text = FormatElapsedTime(elapsedTime);
    }

    // Метод для форматирования времени в строку "чч:мм:сс"
    private string FormatElapsedTime(float time)
    {
        int hours = Mathf.FloorToInt(time / 3600);
        int minutes = Mathf.FloorToInt((time % 3600) / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }
}