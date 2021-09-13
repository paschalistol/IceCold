using UnityEngine;
[System.Serializable]
public class ColorToPrefab
{
    [SerializeField]private Color color;
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool bonus = false;
    public Color GetColor()
    {
        return color;
    }
    public GameObject GetPrefab()
    {
        return prefab;
    }
    public bool IsBonus()
    {
        return bonus;
    }
}
