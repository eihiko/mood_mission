using System.Collections;
using System.Linq;
using SadScene;
using UnityEngine;

public class SoccerMovementHandler : MovementHandler
{
    public int currentIndex = 1;
    public Transform[] lanes;
    private bool shouldAdjustPosition = true;

    public override void HandleMovement(Transform transform, Joystick joystick)
    {
        float moveSpeed = Time.deltaTime * joystick.CurrentSpeedAndDirection.y;
        transform.position = new Vector3(transform.position.x + moveSpeed, transform.position.y, transform.position.z);
        adjustPosition(transform, joystick);
    }

    private void adjustPosition(Transform transform, Joystick joystick)
    {
        if (joystick.CurrentSpeedAndDirection.x == 0f) shouldAdjustPosition = true;
        if (!shouldAdjustPosition) return;
        float newPositionZ = transform.position.z;
        if (joystick.CurrentSpeedAndDirection.x <= -1.0f)
        {
            newPositionZ = currentIndex == 0 ? lanes[currentIndex].position.z : lanes[--currentIndex].position.z;
            shouldAdjustPosition = false;
        }
        else if (joystick.CurrentSpeedAndDirection.x >= 1.0f)
        {
            newPositionZ = currentIndex == lanes.Length - 1
                ? lanes[currentIndex].position.z
                : lanes[++currentIndex].position.z;
            shouldAdjustPosition = false;
        }
        transform.position = new Vector3(transform.position.x, transform.position.y, newPositionZ);
    }
}
