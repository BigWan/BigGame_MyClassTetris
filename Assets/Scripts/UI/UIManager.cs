using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour {

	public Transform MainUI;
	public Transform ButtonPanel;
	public Button StartButton;
	public Button SettingButton;

	public Text scoreText;
	public TetrisManager manager;

	private void Awake() {
		Debug.Log("Awake");

		manager.ScoreChange += OnScoreChange;
	}

	void OnScoreChange(object sender,System.EventArgs e){
		ShowScore();
	}
	void ShowScore(){
		Debug.Log("Show Score");
		scoreText.text = manager.score.ToString();
	}


}
