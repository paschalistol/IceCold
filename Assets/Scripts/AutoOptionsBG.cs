using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoOptionsBG : MonoBehaviour
{
    [SerializeField] private GameObject optionsBG;
    private void OnEnable()
    {
        optionsBG.SetActive(true);
    }
    private void OnDisable()
    {
        if (optionsBG != null)
        {

        optionsBG.SetActive(false);
        }
    }
}
