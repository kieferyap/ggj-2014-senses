﻿using UnityEngine;
using System.Collections;

public class SenseController : MonoBehaviour {
	public GameObject SightGO;
	public GameObject HearingGO;
	public GameObject ScentGO;
	public GameObject FeelingGO;
	
	public bool startWithNoSense = false;

	private static SenseController _singleton;

	public static SenseController Instance {
		get { return _singleton; }
	}

	public enum SenseType {
		Sight, Hearing, Scent, Feeling, None
	};
	
	private static ButtonHandler buttonHandler;
	
	private Material enabledSkybox;
	private Material disabledSkybox;
	
	void Awake () {
		SightGO = transform.Find("SightCamera").gameObject;
		HearingGO = transform.Find("HearingCamera").gameObject;
		ScentGO = transform.Find("ScentCamera").gameObject;
		FeelingGO = transform.Find("FeelingCamera").gameObject;

		enabledSkybox = Resources.Load ("Materials/Materials/Skybox-invert", typeof(Material)) as Material;
		disabledSkybox = Resources.Load ("Materials/Materials/Skybox", typeof(Material)) as Material;
	
		SetSenseEnabled(SenseType.Sight, !startWithNoSense);
		SetSenseEnabled(SenseType.Hearing, false);
		SetSenseEnabled(SenseType.Feeling, false);
		SetSenseEnabled(SenseType.Scent, false);

		buttonHandler = transform.parent.GetComponent<ButtonHandler>();
		_singleton = this;

		//Debug.Log ("ID: " + GetInstanceID());
	}
	
	public bool GetSenseEnabled (SenseType sense) {
		switch(sense) {
			case SenseType.Sight:
				return SightGO.activeInHierarchy;
			case SenseType.Hearing:
				return HearingGO.activeInHierarchy;
			case SenseType.Scent:
				return ScentGO.activeInHierarchy;
			case SenseType.Feeling:
				return FeelingGO.activeInHierarchy;
			default:
				Debug.Log ("GetSenseEnabled: Invalid sense");
				return false;
		}
	}
	
	public void SetSenseEnabled (SenseType sense, bool active) {
		switch(sense) {
			case SenseType.Sight:
				SightGO.SetActive(active);
				OnSightStateChanged(active);
				break;
			case SenseType.Hearing:
				HearingGO.SetActive(active);
				WorldAudioManager.Instance.ToggleAudioSource(active);
				break;
			case SenseType.Scent:
				ScentGO.SetActive(active);
				break;
			case SenseType.Feeling:
				FeelingGO.SetActive(active);

				if(!active && PlayerController.Instance.carryingObject != null) {
					PlayerController.Instance.carryingObject.collider.enabled = true;
					PlayerController.Instance.carryingObject.rigidbody.useGravity = true;
					PlayerController.Instance.carryingObject.transform.parent = null;
					PlayerController.Instance.carryingObject = null;
					PlayerController.Instance.isCarrying = false;
				}
				if (buttonHandler != null) {
					buttonHandler.enabled = active;
				}

				break;
			default:
				Debug.Log ("SetSenseEnabled: Invalid sense");
				break;
		}
		WorldAudioManager.Instance.PlayLockSense();
	}

	/*
	 * Activation Functions
	 */
	public void OnSightStateChanged(bool newState) {
		GameObject[] g = GameObject.FindGameObjectsWithTag("SightActivatable");
		for (int i = 0; i < g.Length; i++) {
			Collider c = g[i].GetComponent<Collider>();
			if (c != null) c.isTrigger = !newState; //You cannot physically interact with a Trigger; therefore, this has the same consequence as disabling a collider
		}
		
		if (newState) {
			RenderSettings.skybox = enabledSkybox;
		} else {
			RenderSettings.skybox = disabledSkybox;
		}
	}
}
