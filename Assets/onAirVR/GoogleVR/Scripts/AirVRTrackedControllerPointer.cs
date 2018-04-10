using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class AirVRTrackedControllerPointer : AirVRPointer {
	private Transform _origin;
	private GvrArmModel _armModel;

	public void Configure(GameObject bodyModelPrefab, bool createRay, GvrArmModel armModel) {
		Configure(bodyModelPrefab, createRay);

		_armModel = armModel;
		_origin = _armModel.transform.parent;
	}

    // implements AirVRPointerBase
    protected override string inputDeviceName {
        get {
            return AirVRInputDeviceName.TrackedController;
        }
    }

    protected override byte raycastHitResultKey {
        get {
            return (byte)AirVRTrackedControllerKey.RaycastHitResult;
        }
    }

    protected override Vector3 worldOriginPosition {
        get {
			Vector3 pos = _armModel.ControllerPositionFromHead;
			return _origin == null ? pos : _origin.localToWorldMatrix.MultiplyPoint(pos);
        }
    }

    protected override Quaternion worldOriginOrientation {
        get {
			return (_origin == null ? Quaternion.identity : _origin.rotation) * _armModel.ControllerRotationFromHead;
        }
    }
}
