using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableButtonPusher : MonoBehaviour
{
    public GameObject R_buttonPusher;
    public GameObject L_buttonPusher;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == L_buttonPusher || other.gameObject == R_buttonPusher)
        {
            other.isTrigger = false;
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == L_buttonPusher || other.gameObject == R_buttonPusher)
        {
            other.isTrigger = true;
        }
    }
}
