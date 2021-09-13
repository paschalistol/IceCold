using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class HoleGeneratorClassic : MonoBehaviour
{
    [SerializeField] private Texture2D map;
    [SerializeField] private Image holeMap;
    [SerializeField] private ColorToPrefab[] colorMappings;
    private RectTransform canvas;
    private float holeMapScale;
    [SerializeField] private GameObject holeNumberPrefab;
    [SerializeField] private GameObject panelObject;
    private int bonusNumber = 0;

    void Start()
    {
        canvas = holeMap.transform.parent.transform.parent.GetComponent<RectTransform>();
        holeMapScale = holeMap.GetComponent<RectTransform>().localScale.x;
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        for (int y = 0; y < map.height; y++)
        {
            for (int x = 0; x < map.width; x++)
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
                GameObject hole = Instantiate(colorMapping.GetPrefab(),position, Quaternion.identity, transform);
                if (colorMapping.IsBonus())
                {
                    Vector3 numberPosition = position;
                    numberPosition.y += 0.065f;
                    Debug.Log(numberPosition);
                    //GameObject holeNumber = Instantiate(holeNumberPrefab, numberPosition, Quaternion.identity, panelObject.transform);
                    //holeNumber.GetComponent<TMP_Text>().SetText("-" + ++bonusNumber + "-");
                }
            }
        }
    }
}
