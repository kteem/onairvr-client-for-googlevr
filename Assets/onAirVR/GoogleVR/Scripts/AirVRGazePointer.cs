using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class AirVRGazePointer : AirVRPointer {
    // implements AirVRPointerBase
    protected override string inputDeviceName {
        get {
            return AirVRInputDeviceName.HeadTracker;
        }
    }

    protected override byte raycastHitResultKey {
        get {
            return (byte)AirVRHeadTrackerKey.RaycastHitResult;
        }
    }

    protected override Vector3 worldOriginPosition {
        get { 
            return trackingOriginLocalToWorldMatrix.MultiplyPoint(UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.CenterEye));
        }
    }

    protected override Quaternion worldOriginOrientation {
        get {
            return rotationOfTrackingSpace * UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye);
        }
    }
}
