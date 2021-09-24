using UnityEngine;
using UnityEngine.UI;

public class FullScreenToggleHandler : MonoBehaviour
{
    [SerializeField] private GameObject checkbox;

    private void OnEnable()
    {
        ToggleCheckbox();
    }

    public void ToggleCheckbox()
    {
        if ((PlayerPrefs.GetInt("fullScreen") == 1))
        {
            checkbox.SetActive(true);
        }
        else
        {
            checkbox.SetActive(false);
        }
    }
}
