using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }
    public void PlayGame()
    {        
        SceneManager.LoadScene("GameScene");
    }
    public void QuitGame()
    {
        Debug.Log("Quit the game.");
        Application.Quit();
    }
    private void CheckOptions()
    {
        if (GameObject.FindGameObjectWithTag("volume").GetComponent<Toggle>().isOn)
            GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().mute = true;
        else
            GameObject.FindGameObjectWithTag("music").GetComponent<AudioSource>().mute = false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
