using UnityEngine;
using UnityEngine.UI;

public class IDCardDisplay : MonoBehaviour
{
    public GameObject mUI;
    public GameObject mText;

    private bool isPlayerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            mText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            mText.SetActive(false);
            mUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            mUI.SetActive(!mUI.activeSelf);
        }
    }
}