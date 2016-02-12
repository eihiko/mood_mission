using System.Collections;
using UnityEngine;

public abstract class MovementHandler : MonoBehaviour
{
    public abstract void HandleMovement(Transform transform, Joystick joystick);

    public virtual void OverrideMovement(Transform transform, float moveSpeed, float moveDirection)
    {
        transform.position = new Vector3(transform.position.x + moveSpeed, transform.position.y, transform.position.z - moveDirection);
    }
}
