using UnityEngine;
[System.Serializable]
public class ColorToPrefab
{
    [SerializeField]private Color color;
    [SerializeField] private GameObject prefab;
    public Color GetColor()
    {
        return color;
    }
    public GameObject GetPrefab()
    {
        return prefab;
    }
}
