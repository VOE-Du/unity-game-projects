using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerText : MonoBehaviour
{

    public Text tipText;              // Hint text (drag the Text component in the scene)
    [TextArea] public string tipMessage = "It seems there is something on the blackboard.";

    void Start()
    {
        tipText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            tipText.text = tipMessage;  // Use modifiable variables
            tipText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            tipText.gameObject.SetActive(false);
        }
    }
}