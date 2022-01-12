using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;
using TMPro;
using System;

public class AnimStateController : MonoBehaviour
{
    [Serializable]
    public class NPCAction
    {
        public string name;
        public AudioClip EN_audioClip;
        public AudioClip FR_audioClip;
        public string EN_text;
        public string FR_text;
        public string animation;
    }

    [SerializeField] public List<NPCAction> npcActions;
    [SerializeField] public List<NPCAction> tasks;
    [SerializeField] public List<NPCAction> pFeed;
    [SerializeField] public List<NPCAction> nFeed;
    [SerializeField] public List<NPCAction> toDoActions;
    public int numberOfToDoActions = 5;

    public GameObject language;
    private LanguageSettings languageSettingsScript;
    AudioSource audioSource;

    Animator animator;
    public bool talk = false;
    public bool command = false;
    public bool positive = false;
    public bool negative = false;

    GameObject npc;
    private TextMeshProUGUI text;
    private Canvas canvas;
    public Transform target;
    Vector3 canvasPos;

    string actionName;
    //string actionLanguage;
    AudioClip actionAudioClip;
    string actionText;
    string actionAnimation;

    public GameObject triggerZone;
    private TriggerZone triggerZoneScript;

    // Start is called before the first frame update
    void Start()
    {
        triggerZoneScript = triggerZone.GetComponent<TriggerZone>(); 
        languageSettingsScript = language.GetComponent<LanguageSettings>();
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        canvas = GetComponentInChildren<Canvas>();
        text = canvas.GetComponentInChildren<TextMeshProUGUI>();

        npc = this.gameObject;

        canvasPos = canvas.transform.position;
        canvas.enabled = false;

        tasks = new List<NPCAction>();
        pFeed = new List<NPCAction>();
        nFeed = new List<NPCAction>();
        toDoActions = new List<NPCAction>();
        
        setNPCActions();
    }

    // Update is called once per frame
    void Update()
    {
        followNPC(); // canvas follows npc character above the head
        if (!audioSource.isPlaying)
        {
            canvas.enabled = false;
            animator.Play("Sitting Idle");
            animator.SetBool("isTalking", false);
            animator.SetBool("isGivingTask", false);
            animator.SetBool("isNegative", false);
            animator.SetBool("isPositive", false);
        }
    }

    void followNPC()
    {
        canvasPos.x = npc.transform.position.x;
        canvasPos.y = npc.transform.position.y + 1.8f;
        canvasPos.z = npc.transform.position.z;
        canvas.transform.position = canvasPos; ;

        Vector3 direction = target.position - canvasPos;
        Quaternion rotation = Quaternion.LookRotation(direction);

        canvas.transform.rotation = rotation;
    }

    public void performAction(string name)
    {
        canvas.enabled = languageSettingsScript.CC;

        foreach (NPCAction action in npcActions)
        {
            if (action.name == name)
            {
                actionName = action.name;
                if (languageSettingsScript.FR)
                {
                    actionAudioClip = action.FR_audioClip;
                    actionText = action.FR_text;
                }
                else
                {
                    actionAudioClip = action.EN_audioClip;
                    actionText = action.EN_text;
                }
                actionAnimation = action.animation;
                break;
            }

        }

        enableAnimation(actionAnimation);
        audioSource.clip = actionAudioClip;
        text.SetText(actionText);
        audioSource.Play();
    }

    void enableAnimation(string name)
    {
        animator.Play("Sitting Idle");
        animator.SetBool("isTalking", false);
        animator.SetBool("isGivingTask", false);
        animator.SetBool("isNegative", false);
        animator.SetBool("isPositive", false);

        if (name == "talk")
        {
            animator.SetBool("isTalking", true);
            talk = true;
        }
        else if (name == "command")
        {
            animator.SetBool("isGivingTask", true);
            command = true;
        }
        else if (name == "positive")
        {
            animator.SetBool("isPositive", true);
            positive = true;
        }
        else if (name == "negative")
        {
            animator.SetBool("isNegative", true);
            negative = true;
        }
    }

    void setNPCActions()
    {
        foreach(NPCAction action in npcActions)
        {
            //TASKS
            if (action.name.Contains("task"))
            {
                if (!action.name.Contains("back"))
                {
                    tasks.Add(action);
                }   
            }

            if (action.name.Contains("first"))
            {
                tasks.Insert(0, action);
            }
            
            if (action.name.Contains("last"))
            {
                tasks.Insert(tasks.Count, action);
            }

            //POSITIVE
            if (action.name.Contains("pos"))
            {
                pFeed.Add(action);
            }

            //NEGATIVE
            if (action.name.Contains("neg"))
            {
                nFeed.Add(action);
            }
        }

        toDoActions.Add(tasks[0]); // add first task

        // add random tasks
        for (int i = 0; i < numberOfToDoActions - 2; i++)
        {
            bool found = false;
            NPCAction foundAction;
            while (!found)
            {
                int randomIndex = UnityEngine.Random.Range(1, tasks.Count - 1);
                NPCAction randomTask = tasks[randomIndex];
                string[] randomTaskInfo = randomTask.name.Split('-');
                bool isOk = true;

                if (!toDoActions.Contains(randomTask))
                {    
                    foreach (NPCAction toDo in toDoActions)
                    {
                        if (toDo.name.Contains(randomTaskInfo[2]))
                        {
                            isOk = false;
                            break;
                        }
                    }

                    if (isOk)
                    {
                        found = true;
                        foundAction = randomTask;
                        toDoActions.Add(foundAction);
                    }
                }
            }
        }

        // add last task
        toDoActions.Add(tasks[tasks.Count-1]);

        triggerZoneScript.currentTask = toDoActions[0].name;
    }

    public string findNextTask(string taskType)
    {
        int index = 0;
        string nextTaskName = "closing";

        foreach (NPCAction task in toDoActions)
        {
            if (task.name.Contains(taskType))
            {
                nextTaskName = task.name;
                break;
            }
            index++;
        }

        if (!nextTaskName.Contains("last"))
        {
            nextTaskName = toDoActions[index + 1].name;
        }
        else
        {
            nextTaskName = "closing";
        }

        return nextTaskName;
    }
}
