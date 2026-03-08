using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("Position Recoil")]
    public float recoilBack = 0.25f;
    public float recoilUp = 0.10f;
    public float recoilSide = 0.05f;

    [Header("Rotation Recoil")]
    public float recoilX = -20f;
    public float recoilY = 8f;
    public float recoilZ = 10f;

    [Header("Speed")]
    public float snappiness = 25f;
    public float returnSpeed = 8f;

    private Vector3 startLocalPosition;
    private Quaternion startLocalRotation;

    private Vector3 targetPosition;
    private Vector3 currentPosition;

    private Vector3 targetRotation;
    private Vector3 currentRotation;

    private void Awake()
    {
        startLocalPosition = transform.localPosition;
        startLocalRotation = transform.localRotation;

        Debug.Log("GunRecoil Awake auf Objekt: " + gameObject.name);
    }

    private void Update()
    {
        targetPosition = Vector3.Lerp(targetPosition, Vector3.zero, returnSpeed * Time.deltaTime);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.deltaTime);

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Lerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        transform.localPosition = startLocalPosition + currentPosition;
        transform.localRotation = startLocalRotation * Quaternion.Euler(currentRotation);
    }

    public void Fire()
    {
        Debug.Log("GunRecoil Fire wurde aufgerufen auf: " + gameObject.name);

        targetPosition += new Vector3(
            Random.Range(-recoilSide, recoilSide),
            recoilUp,
            -recoilBack
        );

        targetRotation += new Vector3(
            recoilX,
            Random.Range(-recoilY, recoilY),
            Random.Range(-recoilZ, recoilZ)
        );
    }

    public void ResetRecoil()
    {
        targetPosition = Vector3.zero;
        currentPosition = Vector3.zero;
        targetRotation = Vector3.zero;
        currentRotation = Vector3.zero;

        transform.localPosition = startLocalPosition;
        transform.localRotation = startLocalRotation;
    }
}