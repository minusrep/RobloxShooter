using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGameobject : MonoBehaviour
{
    public float rotationSpeed;

    void OnMouseDrag()
    {
        float rotationX = Input.GetAxis("Mouse X") * rotationSpeed * Mathf.Deg2Rad;
        transform.Rotate(Vector3.down, -rotationX, Space.Self);
    }
}
