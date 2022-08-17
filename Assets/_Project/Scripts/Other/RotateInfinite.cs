using UnityEngine;

public class RotateInfinite : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.Rotate(Vector3.up, Space.Self);
    }
}
