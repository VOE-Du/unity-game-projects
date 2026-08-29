using UnityEngine;
using UnityEngine.UI;

public class SimplePickup : MonoBehaviour
{
    public GameObject hiddenObject;  // The hidden object to be displayed
    public Text tipText;             // Hint text

    private bool isNear = false;

    void Start()
    {
        hiddenObject.SetActive(false);
        tipText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isNear && Input.GetKeyDown(KeyCode.E))
        {
            hiddenObject.SetActive(true);
            tipText.gameObject.SetActive(false);
            Destroy(gameObject);
            Destroy(this); // Pick it up and delete the script
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isNear = true;
            tipText.text = "This might be useful! \r\nPress button E.";
            tipText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isNear = false;
            tipText.gameObject.SetActive(false);
        }
    }
}