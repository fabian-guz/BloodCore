using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("Camera Kick")]
    public float recoilReturnSpeed = 8f;
    public float recoilSnappiness = 14f;

    private float xRotation = 0f;

    private Vector2 currentRecoil;
    private Vector2 targetRecoil;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        targetRecoil = Vector2.Lerp(targetRecoil, Vector2.zero, recoilReturnSpeed * Time.deltaTime);
        currentRecoil = Vector2.Lerp(currentRecoil, targetRecoil, recoilSnappiness * Time.deltaTime);

        float finalXRotation = xRotation + currentRecoil.x;

        transform.localRotation = Quaternion.Euler(finalXRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void AddRecoil(float pitchAmount, float yawAmount)
    {
        targetRecoil += new Vector2(
            -pitchAmount,
            Random.Range(-yawAmount, yawAmount)
        );
    }
}