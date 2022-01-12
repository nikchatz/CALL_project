using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcStartTrigger : MonoBehaviour
{
    public GameObject npc;
    private AnimStateController npcScript;

    // Start is called before the first frame update
    void Start()
    {
        npcScript = npc.GetComponent<AnimStateController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "XR Rig" && this.gameObject.name == "npcDialogTrigger")
        {
            npcScript.performAction("opening");
            this.GetComponent<BoxCollider>().enabled = false;
        }
        
        if (other.gameObject.name == "XR Rig" && this.gameObject.name == "GameStartTrigger")
        {
            npcScript.performAction("first-1-hamburger");
            this.GetComponent<BoxCollider>().enabled = false;
        }
        
    }
}
