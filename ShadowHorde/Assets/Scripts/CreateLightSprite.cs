using UnityEngine;

public class CreateLightSprite : MonoBehaviour
{
    [ContextMenu("Generate Light Texture")]
    void Generate()
    {
        int size = 256;
        Texture2D tex = new Texture2D(size, size);
        Vector2 center = new Vector2(size / 2f, size / 2f);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                float dist = Vector2.Distance(new Vector2(x, y), center);
                float alpha = Mathf.Clamp01(1f - (dist / (size / 2f)));
                alpha = Mathf.Pow(alpha, 2f); // suaviza as bordas
                tex.SetPixel(x, y, new Color(1f, 0.9f, 0.6f, alpha));
            }
        }

        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(
            Application.dataPath + "/Sprites/LightGradient.png", bytes
        );
        Debug.Log("Texture saved!");
    }
}