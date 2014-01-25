﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUD : MonoBehaviour {
	public SenseController senseController;
	public int margin = 20;
	private int slotIconSize;
	
	Rect senseSlot1;
	Rect senseSlot2;
	
	Dictionary<SenseController.SenseType, Texture2D> iconDict;
	// Use this for initialization
	void Start () {
		slotIconSize = Mathf.FloorToInt(Screen.height * 0.2f);
		
		iconDict = new Dictionary<SenseController.SenseType, Texture2D>();

		iconDict.Add(SenseController.SenseType.Sight,
						Resources.Load ("SenseIcons/sight-icon", typeof(Texture2D)) as Texture2D);
		iconDict.Add(SenseController.SenseType.Hearing,
		             	Resources.Load ("SenseIcons/hearing-icon", typeof(Texture2D)) as Texture2D);
		iconDict.Add(SenseController.SenseType.Scent,
		            	Resources.Load ("SenseIcons/scent-icon", typeof(Texture2D)) as Texture2D);
		iconDict.Add(SenseController.SenseType.Feeling,
		             	Resources.Load ("SenseIcons/feeling-icon",typeof(Texture2D)) as Texture2D);
		iconDict.Add (SenseController.SenseType.None, null);
		
		senseSlot1 = new Rect(margin, Screen.height - margin - slotIconSize, slotIconSize, slotIconSize);
		senseSlot2 = new Rect(margin + slotIconSize + 10, Screen.height - margin - slotIconSize,
								slotIconSize, slotIconSize);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI () {
		GUI.backgroundColor = _GetBoxColor(0);
		GUI.Box (senseSlot1, iconDict[GameController.Instance.GetSenseInSlot(0)]);
		GUI.backgroundColor = _GetBoxColor(1);
		GUI.Box (senseSlot2, iconDict[GameController.Instance.GetSenseInSlot(1)]);
	}
	
	private Color _GetBoxColor(int slot) {
		if (GameController.Instance.IsSlotActive(slot)) {
			return Color.cyan;
		} else {
			return Color.white;
		}
	}
}
