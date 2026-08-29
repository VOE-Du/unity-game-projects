using UnityEngine;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour
{
    public GameObject backpackPanel;
    public KeyCode toggleKey = KeyCode.Tab;

    public Image imageSlot1;
    public Image imageSlot2;
    public Image imageSlot3;
    public Text textSlot1;
    public Text textSlot2;
    public Text textSlot3;

    public Sprite novelSprite;
    public Sprite mathSprite;
    public Sprite historySprite;

    private BookInventorySimple inventory;
    private bool isOpen = false;

    void Start()
    {
        inventory = FindObjectOfType<BookInventorySimple>();
        backpackPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (isOpen) CloseBackpack();
            else OpenBackpack();
        }
    }

    void OpenBackpack()
    {
        isOpen = true;
        RefreshBackpack();
        backpackPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseBackpack()
    {
        isOpen = false;
        backpackPanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RefreshBackpack()
    {
        if (inventory == null) return;

        // Fiction
        if (inventory.HasNovel())
        {
            if (imageSlot1 != null)
            {
                imageSlot1.sprite = novelSprite;
                imageSlot1.gameObject.SetActive(true);
            }
            if (textSlot1 != null) textSlot1.text = "1";
        }
        else
        {
            if (imageSlot1 != null) imageSlot1.gameObject.SetActive(false);
            if (textSlot1 != null) textSlot1.text = "0";
        }

        // Math
        if (inventory.HasMath())
        {
            if (imageSlot2 != null)
            {
                imageSlot2.sprite = mathSprite;
                imageSlot2.gameObject.SetActive(true);
            }
            if (textSlot2 != null) textSlot2.text = "1";
        }
        else
        {
            if (imageSlot2 != null) imageSlot2.gameObject.SetActive(false);
            if (textSlot2 != null) textSlot2.text = "0";
        }

        // History
        if (inventory.HasHistory())
        {
            if (imageSlot3 != null)
            {
                imageSlot3.sprite = historySprite;
                imageSlot3.gameObject.SetActive(true);
            }
            if (textSlot3 != null) textSlot3.text = "1";
        }
        else
        {
            if (imageSlot3 != null) imageSlot3.gameObject.SetActive(false);
            if (textSlot3 != null) textSlot3.text = "0";
        }
    }
}