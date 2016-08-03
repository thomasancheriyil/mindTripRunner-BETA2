using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class polishPill : MonoBehaviour {
	public GameObject popup;
	private timeManager gameTimer;
	AudioSource pillBonus;
	Text popuptext;

	// Use this for initialization
	void Start () {
		
		gameTimer = GameObject.Find ("TimeManager").GetComponent<timeManager>();
		pillBonus = GetComponent<AudioSource>();

	}
	void OnCollisionEnter(Collision other){
		
		if (other.gameObject.CompareTag ("Player")) {
			gameTimer.timeLeft += gameTimer.bonus;
		}
		pillBonus.Play();
		GameObject A = Instantiate (popup);
		Destroy (this.gameObject);
		popuptext = A.transform.GetChild(0).gameObject.GetComponent<Text> ();
		popuptext.text = string.Concat("+" ,gameTimer.bonus.ToString ());
		popuptext.color = Color.green;
	}
}
