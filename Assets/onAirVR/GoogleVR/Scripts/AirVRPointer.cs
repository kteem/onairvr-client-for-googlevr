using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AirVRPointer : AirVRPointerBase {
    private Quaternion _rotationOfRoot;
    private Matrix4x4 _trackingLocalToWorldMatrix;

    protected Quaternion rotationOfTrackingSpace {
        get {
            return _rotationOfRoot;
        }
    }

    // implements AirVRPointerBase
    protected override void recalculatePointerRoot() {
        Transform root = Camera.main.transform.parent ?? Camera.main.transform;
        _rotationOfRoot = Camera.main.transform.parent != null ? Camera.main.transform.parent.rotation : Quaternion.identity;
        _trackingLocalToWorldMatrix.SetTRS(Camera.main.transform.position, _rotationOfRoot, root.localScale);
    }

    protected override Matrix4x4 trackingOriginLocalToWorldMatrix {
        get {
            return _trackingLocalToWorldMatrix;
        }
    }
}
