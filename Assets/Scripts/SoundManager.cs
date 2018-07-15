using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioSource player;

	public AudioClip bgm;

	public AudioClip speedUp;
	public AudioClip rotate;
	public AudioClip clearLine;
	public AudioClip gameover;

	void Awake(){
		player = GetComponent<AudioSource>() as AudioSource;
	}


	void OnSpeedUp(){

	}


}
