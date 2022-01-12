using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.UI;
using TMPro;

public class ObjectWord : MonoBehaviour
{
    public GameObject language;
    private LanguageSettings languageSettingsScript;
    GameObject item;
    string itemName;
    private TextMeshProUGUI text;
    private Canvas canvas;
    public Transform target;
    Vector3 canvasPos;

    public AudioClip EN_audioClip;
    public AudioClip FR_audioClip;
    public string EN_word;
    public string FR_word;
    AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        languageSettingsScript = language.GetComponent<LanguageSettings>();
        audioSource = GetComponent<AudioSource>();
        canvas = GetComponentInChildren<Canvas>();
        text = canvas.GetComponentInChildren<TextMeshProUGUI>();

        item = this.gameObject;
        itemName = item.name;
        text.SetText(itemName);

        canvasPos = canvas.transform.position;
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        canvasPos.x = item.transform.position.x;
        canvasPos.y = item.transform.position.y + 0.5f;
        canvasPos.z = item.transform.position.z;
        canvas.transform.position = canvasPos; ;

        Vector3 direction = target.position - canvasPos;
        Quaternion rotation = Quaternion.LookRotation(direction);

        canvas.transform.rotation = rotation;

        //setLanguage();

    }

    public void toggleText()
    {
        if (languageSettingsScript.CC)
        {
            canvas.enabled = !canvas.enabled;
        }
        setLanguage();
    }

    public void setLanguage()
    {
        if (languageSettingsScript.FR)
        {
            itemName = FR_word;
            audioSource.clip = FR_audioClip;
        }
        else if (!languageSettingsScript.FR)
        {
            itemName = EN_word;
            audioSource.clip = EN_audioClip;
        }

        text.SetText(itemName);
    }
}
