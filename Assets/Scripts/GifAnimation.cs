using UnityEngine;
using UnityEngine.UI;

public class GifAnimation : MonoBehaviour
{
    public string folderPath = "GIFFrames/Cat"; // Путь к папке с кадрами GIF
    public float frameDelay = 0.1f; // Задержка между кадрами
    private RawImage rawImage; // Компонент RawImage для отображения GIF
    private Texture2D[] gifFrames; // Массив текстур для кадров GIF
    private int currentFrame = 0; // Текущий кадр
    private float timer = 0f; // Таймер для задержки

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        LoadGifFrames();
        if (gifFrames.Length > 0)
        {
            rawImage.texture = gifFrames[0];
        }
        else
        {
            Debug.LogError("Gif frames array is empty!");
        }
    }

    void Update()
    {
        if (gifFrames.Length == 0) return; // Проверка на пустой массив

        timer += Time.deltaTime;
        if (timer >= frameDelay)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            rawImage.texture = gifFrames[currentFrame];
        }
    }

    void LoadGifFrames()
    {
        // Загрузка всех текстур из указанной папки
        gifFrames = Resources.LoadAll<Texture2D>(folderPath);
        if (gifFrames.Length == 0)
        {
            Debug.LogError("No textures found in folder: " + folderPath);
        }
    }
}