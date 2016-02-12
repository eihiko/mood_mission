using System.Collections;
using UnityEngine;

public class JumpMovementHandler : MovementHandler
{
    public override void HandleMovement(Transform transform, Joystick joystick)
    {
        float moveSpeed = Time.deltaTime * joystick.CurrentSpeedAndDirection.y;
        float moveDirection = Time.deltaTime * joystick.CurrentSpeedAndDirection.x;
        transform.position = new Vector3(transform.position.x + moveSpeed, transform.position.y, transform.position.z - moveDirection);
    }
}
