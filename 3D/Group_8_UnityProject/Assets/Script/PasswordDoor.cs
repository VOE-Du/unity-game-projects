using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PasswordDoor : MonoBehaviour
{
    [Header("Password Setting")]
    public string correctPassword = "8275";
    private string currentInput = "";

    [Header("Interaction settings")]
    public float interactionRange = 3f;
    public string interactionTag = "Player";
    public KeyCode interactKey = KeyCode.E;

    [Header("Door animation")]
    public Animation doorAnimation;
    public string openAnimationName = "DoorOpen";

    [Header("UI input panel")]
    public GameObject inputPanel;
    public Text inputDisplayText;

    [Header("Feedback settings")]
    public AudioSource audioSource;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    private bool isDoorOpened = false;
    private bool isInputActive = false;
    private Transform player;
    public GameObject mCube;

    void Start()
    {
        Debug.Log("PasswordDoor start");

        if (inputPanel != null)
        {
            inputPanel.SetActive(false);
            Debug.Log("InputPanel has been initialized.");
        }
        else
        {
            Debug.LogError("The inputPanel has not been assigned! Please drag in the InputPanel GameObject in the Inspector.");
        }

        // Find Player
        GameObject playerObj = GameObject.FindGameObjectWithTag(interactionTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log($"Find Player: {playerObj.name}");
        }
        else
        {
            Debug.LogError($"No player with the tag {interactionTag} was found! Please check the Tag setting of the player object.");
        }

        // Output component status
        Debug.Log($"doorAnimation: {(doorAnimation != null ? doorAnimation.name : "Unassigned")}");
        Debug.Log($"inputDisplayText: {(inputDisplayText != null ? "assigned" : "Unassigned")}");
        Debug.Log($"audioSource: {(audioSource != null ? "assigned" : "Unassigned")}");
    }

    void Update()
    {
        if (player != null && !isDoorOpened)
        {
            float dist = Vector3.Distance(transform.position, player.position);
            if (dist <= interactionRange)
            {
                Debug.Log($"The player is within range. Distance: {dist:F2}. Press the E key to open the door.");
            }
        }

        if (isDoorOpened) return;

        // Check if it is within the range and press the E key
        if (IsPlayerInRange() && Input.GetKeyDown(interactKey))
        {
            Debug.Log("After pressing the E key, the distance detection was successful.");
            ToggleInputPanel();
        }

        // Handle the input panel
        if (isInputActive)
        {
            HandleNumberInput();

            // Press ESC to close the panel.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Press ESC to close the panel.");
                CloseInputPanel();
            }
        }
    }

    bool IsPlayerInRange()
    {
        if (player == null)
        {
            Debug.LogWarning("Player reference is empty");
            return false;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        bool inRange = distance <= interactionRange;

        if (inRange)
        {
            Debug.Log($"Player within range, distance: {distance:F2}");
        }

        return inRange;
    }

    void ToggleInputPanel()
    {
        if (isInputActive)
            CloseInputPanel();
        else
            OpenInputPanel();
    }

    void OpenInputPanel()
    {
        Debug.Log("Open the input panel");
        isInputActive = true;
        currentInput = "";
        UpdateInputDisplay();

        if (inputPanel != null)
        {
            inputPanel.SetActive(true);
            Debug.Log("InputPanel activated");
        }
        else
        {
            Debug.LogError("Unable to open the panel: inputPanel is null");
        }
    }

    void CloseInputPanel()
    {
        Debug.Log("Close the input panel");
        isInputActive = false;
        currentInput = "";

        if (inputPanel != null)
            inputPanel.SetActive(false);
    }

    void HandleNumberInput()
    {
        // Input a number - Use a more lenient detection
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                Debug.Log($"Pressed the number key: {i}");
                AddDigit(i);
                return;
            }
        }

        // Delete
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Debug.Log("Pressed the delete key");
            if (currentInput.Length > 0)
            {
                currentInput = currentInput.Remove(currentInput.Length - 1);
                UpdateInputDisplay();
                Debug.Log($"After deletion, input: '{currentInput}'");
            }
        }
    }

    void AddDigit(int digit)
    {
        if (currentInput.Length >= 4)
        {
            Debug.Log("The input has reached the 4-digit limit.");
            return;
        }

        currentInput += digit.ToString();
        UpdateInputDisplay();
        Debug.Log($"Add the digit {digit}, current input: '{currentInput}', length: {currentInput.Length}");

        // Automatic verification upon entering 4 digits or more.
        if (currentInput.Length == 4)
        {
            Debug.Log("4 digits have been entered. Verification begins...");
            CheckPassword();
        }
    }

    void UpdateInputDisplay()
    {
        if (inputDisplayText != null)
        {
            string display = currentInput.PadRight(4, '_');
            inputDisplayText.text = display;
            Debug.Log($"Update display: '{display}'");
        }
        else
        {
            Debug.LogError("The inputDisplayText is null, so the display cannot be updated.");
        }
    }

    void CheckPassword()
    {
        Debug.Log($"Verify Password");
        Debug.Log($"The input password: '{currentInput}'");
        Debug.Log($"The correct password: '{correctPassword}'");
        Debug.Log($"Are they equal: {currentInput == correctPassword}");

        if (currentInput == correctPassword)
        {
            Debug.Log("Password is correct! The door has been opened.");
            OpenDoor();
        }
        else
        {
            Debug.Log("Incorrect password!");
            if (audioSource != null && wrongSound != null)
            {
                audioSource.PlayOneShot(wrongSound);
                Debug.Log("Play the incorrect sound effect.");
            }

            // Clear the input and allow the player to re-enter.
            currentInput = "";
            UpdateInputDisplay();
        }
    }

    void OpenDoor()
    {
        Debug.Log("Perform the opening operation.");
        isDoorOpened = true;
        CloseInputPanel();

        // Play the animation
        if (doorAnimation != null && !string.IsNullOrEmpty(openAnimationName))
        {
            doorAnimation.Play(openAnimationName);
            Debug.Log($"Play animation: {openAnimationName}");
        }
        else
        {
            Debug.LogWarning($"Unable to play the animation: doorAnimation={doorAnimation != null}, openAnimationName='{openAnimationName}'");
        }

        // Play sound effect
        if (audioSource != null && correctSound != null)
        {
            audioSource.PlayOneShot(correctSound);
            Debug.Log("Play the correct sound effect");
        }

        // Activate the object
        if (mCube != null)
        {
            mCube.SetActive(true);
            Debug.Log($"Activate mCube: {mCube.name}");
        }
        else
        {
            Debug.Log("mCube is not assigned a value, so skip the activation process.");
        }
    }

    // Visual debugging
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        if (IsPlayerInRange())
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, interactionRange);
        }
    }
}