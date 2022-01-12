using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSettings : MonoBehaviour
{
    public GameObject npcTrigger;
    public GameObject xrRig;
    public GameObject npc;
    public Transform npcLocation;
    public Transform playerStartLocation;

    public bool EN = true;
    public bool FR = false;
    public bool CC = true;

    public void HandleInputData(int val)
    {
        if (val == 0)
        {
            EN = true;
            FR = false;
        }
        else if (val == 1)
        {
            FR = true;
            EN = false;
        }
        npcTrigger.GetComponent<BoxCollider>().enabled = true;
    }

    public void HandleToggleInput(bool active)
    {
        CC = active;
        npcTrigger.GetComponent<BoxCollider>().enabled = true;
    }

    public void StartGame()
    {
        xrRig.transform.position = playerStartLocation.position;
        xrRig.transform.rotation = playerStartLocation.rotation;
        npc.transform.position = npcLocation.position;
        npc.transform.rotation = npcLocation.rotation;

    }
}
