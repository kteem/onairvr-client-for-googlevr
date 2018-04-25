using UnityEngine;

// Script Execution Order를 GvrControllerInput 다음인 -31122 으로 설정. 
public sealed class AirVRClientGvrControllerSwipeInput : Singleton<AirVRClientGvrControllerSwipeInput>
{
    private AirVRClientGvrControllerSwipeInput() { }

    private int lastUpdatedFrameCount = -1;

    private bool emitSwipe = false;
    private Vector2 moveAmount;
    private float minMoveMagnitude = 0.3f;

    private bool _up = false;
    private bool _down = false;
    private bool _left = false;
    private bool _right = false;

    public static bool Up
    {
        get
        {
            if (Instance == null)
            {
                return false;
            }
            return Instance._up;
        }
    }

    public static bool Down
    {
        get
        {
            if (Instance == null)
            {
                return false;
            }
            return Instance._down;
        }
    }

    public static bool Left
    {
        get
        {
            if (Instance == null)
            {
                return false;
            }
            return Instance._left;
        }
    }

    public static bool Right
    {
        get
        {
            if (Instance == null)
            {
                return false;
            }
            return Instance._right;
        }
    }

    private void Update()
    {
        if (lastUpdatedFrameCount != Time.frameCount)
        {
            lastUpdatedFrameCount = Time.frameCount;
            _up = _down = _left = _right = false;
        }

        if (GvrControllerInput.TouchDown)
        {
            emitSwipe = true;
            moveAmount = GvrControllerInput.TouchPos;
        }

        if (GvrControllerInput.TouchUp && emitSwipe)
        {
            emitSwipe = false;

            moveAmount.x = GvrControllerInput.TouchPos.x - moveAmount.x;
            moveAmount.y = GvrControllerInput.TouchPos.y - moveAmount.y;

            if (moveAmount.magnitude >= minMoveMagnitude)
            {
                moveAmount.Normalize();

                // Left/Right
                if (Mathf.Abs(moveAmount.x) > Mathf.Abs(moveAmount.y))
                {
                    if (moveAmount.x < 0.0f)
                    {
                        _left = true;
                    }
                    else
                    {
                        _right = true;
                    }
                }
                // Up/Down
                else
                {
                    if (moveAmount.y > 0.0f)
                    {
                        _down = true;
                    }
                    else
                    {
                        _up = true;
                    }
                }
            }
        }
    }
}
