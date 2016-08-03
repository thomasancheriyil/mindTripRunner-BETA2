using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timeManager : MonoBehaviour {
	public float StartingTime;
	public float timeLeft;
	public float penalty = 3.0f;
	public float bonus = 3.0f;
	public Text timer;
	public Image dangerImage;
	public float flashSpeed = 5f;
	public Color flashColor= new Color(1f,0f,0f,.1f);
	public int alarmThresholdSec = 10;
	private string a;
	private string b;
	// Use this for initialization
	void Start () {
		timeLeft = StartingTime;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (timeLeft < .1) {
			timeLeft = 0.0f;
		}
			else{
			timeLeft -= Time.deltaTime;
			}
		int mins = (int) Mathf.Floor(timeLeft / 60.0f);
		int secs = (int) Mathf.Floor(timeLeft - mins * 60.0f);
		if (mins < 0) {
			a = "00";
			b = "00";
		}
		else if (0 <= mins && mins < 10) {
			a = string.Concat("0" ,mins.ToString ());
		} else {
			a = mins.ToString ();
		}
		if (secs < 0) {
			b = "00";
		} else if (secs >= 0 && secs < 10) {
			b = string.Concat ("0", secs.ToString ());
		} else {
			b = secs.ToString ();
		}
		a = a +" : "+ b;
		timer.text = a;
		if (timeLeft < alarmThresholdSec) {
			dangerImage.color = flashColor;
		} else {
			dangerImage.color = Color.Lerp (dangerImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}
}
}
