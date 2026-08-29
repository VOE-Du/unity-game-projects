using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlights : MonoBehaviour
{
    [Header("Flashlight settings")]
    public Light lightSource;
    public KeyCode switchKey = KeyCode.F;

    [Header("Purple light mode")]
    public bool enableUVMode = true;
    public KeyCode uvModeKey = KeyCode.U;
    public Color normalLightColor = Color.white;
    public Color uvLightColor = new Color(0.5f, 0f, 1f, 1f);
    public float uvIntensity = 2f;

    [Header("Blackboard interaction settings")]
    public float raycastDistance = 5f;
    public LayerMask blackboardLayer;
    public Text uiText;                         // The displayed UI text (shown after irradiation)

    [Header("New addition: The second Text setting")]
    public Text dynamicText;                    // The new Text component will change its content upon illumination (permanently).
    public string dynamicMessage = "3 + 2 = 5"; // This can be modified manually.

    [Header("Visual Effect")]
    public bool showDebugRay = true;            // Display the debugging rays.

    private bool isUVMode = false;
    private bool isPointingAtBlackboard = false;
    private bool hasChangedDynamicText = false;
    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInParent<Camera>();
        if (playerCamera == null)
            playerCamera = Camera.main;

        // Make sure that the text is initially hidden.
        if (uiText != null)
            uiText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Turn on/off the flashlight
        if (Input.GetKeyDown(switchKey))
        {
            lightSource.enabled = !lightSource.enabled;

            //If the flashlight is turned off and the text is hidden at the same time
            if (!lightSource.enabled)
            {
                HideText();
            }
        }

        // Switch to the ultraviolet light mode (this can only be done when the flashlight is on)
        if (enableUVMode && Input.GetKeyDown(uvModeKey) && lightSource.enabled)
        {
            ToggleUVMode();
        }

        // Real-time detection of whether the purple light has reached the blackboard
        if (lightSource.enabled && isUVMode)
        {
            CheckBlackboardWithUV();
        }
        else
        {
            // If it is not in the spotlight mode, make sure the text is hidden (but the second Text will not be restored)
            if (isPointingAtBlackboard)
            {
                HideText();
                isPointingAtBlackboard = false;
            }
        }
    }

    void ToggleUVMode()
    {
        isUVMode = !isUVMode;

        if (isUVMode)
        {
            // Switch to the purple light mode
            lightSource.color = uvLightColor;
            lightSource.intensity = uvIntensity;
            Debug.Log("The ultraviolet light mode has been activated - it can be used to shine on the blackboard to view the hidden text.");
        }
        else
        {
            // Switch back to the normal mode and only hide the first Text.
            lightSource.color = normalLightColor;
            lightSource.intensity = 1f;
            HideText();
            isPointingAtBlackboard = false;
            Debug.Log("Normal mode has been enabled - Hide text");
        }
    }

    void CheckBlackboardWithUV()
    {
        // Project rays (in the direction of the flashlight) from the center of the camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Draw debugging rays
        if (showDebugRay)
        {
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.magenta);
        }

        if (Physics.Raycast(ray, out hit, raycastDistance, blackboardLayer))
        {
            // The purple light shines on the blackboard.
            if (!isPointingAtBlackboard)
            {
                isPointingAtBlackboard = true;
                ShowText();

                // Only during the first irradiation will the content of the second Text be changed.
                if (!hasChangedDynamicText)
                {
                    ChangeDynamicText();
                    hasChangedDynamicText = true;
                    Debug.Log("The purple light was shone onto the blackboard for the first time! The second text content was permanently changed to:" + dynamicMessage);
                }

                Debug.Log("The purple light shines on the blackboard! Displayed text:");
            }
        }
        else
        {
            // √ª”–’’…‰µΩ∫⁄∞Â
            if (isPointingAtBlackboard)
            {
                isPointingAtBlackboard = false;
                HideText();
                Debug.Log("Move out of the blackboard range and hide the first Text (the content of the second Text remains unchanged)");
            }
        }
    }

    void ShowText()
    {
        if (uiText != null)
        {
            uiText.text = "Do you still remember\nthe answer?\n3 + 2 = ?";
            uiText.gameObject.SetActive(true);
        }
    }

    void HideText()
    {
        if (uiText != null)
        {
            uiText.gameObject.SetActive(false);
        }
    }

    // Change the content of the second Text (permanently, it will not be restored)
    void ChangeDynamicText()
    {
        if (dynamicText != null)
        {
            dynamicText.text = dynamicMessage;
        }
    }

    // Public method: Manually control text display (can be used for other triggering methods)
    public void ForceShowText()
    {
        ShowText();

        if (!hasChangedDynamicText)
        {
            ChangeDynamicText();
            hasChangedDynamicText = true;
        }
    }

    public void ForceHideText()
    {
        HideText();
        isPointingAtBlackboard = false;
    }

    // Public method: Manually modify dynamicMessage (runtime modification)
    public void SetDynamicMessage(string newMessage)
    {
        dynamicMessage = newMessage;
        //If the area has already been illuminated and the second Text has changed, immediately update the display.
        if (hasChangedDynamicText && dynamicText != null)
        {
            dynamicText.text = dynamicMessage;
        }
    }

    // Public method: Reset state (if necessary to start over)
    public void ResetDynamicTextState()
    {
        hasChangedDynamicText = false;
    }

    // Display the range of the purple light in the Scene view
    void OnDrawGizmosSelected()
    {
        if (playerCamera != null && showDebugRay)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * raycastDistance);
        }
    }
}