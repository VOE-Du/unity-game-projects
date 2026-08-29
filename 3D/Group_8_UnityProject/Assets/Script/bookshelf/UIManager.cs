using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Top text")]
    public Text statusText;
    public string defaultMessage = "The books have not been fully returned.";
    public string completeMessage = "The books on the 8th bookshelf are all present and complete.";

    [Header("Updated Text (which can be manually modified) as of the latest update")]
    public Text targetCompleteText;
    public string targetCompleteMessage = "Decryption successful!";

    [Header("Three buttons on the first layer")]
    public Button btn1_Novel;
    public Button btn1_Math;
    public Button btn1_History;
    public Text txt1_Novel;
    public Text txt1_Math;
    public Text txt1_History;

    [Header("Three buttons on the second layer")]
    public Button btn2_Novel;
    public Button btn2_Math;
    public Button btn2_History;
    public Text txt2_Novel;
    public Text txt2_Math;
    public Text txt2_History;

    [Header("Three buttons on the third layer")]
    public Button btn3_Novel;
    public Button btn3_Math;
    public Button btn3_History;
    public Text txt3_Novel;
    public Text txt3_Math;
    public Text txt3_History;

    [Header("sound effect")]
    public AudioClip placeSound;
    public AudioClip wrongSound;
    public AudioClip completeSound;

    private BookInventorySimple inventory;
    private AudioSource audioSource;
    private bool hasCompleted = false;

    void Start()
    {
        inventory = FindObjectOfType<BookInventorySimple>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        HideAllButtons();
        BindButtons();
        UpdateStatusText();
    }

    void BindButtons()
    {
        btn1_Novel.onClick.AddListener(() => OnButtonClick(1, "Fiction"));
        btn1_Math.onClick.AddListener(() => OnButtonClick(1, "Math"));
        btn1_History.onClick.AddListener(() => OnButtonClick(1, "History"));

        btn2_Novel.onClick.AddListener(() => OnButtonClick(2, "Fiction"));
        btn2_Math.onClick.AddListener(() => OnButtonClick(2, "Math"));
        btn2_History.onClick.AddListener(() => OnButtonClick(2, "History"));

        btn3_Novel.onClick.AddListener(() => OnButtonClick(3, "Fiction"));
        btn3_Math.onClick.AddListener(() => OnButtonClick(3, "Math"));
        btn3_History.onClick.AddListener(() => OnButtonClick(3, "History"));
    }

    public void HideAllButtons()
    {
        btn1_Novel.gameObject.SetActive(false);
        btn1_Math.gameObject.SetActive(false);
        btn1_History.gameObject.SetActive(false);

        btn2_Novel.gameObject.SetActive(false);
        btn2_Math.gameObject.SetActive(false);
        btn2_History.gameObject.SetActive(false);

        btn3_Novel.gameObject.SetActive(false);
        btn3_Math.gameObject.SetActive(false);
        btn3_History.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowButtonsForLayer(int layerNumber)
    {
        if (inventory.IsLayerCompleted(layerNumber))
        {
            Debug.Log("The" + layerNumber + "layer has already been completed!");
            return;
        }

        HideAllButtons();

        switch (layerNumber)
        {
            case 1:
                btn1_Novel.gameObject.SetActive(true);
                btn1_Math.gameObject.SetActive(true);
                btn1_History.gameObject.SetActive(true);
                UpdateButtonTexts(1);
                break;
            case 2:
                btn2_Novel.gameObject.SetActive(true);
                btn2_Math.gameObject.SetActive(true);
                btn2_History.gameObject.SetActive(true);
                UpdateButtonTexts(2);
                break;
            case 3:
                btn3_Novel.gameObject.SetActive(true);
                btn3_Math.gameObject.SetActive(true);
                btn3_History.gameObject.SetActive(true);
                UpdateButtonTexts(3);
                break;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void UpdateButtonTexts(int layer)
    {
        switch (layer)
        {
            case 1:
                txt1_Novel.text = inventory.HasNovel() ? "Fiction ✓" : "Fiction ✗";
                txt1_Math.text = inventory.HasMath() ? "Math ✓" : "Math ✗";
                txt1_History.text = inventory.HasHistory() ? "History ✓" : "History ✗";
                break;
            case 2:
                txt2_Novel.text = inventory.HasNovel() ? "Fiction ✓" : "Fiction ✗";
                txt2_Math.text = inventory.HasMath() ? "Math ✓" : "Math ✗";
                txt2_History.text = inventory.HasHistory() ? "History ✓" : "History ✗";
                break;
            case 3:
                txt3_Novel.text = inventory.HasNovel() ? "Fiction ✓" : "Fiction ✗";
                txt3_Math.text = inventory.HasMath() ? "Math ✓" : "Math ✗";
                txt3_History.text = inventory.HasHistory() ? "History ✓" : "History ✗";
                break;
        }
    }

    void OnButtonClick(int layerNumber, string bookName)
    {
        Debug.Log($"Click：the {layerNumber} Layer，put the {bookName} book away");

        bool success = inventory.PlaceBookAtLayer(layerNumber, bookName);

        if (success)
        {
            if (placeSound != null) audioSource.PlayOneShot(placeSound);
            HideAllButtons();
            UpdateStatusText();

            BackpackUI backpack = FindObjectOfType<BackpackUI>();
            if (backpack != null) backpack.RefreshBackpack();
        }
        else
        {
            if (wrongSound != null) audioSource.PlayOneShot(wrongSound);
        }
    }

    public void UpdateStatusText()
    {
        if (statusText == null) return;

        if (inventory.IsAllBooksReturned())
        {
            statusText.text = completeMessage;

            if (!hasCompleted)
            {
                hasCompleted = true;
                OnAllBooksCompleted();
            }
        }
        else
        {
            statusText.text = defaultMessage;
        }
    }

    void OnAllBooksCompleted()
    {
        Debug.Log("All the books have been returned!");

        // 1. Update Target Text
        if (targetCompleteText != null)
        {
            targetCompleteText.text = targetCompleteMessage;
            Debug.Log($"The target text has been updated to: {targetCompleteMessage}");
        }

        // 2. Play the completion sound effect
        if (completeSound != null)
        {
            audioSource.PlayOneShot(completeSound);
        }

        // 3. Turn off all Outline illumination (search for all Outlines in the scene directly)
        Outline[] allOutlines = FindObjectsOfType<Outline>();
        foreach (Outline outline in allOutlines)
        {
            outline.enabled = false;
            Debug.Log($"Close Outline：{outline.gameObject.name}");
        }

        // 4. Also invoke the method of Inventory
        if (inventory != null)
        {
            inventory.DisableAllOutlines();
        }

        Debug.Log("All the lights have been turned off.");
    }
}