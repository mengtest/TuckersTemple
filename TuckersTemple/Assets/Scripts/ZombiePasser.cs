﻿/*
 * ZombiePasser.cs
 * 
 * Passes information between Unity scenes:
 * Stores level to be started in main
 * Controls musicToggle, sfx, vibration bools
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePasser : MonoBehaviour {

	// public:

	// private:
	private int levelNum = 1;

	// these will store the settings save data
	private bool musicToggle;
	private bool sfxToggle;
	private bool vibToggle;

	// Use this for initialization
	void Start () {
		Awake ();
	}

	// Make this game object and all its transform children
	// survive when loading a new scene.
	private void Awake () {
		DontDestroyOnLoad(this);
		if (FindObjectsOfType(GetType()).Length > 1)
		{
			Destroy(gameObject);
		}
	}

	// sets level to be played through button
	// associated with that level
	public void setLevel(int newLevelNum){
		levelNum = newLevelNum;
	}
		
	// music toggle:
	public void setMusicToggle(){
		if (musicToggle == true) {
			musicToggle = false;
		} else {
			musicToggle = true;
		}
	}
		
	// sfx toggle:
	public void setSFXToggle(){
		if (sfxToggle == true) {
			sfxToggle = false;
		} else {
			sfxToggle = true;
		}
	}

	// vibration toggle:
	public void setVibToggle(){
		if (vibToggle == true) {
			vibToggle = false;
		} else {
			vibToggle = true;
		}
	}

	// return the private level int
	public int getLevel(){
		return levelNum;
	}

	public bool getMusicToggle(){
		return musicToggle;
	}

	public bool getSFXToggle(){
		return sfxToggle;
	}

	public bool getVibToggle(){
		return vibToggle;
	}
}
