using UnityEngine;

public class BloodDecal : MonoBehaviour
{
    public float lifeTime = 20f;
    public float surfaceOffset = 0.03f;
    public Vector3 rotationOffset = Vector3.zero;

    public void Setup(Vector3 position, Vector3 normal, float scale)
    {
        transform.position = position + normal * surfaceOffset;
        transform.rotation = Quaternion.LookRotation(normal);
        transform.rotation *= Quaternion.Euler(rotationOffset);

        transform.Rotate(0f, 0f, Random.Range(0f, 360f), Space.Self);
        transform.localScale = new Vector3(scale, scale, scale);

        Destroy(gameObject, lifeTime);
    }
}