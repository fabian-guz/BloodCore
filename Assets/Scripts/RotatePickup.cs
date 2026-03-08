using UnityEngine;

public class RotatePickup : MonoBehaviour
{
    public float rotationSpeed = 60f;
    public float floatHeight = 0.25f;
    public float floatSpeed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );
    }
}