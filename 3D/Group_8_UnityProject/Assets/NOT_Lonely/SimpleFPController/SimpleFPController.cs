using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleFPController : MonoBehaviour
{
    [Header("MOUSE LOOK")]
    public Vector2 mouseSensitivity = new Vector2(80, 80);
    public Vector2 verticalLookLimit = new Vector2(-85, 85);

    private float xRot;
    private Camera cam;

    [Header("MOVEMENT")]
    public float walkSpeed = 1;
    public float runSpeed = 3;
    public float jumpForce = 2;
    private float speed = 1;

    [Header("CONTROLS")]
    public KeyCode forward = KeyCode.W;
    public KeyCode backward = KeyCode.S;
    public KeyCode strafeLeft = KeyCode.A;
    public KeyCode strafeRight = KeyCode.D;
    public KeyCode run = KeyCode.LeftShift;
    public KeyCode jump = KeyCode.Space;
    public KeyCode toggleCursor = KeyCode.Escape;

    [Header("SIGHT")]
    public bool sight = true;
    public GameObject sightPrefab;

    private Rigidbody rb;
    private bool isCursorLocked = true;

    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();

        LockCursor(true);

        if (sight)
        {
            GameObject sightObj = Instantiate(sightPrefab);
            sightObj.transform.SetParent(transform.parent);
        }
    }

    void Update()
    {
        // ESC键切换光标锁定
        if (Input.GetKeyDown(toggleCursor))
        {
            isCursorLocked = !isCursorLocked;
            LockCursor(isCursorLocked);
        }

        // 检查是否正在使用输入框
        bool isUsingInput = false;
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            UnityEngine.GameObject selected = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            if (selected != null && selected.GetComponent<InputField>() != null)
            {
                isUsingInput = true;
            }
        }

        // 只有在光标锁定且没有使用输入框时，才处理移动和视角
        if (isCursorLocked && !isUsingInput)
        {
            CameraLook();
            PlayerMove();
        }
    }

    void LockCursor(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void CameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity.x * 10;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity.y * 10;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, verticalLookLimit.x, verticalLookLimit.y);
        cam.transform.localEulerAngles = new Vector3(xRot, 0, 0);

        transform.Rotate(Vector3.up * mouseX);
    }

    void PlayerMove()
    {
        if (Input.GetKey(run))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        if (Input.GetKey(forward))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(backward))
        {
            transform.Translate(Vector3.forward * -speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(strafeLeft))
        {
            transform.Translate(Vector3.right * -speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKey(strafeRight))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetKeyDown(jump))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}