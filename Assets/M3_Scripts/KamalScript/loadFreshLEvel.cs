using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class loadFreshLEvel : MonoBehaviour {
	public GameObject loadIm;
	public void loadfresh (int level){
		loadIm.SetActive (true);
		SceneManager.LoadScene(level);
	}
}
