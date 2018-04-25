using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

internal class AirVRTrackedControllerInputDevice : AirVRInputDevice {
	public AirVRTrackedControllerInputDevice(GvrArmModel armModel) {
		_armModel = armModel;
	}

	private GvrArmModel _armModel;

	private Vector2 translateTouchPos(Vector2 pos) {
		return new Vector2(pos.x - 0.5f, 0.5f - pos.y) * 2.0f;
	}

	// implements AirVRInputDevice
    protected override string deviceName {
        get {
            return AirVRInputDeviceName.TrackedController;
        }
    }

    protected override bool connected {
        get {
			return GvrControllerInput.State == GvrConnectionState.Connected;
        }
    }

    protected override void PendInputs(AirVRInputStream inputStream) {
	    inputStream.PendTransform(this, (byte)AirVRTrackedControllerKey.Transform,
								  _armModel.ControllerPositionFromHead, 
								  _armModel.ControllerRotationFromHead);
	    inputStream.PendTouch(this, (byte)AirVRTrackedControllerKey.Touchpad,
							  GvrControllerInput.IsTouching ? translateTouchPos(GvrControllerInput.TouchPos) : Vector2.zero, 
							  GvrControllerInput.IsTouching);
	    inputStream.PendButton(this, (byte)AirVRTrackedControllerKey.ButtonTouchpad, GvrControllerInput.ClickButton);
	    inputStream.PendButton(this, (byte)AirVRTrackedControllerKey.ButtonBack, GvrControllerInput.AppButton);
    }
}
