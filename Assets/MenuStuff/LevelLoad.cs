using UnityEngine;
using System.Collections;

public class LevelLoad : MonoBehaviour {

    public void LoadTutorial()
    {
        Application.LoadLevel("Level0");
    }

    public void LoadLevel1()
    {
        Application.LoadLevel("Level-rework");
    }

    public void LoadLevel2()
    {
        Application.LoadLevel("AdvLevel");
    }
}
