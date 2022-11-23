using UnityEngine;

public class PhotoCapture : MonoBehaviour
{
    private static PhotoCapture _instance;

    public static PhotoCapture instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.

            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PhotoCapture>();
            }
            return _instance;
        }
    }

    public Camera captureCamera;

    public Texture2D MakeScrenshot()
    {
        if (captureCamera == null)
            captureCamera = GameObject.FindWithTag("Screenshot Camera").GetComponent<Camera>();
        int width = this.captureCamera.pixelWidth;
        int height = this.captureCamera.pixelHeight;
        Texture2D texture = new Texture2D(width, height);

        RenderTexture targetTexture = RenderTexture.GetTemporary(width, height);

        this.captureCamera.targetTexture = targetTexture;
        this.captureCamera.Render();

        RenderTexture.active = targetTexture;

        Rect rect = new Rect(0, 0, width, height);
        texture.ReadPixels(rect, 0, 0);
        texture.Apply();
        return texture;
    }

}
