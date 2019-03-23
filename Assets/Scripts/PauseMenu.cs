using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Awake pausemenu");
        if (SceneManager.GetActiveScene().name == "GameOverMenu")
            SetResult();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void RepeatGame()
    {
        SceneManager.LoadScene("GameScene");
        Time.timeScale = 1;
    }
    public void ShowMainMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }
    public void ResumeGame()
    {
        SceneManager.LoadSceneAsync("GameScene");
        SceneManager.UnloadSceneAsync("PauseMenu");
        Time.timeScale = 1;
    }
    public void SetResult()
    {
        Debug.Log("Result set");
        int resultP1, resultP2, winner;
        winner = PlayerPrefs.GetInt("winner");
        resultP1 = PlayerPrefs.GetInt("p1Result");
        resultP2 = PlayerPrefs.GetInt("p2Result");
        GameObject.FindGameObjectWithTag("result").GetComponent<Text>().text = resultP1+" - "+resultP2; 
        GameObject.FindGameObjectWithTag("winner").GetComponent<Text>().text = "PLAYER " + winner + " WINS";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
