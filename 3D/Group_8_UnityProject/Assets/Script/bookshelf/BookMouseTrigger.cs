using UnityEngine;

public class BookMouseTrigger : MonoBehaviour
{
    public int layerNumber;  // 1¡¢2¡¢3

    private UIManager uiManager;
    private Camera mainCamera;
    private bool isHovering = false;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        mainCamera = Camera.main;

        // Make sure there is a Collider (it doesn't need to be a Trigger)
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            col = gameObject.AddComponent<BoxCollider>();
        }
        col.isTrigger = false;
    }

    void Update()
    {
        // Project rays (crosshair) from the center of the screen
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 5f))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isHovering)
                {
                    isHovering = true;
                    Debug.Log($"Point the mouse at the {layerNumber}th layer of the book.");
                }

                // Press "E" when the mouse pointer is over.
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Debug.Log($"Press E to activate the {layerNumber}th layer");
                    if (uiManager != null)
                    {
                        uiManager.ShowButtonsForLayer(layerNumber);
                    }
                }
            }
            else
            {
                isHovering = false;
            }
        }
        else
        {
            isHovering = false;
        }
    }
}