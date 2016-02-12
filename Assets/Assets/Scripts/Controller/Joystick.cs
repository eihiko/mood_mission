using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Globals;

public class Joystick : MonoBehaviour
{
    public Vector2 CurrentSpeedAndDirection;
    public float MaxSpeed = 3f;
    public float MaxDirection = 1f;
    public bool shouldStartTimers = true;

    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    private Vector3 originalPosition;
    private bool initialized = false;

	private void Initialize() 
    {
        var parent = transform.parent.GetComponent<RectTransform>().rect;
        xMin = transform.TransformPoint(new Vector3(parent.xMin, 0f, 0f)).x;
        xMax = transform.TransformPoint(new Vector3(parent.xMax, 0f, 0f)).x;
        yMin = transform.TransformPoint(new Vector3(0f, parent.yMin, 0f)).y;
        yMax = transform.TransformPoint(new Vector3(0f, parent.yMax, 0f)).y;
	    initialized = true;
    }

    public virtual void ButtonDown()
    {
        if (Input.touchCount > 1) return;
        if (!initialized) Initialize();
        originalPosition = transform.position;
        Timeout.StopTimers();
        Utilities.StopAudio(Sound.CurrentPlayingSound);
        StopAllCoroutines();
    }

    public virtual void ButtonRelease()
    {
        if (Input.touchCount > 1) return;
        transform.position = originalPosition;
        resetSpeedAndDirection();
        if (shouldStartTimers) Timeout.StartTimers();
    }

    public void Drag()
    {
        float x = 0f;
        float y = 0f;
        if (Input.touchCount > 1)
        {
            transform.position = originalPosition;
            resetSpeedAndDirection();
            return;
        }
        if (isInRange(transform.position) && isInRange(Input.mousePosition))
        {
            x = Input.mousePosition.x;
            y = Input.mousePosition.y;
        }
        else if (isInRangeX(transform.position) && isInRangeX(Input.mousePosition))
        {
            x = Input.mousePosition.x;
            y = Input.mousePosition.y > yMax ? yMax : yMin;
        }
        else if (isInRangeY(transform.position) && isInRangeY(Input.mousePosition))
        {
            x = Input.mousePosition.x > xMax ? xMax : xMin;
            y = Input.mousePosition.y;
        }
        else
        {
            x = Input.mousePosition.x > xMax ? xMax : xMin;
            y = Input.mousePosition.y > yMax ? yMax : yMin;
        }
        transform.position = new Vector2(x, y);
        computeSpeedAndDirection(x, y);
    }

    private void resetSpeedAndDirection()
    {
        CurrentSpeedAndDirection = new Vector2(0f, 0f);
    }

    private void computeSpeedAndDirection(float x, float y)
    {
        var baseX = x - originalPosition.x;
        var baseY = y - originalPosition.y;
        var baseXMax = xMax - originalPosition.x;
        var baseYMax = yMax - originalPosition.y;
        CurrentSpeedAndDirection = new Vector2((baseX / baseXMax) * MaxDirection, (baseY / baseYMax) * MaxSpeed);
    }

    private bool isInRange(Vector3 position)
    {
        return isInRangeX(position) && isInRangeY(position);
    }

    private bool isInRangeX(Vector3 position)
    {
        return position.x >= xMin && position.x <= xMax;
    }

    private bool isInRangeY(Vector3 position)
    {
        return position.y >= yMin && position.y <= yMax;
    }
}
