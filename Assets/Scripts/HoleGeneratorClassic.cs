using UnityEngine;
using UnityEngine.UI;

public class HoleGeneratorClassic : MonoBehaviour
{
    [SerializeField] private Texture2D map;
    [SerializeField] private Image holeMap;
    [SerializeField] private ColorToPrefab[] colorMappings;
    private RectTransform canvas;
    private float holeMapScale;

    void Start()
    {
        canvas = holeMap.transform.parent.transform.parent.GetComponent<RectTransform>();
        holeMapScale = holeMap.GetComponent<RectTransform>().localScale.x;
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        
        for (int x = 0; x < map.width; x++)
        {
            for (int y = 0; y < map.height; y++)
            {
                GenerateHoles(x,y);
            }
        }
    }

    private void GenerateHoles(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);
        if (pixelColor.a == 0)
        {
            //the pixel is transparent
            return;
        }
        foreach (ColorToPrefab colorMapping in colorMappings)
        {
            if (colorMapping.GetColor().r.Equals(pixelColor.r) || colorMapping.GetColor().g.Equals(pixelColor.g) || colorMapping.GetColor().b.Equals(pixelColor.b))
            {
                Vector3 position = new Vector3(x, y, 0) * canvas.localScale.x / holeMapScale;

                position.x += -3.1f;
                position.y += -4.85f;
                Instantiate(colorMapping.GetPrefab(),position, Quaternion.identity, transform);
            }
        }
    }
}
