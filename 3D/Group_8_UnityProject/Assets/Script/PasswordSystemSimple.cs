using UnityEngine;
using UnityEngine.UI;

public class PasswordSystemSimple : MonoBehaviour
{
    [Header("Password Authentification")]
    [SerializeField] private string correctPassword = "1027";

    [Header("Update on clues")]
    [SerializeField] private string clueUpdateString = "D=7";//"D=7"

    [Header("UI×é¼þ")]
    [SerializeField] private InputField passwordInputField;
    [SerializeField] private Text textA;   // "Lucky Number 7"
    [SerializeField] private Text textB;   // "Hint"
    [SerializeField] private Text textC;   // "Clue"
    [SerializeField] private GameObject desktopImagePanel;

    private bool isUnlocked = false;

    void Start()
    {
        if (desktopImagePanel != null)
            desktopImagePanel.SetActive(false);

        if (textA != null)
            textA.text = "";

        if (textB != null)
            textB.text = "";

        if (textC != null)
            textC.text = "Clue 4";

        if (passwordInputField != null)
        {
            passwordInputField.onEndEdit.AddListener(OnPasswordEnter);
        }
    }

    void OnPasswordEnter(string input)
    {
        if (isUnlocked) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (input == correctPassword)
            {
                Unlock();
            }
            else
            {
                ShowError();
            }
        }
    }

    void Unlock()
    {
        isUnlocked = true;

        // display picture
        if (desktopImagePanel != null)
            desktopImagePanel.SetActive(true);

        // Text A: Displays "Lucky Number 7"
        if (textA != null)
        {
            textA.text = "Lucky number 7";
            textA.color = new Color32(184, 134, 11, 255);
        }

        // Text B: Clear error message (or keep it unchanged)
        if (textB != null)
        {
            textB.text = "";
        }

        // Text C: Displays the updated content of the manually modified clues
        if (textC != null)
        {
            textC.text = clueUpdateString;
        }

        // Disable the input box
        passwordInputField.interactable = false;

        Debug.Log("Password correct! Update of clues: " + clueUpdateString);
    }

    void ShowError()
    {
        // Text B: Display error message
        if (textB != null)
        {
            textB.text = "The password might be related to the principal himself.";
            textB.color = Color.red;
        }

        // Text A: Clear (without displaying the number 7)
        if (textA != null)
        {
            textA.text = "";
        }

        // Clear the password input field and allow the user to re-enter it.
        passwordInputField.text = "";
        passwordInputField.Select();
        passwordInputField.ActivateInputField();
    }
}