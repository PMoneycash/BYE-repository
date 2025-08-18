using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using UnityEngine;

public class AimAtTransform : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target == null) return;

        // Work out yaw (Y angle only)
        Vector3 dir = target.position - transform.position;
        dir.y = 0f; // flatten so only left/right
        if (dir.sqrMagnitude < 1e-6f) return;

        float yaw = Quaternion.LookRotation(dir).eulerAngles.y;

        // Force X=90, Z=180, only Y changes
        transform.rotation = Quaternion.Euler(90f, yaw, 180f);
    }
}
