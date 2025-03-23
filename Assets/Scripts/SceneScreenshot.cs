using UnityEngine;
using UnityEditor;
using System.IO;

public class SceneScreenshot : EditorWindow
{
    private int width = 1920; // Ширина скриншота
    private int height = 1080; // Высота скриншота
    private int superSampling = 2; // Коэффициент Super Sampling
    private string folderPath = "Screenshots"; // Папка для сохранения скриншотов

    [MenuItem("Tools/Take Scene Screenshot")]
    public static void ShowWindow()
    {
        GetWindow<SceneScreenshot>("Scene Screenshot");
    }

    void OnGUI()
    {
        GUILayout.Label("Настройки скриншота", EditorStyles.boldLabel);

        width = EditorGUILayout.IntField("Ширина", width);
        height = EditorGUILayout.IntField("Высота", height);
        superSampling = EditorGUILayout.IntField("Super Sampling", superSampling);
        folderPath = EditorGUILayout.TextField("Папка для сохранения", folderPath);

        if (GUILayout.Button("Сделать скриншот"))
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        // Создаем папку, если её нет
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Получаем текущий вид из окна Scene
        Camera sceneCamera = SceneView.lastActiveSceneView.camera;

        // Вычисляем разрешение для рендеринга с учетом Super Sampling
        int renderWidth = width * superSampling;
        int renderHeight = height * superSampling;

        // Создаем RenderTexture с увеличенным разрешением
        RenderTexture renderTexture = new RenderTexture(renderWidth, renderHeight, 24);
        sceneCamera.targetTexture = renderTexture;

        // Рендерим кадр в текстуру
        Texture2D screenshot = new Texture2D(renderWidth, renderHeight, TextureFormat.RGB24, false);
        sceneCamera.Render();
        RenderTexture.active = renderTexture;
        screenshot.ReadPixels(new Rect(0, 0, renderWidth, renderHeight), 0, 0);
        screenshot.Apply();

        // Уменьшаем текстуру до целевого разрешения
        Texture2D resizedScreenshot = ResizeTexture(screenshot, width, height);

        // Сохраняем текстуру в файл
        byte[] bytes = resizedScreenshot.EncodeToPNG();
        string filename = "SceneScreenshot_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string path = Path.Combine(folderPath, filename);
        File.WriteAllBytes(path, bytes);

        Debug.Log("Скриншот сохранен: " + path);

        // Очищаем ресурсы
        sceneCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(renderTexture);
        DestroyImmediate(screenshot);
        DestroyImmediate(resizedScreenshot);
    }

    Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
        rt.filterMode = FilterMode.Bilinear;
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D result = new Texture2D(newWidth, newHeight, TextureFormat.RGB24, false);
        result.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
        result.Apply();
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }
}