using UnityEngine;

public class BookPickupSimple : MonoBehaviour
{
    public string bookName;

    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed the E key");
            TryPickup();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("riggered entry, encountered: " + other.name + ", Tag: " + other.tag);

        if (other.tag == "Player")
        {
            isPlayerNear = true;
            Debug.Log("Player approaches: " + bookName + " Press 'E' to pick up");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isPlayerNear = false;
            Debug.Log("Player leaves: " + bookName);
        }
    }

    void TryPickup()
    {
        Debug.Log("1. TryPickup has been invoked");

        BookInventorySimple inv = FindObjectOfType<BookInventorySimple>();
        Debug.Log("2. Found Inventory£º" + (inv != null));

        if (inv != null)
        {
            Debug.Log("3. Ready to pick up: " + bookName);
            bool success = inv.PickupBook(bookName);
            Debug.Log("4. Pick-up result: " + success);

            if (success)
            {
                gameObject.SetActive(false);
                Debug.Log("5. The book has disappeared.");
            }
            else
            {
                Debug.Log("5. Pickup failed. The book did not disappear.");
            }
        }
        else
        {
            Debug.Log("Error: Unable to find the BookInventorySimple script on the GameManager!");
        }
    }
}