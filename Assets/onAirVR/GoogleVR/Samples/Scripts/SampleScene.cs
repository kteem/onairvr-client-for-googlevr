using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScene : MonoBehaviour, AirVRClient.EventHandler {
	[SerializeField] private string _address;
	[SerializeField] private int _port;
	[SerializeField] private string _userID;

	private void Awake() {
		AirVRClient.Delegate = this;
	}

	private void Update() {
		if (GvrControllerInput.AppButtonDown) {
			if (AirVRClient.connected) {
				AirVRClient.Disconnect();
			}
			else {
				AirVRClient.Connect(_address, _port, _userID);
			}
		}
	}

	private void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus && AirVRClient.playing) {
			AirVRClient.Stop();
		}
		else if (pauseStatus == false && AirVRClient.connected) {
			AirVRClient.Play();
		}
	}

	// implements AirVRClient.EventHandler
	public void AirVRClientFailed(string reason) {
		Debug.Log("[AirVRClient] failed : " + reason);
	}

	public void AirVRClientConnected() {
		AirVRClient.Play();
	}

	public void AirVRClientPlaybackStarted() {}
	public void AirVRClientPlaybackStopped() {}
	public void AirVRClientDisconnected() {}
	public void AirVRClientUserDataReceived(byte[] userData) {}
}
