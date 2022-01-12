using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> areaObjects;
    public string currentTask;
    public GameObject npc;
    private AnimStateController npcScript;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        areaObjects = new List<GameObject>();
        npcScript = npc.GetComponent<AnimStateController>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!areaObjects.Contains(other.gameObject))
        {
            areaObjects.Add(other.gameObject);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (areaObjects.Contains(other.gameObject))
        {
            areaObjects.Remove(other.gameObject);
        }
    }

    public void CheckAnswer()
    {
        string[] taskInfo = currentTask.Split('-');
        string randomPfeed = npcScript.pFeed[Random.Range(0, npcScript.pFeed.Count)].name;
        string randomNfeed = npcScript.nFeed[Random.Range(0, npcScript.nFeed.Count)].name;

        if (taskInfo.Length > 0)
        {
            int numberOfItems = int.Parse(taskInfo[1]);
            string itemType = taskInfo[2];

            int itemCounter = 0;

            if (areaObjects.Count == numberOfItems)
            {
                foreach (GameObject item in areaObjects)
                {
                    if (item.name.Contains(itemType))
                    {
                        itemCounter++;
                    }
                }

                Debug.Log("Items on counter: " +itemCounter);
                if (itemCounter == numberOfItems)
                {
                    audioSource.Play();
                    foreach (GameObject item in areaObjects)
                    {
                        Destroy(item.gameObject);
                    }
                    areaObjects.Clear();

                    //play pos
                    npcScript.performAction(randomPfeed);

                    string nextTaskName = npcScript.findNextTask(taskInfo[2]);
                    Debug.Log("next task name: " +nextTaskName);
                    StartCoroutine(prepareNextTask(nextTaskName, 4f));

                    
                    /*npcScript.performAction(nextTaskName);
                    taskInfo = nextTaskName.Split('-');*/
                }
                else
                {
                    //play neg
                    npcScript.performAction(randomNfeed);
                    StartCoroutine(repeatTask(4f));
                }
            }
            else
            {
                //play neg
                npcScript.performAction(randomNfeed);
                StartCoroutine(repeatTask(4f));
            }
        }
        
    }

    IEnumerator prepareNextTask(string taskName, float secDelay)
    {
        yield return new WaitForSeconds(secDelay);

        npcScript.performAction(taskName);
        currentTask = taskName;
    }
    IEnumerator repeatTask(float secDelay)
    {
        yield return new WaitForSeconds(secDelay);

        npcScript.performAction(currentTask);
    }

}
