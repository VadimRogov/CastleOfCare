using TMPro;
using UnityEngine;
using System.Collections; // Для использования Coroutine

public class CurrencyManager : MonoBehaviour
{
    public TextMeshProUGUI coinsText; // UI текст для отображения монет
    public TextMeshProUGUI puzzlesText; // UI текст для отображения пазлов
    public GameObject presentPanel; // Панель для подарков или уведомлений

    private const string CoinsKey = "Coins";   // Ключ для монет в PlayerPrefs
    private const string PuzzlesKey = "Puzzles"; // Ключ для пазлов в PlayerPrefs
    
    private void Start()
    {
        // Сброс данных на 0 при каждом запуске игры
        PlayerPrefs.SetInt(CoinsKey, 0);   // Обнуляем монеты
        PlayerPrefs.SetInt(PuzzlesKey, 0); // Обнуляем пазлы

        // Запускаем процесс добавления монет с задержкой
        StartCoroutine(StartCurrencyProcess());
        
        // Обновляем UI для отображения текущего состояния валют
        UpdateCurrencyUI();
    }

    // Coroutine для задержки и добавления монет
    private IEnumerator StartCurrencyProcess()
    {
        // Задержка в 10 секунд
        yield return new WaitForSeconds(10f);

        // Добавляем 10,000 монет только после задержки
        AddCoins(10000);

        // Показываем панель с подарками или уведомлениями
        presentPanel.SetActive(true);
    }

    // Метод для получения количества монет
    private int GetCoins()
    {
        return PlayerPrefs.GetInt(CoinsKey, 0); // Возвращаем 0, если нет сохраненных монет
    }

    // Метод для получения количества пазлов
    private int GetPuzzles()
    {
        return PlayerPrefs.GetInt(PuzzlesKey, 0); // Возвращаем 0, если нет сохраненных пазлов
    }

    // Метод для добавления монет
    private void AddCoins(int amount)
    {
        int currentCoins = GetCoins();
        PlayerPrefs.SetInt(CoinsKey, currentCoins + amount);
        PlayerPrefs.Save();
        UpdateCurrencyUI();
    }

    // Метод для добавления пазлов
    private void AddPuzzles(int amount)
    {
        int currentPuzzles = GetPuzzles();
        PlayerPrefs.SetInt(PuzzlesKey, currentPuzzles + amount);
        PlayerPrefs.Save();
        UpdateCurrencyUI();
    }

    // Метод для обмена пазлов на монеты
    public void ExchangePuzzlesForCoins(int puzzleAmount, int coinsPerPuzzle)
    {
        int currentPuzzles = GetPuzzles();
        if (currentPuzzles >= puzzleAmount)
        {
            int coinsToAdd = puzzleAmount * coinsPerPuzzle;

            AddPuzzles(-puzzleAmount);
            AddCoins(coinsToAdd);
        }
        else
        {
            Debug.Log("Недостаточно пазлов для обмена.");
        }
    }

    // Метод для обновления UI
    private void UpdateCurrencyUI()
    {
        coinsText.text = GetCoins().ToString();
        puzzlesText.text = GetPuzzles().ToString();
    }
}
