using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    [Header("Test")]
    public bool alwaysBob = false;

    [Header("Bobbing")]
    public float bobSpeed = 10f;
    public float bobAmountX = 0.12f;
    public float bobAmountY = 0.08f;
    public float bobRotationZ = 8f;

    [Header("Smoothing")]
    public float smoothSpeed = 14f;

    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;
    private float bobTimer = 0f;

    void Start()
    {
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;

        Debug.Log("WeaponBobbing Start auf Objekt: " + gameObject.name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.localPosition = startLocalPosition + new Vector3(0.25f, 0.15f, 0f);
            Debug.Log("T gedrückt: GunRoot wurde hart verschoben.");
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        bool isMoving = alwaysBob || Mathf.Abs(moveX) > 0.1f || Mathf.Abs(moveZ) > 0.1f;

        Vector3 targetPosition = startLocalPosition;
        Quaternion targetRotation = startLocalRotation;

        if (isMoving)
        {
            bobTimer += Time.deltaTime * bobSpeed;

            float bobX = Mathf.Cos(bobTimer) * bobAmountX;
            float bobY = Mathf.Abs(Mathf.Sin(bobTimer)) * bobAmountY;
            float rotZ = Mathf.Sin(bobTimer) * bobRotationZ;

            targetPosition += new Vector3(bobX, bobY, 0f);
            targetRotation = startLocalRotation * Quaternion.Euler(0f, 0f, rotZ);
        }
        else
        {
            bobTimer = 0f;
        }

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            targetRotation,
            smoothSpeed * Time.deltaTime
        );
    }
}