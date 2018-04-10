using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[RequireComponent(typeof(Camera))]

public class AirVRCamera : AirVRCameraBase {
    [DllImport(AirVRClient.LibPluginName)]
    private static extern void onairvr_InitJNI();

    private AirVRProfile _profile;
    private List<AirVRInputDevice> _inputDevices;

	[SerializeField] private GvrArmModel _armModel;

    protected override void Awake() {
        // Work around : Only the game module can access to java classes in onAirVR client plugin.
        if (Application.isEditor == false && Application.platform == RuntimePlatform.Android) {
            onairvr_InitJNI();
        }
        
		_profile = new AirVRProfile(this);
        base.Awake();

        _inputDevices = new List<AirVRInputDevice>();
		_inputDevices.Add(new AirVRTrackedControllerInputDevice(_armModel));
    }

    protected override void Start() {
        base.Start();

        foreach (var inputDevice in _inputDevices) {
            AirVRInputManager.RegisterInputDevice(inputDevice);
        }

		gameObject.AddComponent<AirVRTrackedControllerPointer>().Configure(defaultTrackedControllerModel, true, _armModel);
    }

    protected override AirVRProfileBase profile {
        get {
            return _profile;
        }
    }

	protected override void RecenterPose() {
		UnityEngine.XR.InputTracking.Recenter();
	}
}
