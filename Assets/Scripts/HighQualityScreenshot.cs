using UnityEngine;
using System.IO;

public class HighQualityScreenshot : MonoBehaviour
{
    public Camera screenshotCamera; // Камера, с которой будет сделан скриншот
    public int width = 1920; // Ширина скриншота (целевое разрешение)
    public int height = 1080; // Высота скриншота (целевое разрешение)
    public int superSampling = 2; // Коэффициент Super Sampling (например, 2 для рендеринга в 2 раза больше)

    void Update()
    {
        // Пример: делаем скриншот при нажатии клавиши "P"
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        // Проверяем, задана ли камера
        if (screenshotCamera == null)
        {
            Debug.LogError("Камера не назначена!");
            return;
        }

        // Вычисляем разрешение для рендеринга с учетом Super Sampling
        int renderWidth = width * superSampling;
        int renderHeight = height * superSampling;

        // Создаем RenderTexture с увеличенным разрешением
        RenderTexture renderTexture = new RenderTexture(renderWidth, renderHeight, 24);
        screenshotCamera.targetTexture = renderTexture; // Назначаем RenderTexture камере

        // Рендерим кадр в текстуру
        Texture2D screenshot = new Texture2D(renderWidth, renderHeight, TextureFormat.RGB24, false);
        screenshotCamera.Render(); // Рендерим кадр
        RenderTexture.active = renderTexture; // Устанавливаем активный RenderTexture
        screenshot.ReadPixels(new Rect(0, 0, renderWidth, renderHeight), 0, 0); // Читаем пиксели из RenderTexture
        screenshot.Apply(); // Применяем изменения

        // Уменьшаем текстуру до целевого разрешения
        Texture2D resizedScreenshot = ResizeTexture(screenshot, width, height);

        // Сохраняем текстуру в файл
        byte[] bytes = resizedScreenshot.EncodeToPNG(); // Кодируем текстуру в PNG
        string filename = "Screenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string path = Path.Combine(Application.dataPath, filename); // Путь для сохранения
        File.WriteAllBytes(path, bytes); // Сохраняем файл

        Debug.Log("Скриншот сохранен: " + path);

        // Очищаем ресурсы
        screenshotCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);
        Destroy(screenshot);
        Destroy(resizedScreenshot);
    }

    // Метод для уменьшения текстуры
    Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        // Создаем временный RenderTexture для уменьшения текстуры
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Bilinear; // Используем билинейную фильтрацию для сглаживания

        // Копируем исходную текстуру в RenderTexture с новым размером
        Graphics.Blit(source, rt);

        // Читаем пиксели из RenderTexture
        RenderTexture.active = rt;
        Texture2D result = new Texture2D(newWidth, newHeight, TextureFormat.RGB24, false);
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();

        // Освобождаем временный RenderTexture
        RenderTexture.ReleaseTemporary(rt);

        return result;
    }
}