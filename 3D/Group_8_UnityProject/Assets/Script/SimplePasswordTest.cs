using UnityEngine;
using UnityEngine.UI;

public class SimplePasswordTest : MonoBehaviour
{
    public InputField passwordInput;
    public Text resultText;
    public GameObject imagePanel;

    private string correctPassword = "1027";

    void Start()
    {
        if (imagePanel != null)
            imagePanel.SetActive(false);

        if (resultText != null)
            resultText.text = "Waiting for input...";
    }

    // This method is used to invoke the button.
    public void CheckPassword()
    {
        if (passwordInput.text == correctPassword)
        {
            resultText.text = "Lucky number 7";
            resultText.color = Color.green;

            if (imagePanel != null)
                imagePanel.SetActive(true);

            Debug.Log("Password is correct£¡");
        }
        else
        {
            resultText.text = "The password might be related to the principal himself.";
            resultText.color = Color.red;
            passwordInput.text = "";
            Debug.Log("Wrong Password");
        }
    }
}