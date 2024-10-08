using UnityEngine;

public class RenderTextureScreenshot : MonoBehaviour
{
    public int width = 3840;
    public int height = 2160;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RenderTexture rt = new RenderTexture(width, height, 24);
            Camera.main.targetTexture = rt;
            Texture2D screenShot = new Texture2D(width, height, TextureFormat.RGB24, false);
            Camera.main.Render();
            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);
            byte[] bytes = screenShot.EncodeToPNG();
            string filename = string.Format("{0}/Screenshot_{1}x{2}_{3}.png",
                                             Application.dataPath,
                                             width, height,
                                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
            System.IO.File.WriteAllBytes(filename, bytes);
            Debug.Log("Screenshot saved: " + filename);
        }
    }
}