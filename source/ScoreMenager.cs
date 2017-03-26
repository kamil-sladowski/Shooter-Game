using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreMenager : MonoBehaviour {
	public static float score;
	public Text scoreText;

	void Avake () {
		score = 0.0f;
		scoreText = GetComponent<Text>();
	}
	
	void Update () {
		scoreText.text = "Score: " + score;
	}
}
