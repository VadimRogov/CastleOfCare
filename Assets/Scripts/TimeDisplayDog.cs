using UnityEngine;
using System;
using UnityEngine.UI;
public class TimeDisplayDog : MonoBehaviour
{
    public Button timeButton;  // Ссылка на кнопку
    private Text timeText;     // Текст для отображения времени
    private TimeSpan elapsedTime;
    private float timer;       // Для отсчета времени

    private void Start()
    {
        // Получаем компонент текста, находящийся внутри кнопки
        timeText = timeButton.GetComponentInChildren<Text>();
        
        // Инициализация времени (начнем с нуля)
        elapsedTime = TimeSpan.Zero;
        
        // Запуск отсчета времени
        timer = 0f;
    }

    private void Update()
    {
        // Увеличиваем таймер на прошедшее время кадра
        timer += Time.deltaTime;
        
        // Преобразуем его в TimeSpan (чтобы получить в формате HH:MM:SS)
        elapsedTime = TimeSpan.FromSeconds(timer);
        
        // Обновляем текст кнопки в формате HH:MM:SS
        timeText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds);
    }
}
