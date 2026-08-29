using UnityEngine;

public class BookInventorySimple : MonoBehaviour
{
    [Header("Book Title Configuration")]
    public string bookNameLayer1 = "Fiction";
    public string bookNameLayer2 = "History";
    public string bookNameLayer3 = "Math";

    public bool hasNovel = false;
    public bool hasMath = false;
    public bool hasHistory = false;

    private bool layer1Completed = false;
    private bool layer2Completed = false;
    private bool layer3Completed = false;

    public GameObject outlineLayer1;
    public GameObject outlineLayer2;
    public GameObject outlineLayer3;

    public Material newMaterialLayer1;
    public Material newMaterialLayer2;
    public Material newMaterialLayer3;

    void Start()
    {
        if (outlineLayer1 != null)
        {
            SetOutlineActive(1, true);
        }
        if (outlineLayer2 != null)
        {
            SetOutlineActive(2, true);
        }
        if (outlineLayer3 != null)
        {
            SetOutlineActive(3, true);
        }
    }

    public bool PickupBook(string bookName)
    {
        Debug.Log($"PickupBook receive：'{bookName}'.");

        if (bookName == bookNameLayer1 && !hasNovel)
        {
            hasNovel = true;
            Debug.Log($"Picked up：{bookName}.");
            return true;
        }
        else if (bookName == bookNameLayer2 && !hasHistory)
        {
            hasHistory = true;
            Debug.Log($"Picked up：{bookName}.");
            return true;
        }
        else if (bookName == bookNameLayer3 && !hasMath)
        {
            hasMath = true;
            Debug.Log($"Picked up：{bookName}.");
            return true;
        }

        Debug.Log($"Pickup failed: {bookName} does not match any configuration.");
        return false;
    }

    public bool PlaceBookAtLayer(int layerNumber, string bookName)
    {
        Debug.Log($"Try to place: {bookName} on the {layerNumber} layer.");
        Debug.Log($"Current backpack contents: Fiction={hasNovel}, History={hasHistory}, Math={hasMath}.");

        if (IsLayerCompleted(layerNumber))
        {
            Debug.Log("This layer is already filled with books!");
            return false;
        }

        string requiredBook = "";
        bool hasRequiredBook = false;

        switch (layerNumber)
        {
            case 1:
                requiredBook = bookNameLayer1;
                hasRequiredBook = hasNovel;
                break;
            case 2:
                requiredBook = bookNameLayer2;
                hasRequiredBook = hasHistory;
                break;
            case 3:
                requiredBook = bookNameLayer3;
                hasRequiredBook = hasMath;
                break;
        }

        Debug.Log($"Layer {layerNumber} requires: {requiredBook}. There is {hasRequiredBook} in the backpack.");

        if (bookName != requiredBook)
        {
            Debug.Log($"Corrected text: {bookName} cannot be placed on the {layerNumber}th layer! It needs to be placed on {requiredBook}.");
            return false;
        }

        if (!hasRequiredBook)
        {
            Debug.Log($"There is no {requiredBook} book in the backpack!");
            return false;
        }

        // Placement successful. Removed from backpack.
        switch (layerNumber)
        {
            case 1:
                hasNovel = false;
                break;
            case 2:
                hasHistory = false;
                break;
            case 3:
                hasMath = false;
                break;
        }

        SetLayerCompleted(layerNumber, true);
        ChangeMaterial(layerNumber);

        // Close this level of Outline
        SetOutlineActive(layerNumber, false);

        Debug.Log($"Success! {bookName} has been placed on Layer {layerNumber}, and Outline has been closed.");
        return true;
    }

    void ChangeMaterial(int layer)
    {
        GameObject targetObj = null;
        Material newMaterial = null;

        switch (layer)
        {
            case 1:
                targetObj = outlineLayer1;
                newMaterial = newMaterialLayer1;
                break;
            case 2:
                targetObj = outlineLayer2;
                newMaterial = newMaterialLayer2;
                break;
            case 3:
                targetObj = outlineLayer3;
                newMaterial = newMaterialLayer3;
                break;
        }

        if (targetObj != null)
        {
            Renderer renderer = targetObj.GetComponent<Renderer>();
            if (renderer != null && newMaterial != null)
            {
                renderer.material = newMaterial;
                Debug.Log($"The material of the {layer}th layer has been switched.");
            }
        }
    }

    void SetOutlineActive(int layer, bool active)
    {
        GameObject targetObj = null;

        switch (layer)
        {
            case 1: targetObj = outlineLayer1; break;
            case 2: targetObj = outlineLayer2; break;
            case 3: targetObj = outlineLayer3; break;
        }

        if (targetObj != null)
        {
            Outline outline = targetObj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = active;
                Debug.Log($"The Outline setting for the {layer} layer is: {active}");
            }
        }
    }

    void SetLayerCompleted(int layer, bool completed)
    {
        if (layer == 1) layer1Completed = completed;
        else if (layer == 2) layer2Completed = completed;
        else if (layer == 3) layer3Completed = completed;
    }

    public bool IsLayerCompleted(int layer)
    {
        if (layer == 1) return layer1Completed;
        else if (layer == 2) return layer2Completed;
        else if (layer == 3) return layer3Completed;
        return false;
    }

    public bool IsAllBooksReturned()
    {
        return layer1Completed && layer2Completed && layer3Completed;
    }

    public void DisableAllOutlines()
    {
        DisableOutline(outlineLayer1);
        DisableOutline(outlineLayer2);
        DisableOutline(outlineLayer3);
    }

    void DisableOutline(GameObject obj)
    {
        if (obj != null)
        {
            Outline outline = obj.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    public bool HasNovel() { return hasNovel; }
    public bool HasMath() { return hasMath; }
    public bool HasHistory() { return hasHistory; }
}