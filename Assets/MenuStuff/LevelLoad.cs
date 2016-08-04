using UnityEngine;
using System.Collections;

public class LevelLoad : MonoBehaviour {

    public void LoadTutorial()
    {
        Application.LoadLevel("Level1");
    }

    public void LoadLevel1()
    {
        Application.LoadLevel("Level1-rework");
    }

    public void LoadLevel2()
    {
        Application.LoadLevel("AdvLevel");
    }
}
