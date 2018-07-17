using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : UnitySingleton<SoundManager> {

	private AudioSource audioPlayer;

	public AudioClip bgm;

	public AudioClip speedUp;
	public AudioClip rotate;
	public AudioClip clearLine;
	public AudioClip gameover;

	void Awake(){
        audioPlayer = GetComponent<AudioSource>() as AudioSource;
	}




    public void SetVolumn(float v) {
        audioPlayer.volume = Mathf.Clamp01(v);
    }

}
