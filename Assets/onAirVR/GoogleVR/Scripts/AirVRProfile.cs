using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class AirVRProfile : AirVRProfileBase {
    public AirVRProfile(AirVRCamera camera) {
		_camera = camera.gameObject.GetComponent<Camera>();
    }

    private Camera _camera;

	// implements AirVRProfileBase
    public override int videoWidth { 
        get {
            return 2560;
        }
    }

    public override int videoHeight { 
        get {
            return 1280;
        }
    }

    public override float videoFrameRate {
        get {
#if !UNITY_EDITOR && UNITY_ANDROID
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject display = activity.Call<AndroidJavaObject>("getSystemService", "window").Call<AndroidJavaObject>("getDefaultDisplay");
            return display.Call<float>("getRefreshRate");
#else
            return 59.2f;
#endif
        }
    }

	public override bool stereoscopy { 
		get {
			return true;
		}
	}

    public override bool isEyeCameraFrustumSymmetric { 
        get {
            return false;
        }
    }

    public override float eyeCameraVerticalFieldOfView { 
        get {
			return _camera.fieldOfView;
        }
    }

    public override float eyeCameraAspectRatio { 
        get {
            return _camera.aspect;
        }
    }

    public override float[] leftEyeCameraNearPlane { 
        get {
            Matrix4x4 inverse = _camera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left).inverse;
            Vector3 l = inverse.MultiplyPoint(new Vector3(-1.0f, 0.0f, -1.0f));
            Vector3 t = inverse.MultiplyPoint(new Vector3(0.0f, 1.0f, -1.0f));
            Vector3 r = inverse.MultiplyPoint(new Vector3(1.0f, 0.0f, -1.0f));
            Vector3 b = inverse.MultiplyPoint(new Vector3(0.0f, -1.0f, -1.0f));

            // should return (l, t, r, b) of the near plane of the camera frustum when n = 1.
            return new float[] { l.x / Mathf.Abs(l.z), t.y / Mathf.Abs(t.z), r.x / Mathf.Abs(r.z), b.y / Mathf.Abs(b.z) };
        }
    }

    public override Vector3 eyeCenterPosition { 
        get {
            return Quaternion.Inverse(UnityEngine.XR.InputTracking.GetLocalRotation(UnityEngine.XR.XRNode.CenterEye)) * UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.CenterEye);
        }
    }

    public override float ipd { 
		get {
			return Vector3.Distance(UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.LeftEye), UnityEngine.XR.InputTracking.GetLocalPosition(UnityEngine.XR.XRNode.RightEye));
		}
	}

	public override bool hasInput {
		get {
			return true;
		}
	}

	public override RenderType renderType {
		get {
			return RenderType.UseSeperateVideoRenderTarget;
		}
	}

	public override int[] leftEyeViewport { 
		get {
			return new int[] { 0, 0, videoWidth / 2, videoHeight};
		}
	}

	public override int[] rightEyeViewport { 
		get {
			return new int[] { videoWidth / 2, 0, videoWidth / 2, videoHeight};
		}
	}

	public override float[] videoScale {
		get {
			// the eye texture size per eye is 1302x1529 (Pixel XL).
			// so the video size of QHD for both eyes is not enough but fills eye textures.
			return new float[] { 1.0f, 1.0f };
		}
	}

	public override bool isUserPresent {
		get {
			return true;
		}
	}

	public override float delayToResumePlayback {
		get {
			return 0.1f;
		}
	}

	public override bool supportHEVC {
		get {
			return true;
		}
	}
}
