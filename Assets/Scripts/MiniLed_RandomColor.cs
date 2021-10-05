using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniLed_RandomColor : MonoBehaviour
{
    private Light thisLight;
    [SerializeField] private Animator animator;

    private void OnEnable()
    {
        if (thisLight == null)
        {
            thisLight = GetComponent<Light>();
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MiniLeds_2anim"))
        {
            thisLight.color = Color.red;
        }
        else
        {
            switch (Random.Range(0, 4))
            {
                case 0:
                    thisLight.color = Color.red;
                    break;
                case 1:
                    thisLight.color = Color.green;
                    break;
                case 2:
                    thisLight.color = Color.magenta;
                    break;
                case 3:
                    thisLight.color = Color.yellow;
                    break;
            }
                
        }
    }
}
