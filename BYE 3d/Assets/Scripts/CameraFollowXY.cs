using UnityEngine;

public class CameraFollowXY : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public float offsetY = 3f;

    private float fixedZ;

    void Start()
    {
        fixedZ = transform.position.z;
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = new Vector3(target.position.x, target.position.y + offsetY, fixedZ);
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }
}