using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonMethods : MonoBehaviour
{
    [SerializeField]
    public GameObject panel;

    [SerializeField]
    public GameObject gamePausedText;

    public Text scoreTxt, highscoreTxt;
    public int score;

    private void Start()
    {
        highscoreTxt.text = PlayerPrefs.GetInt("highscore").ToString();
        score = 0;
        scoreTxt.text = score.ToString();
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void ActivatePanel()
    {
        panel.SetActive(true);
    }

    public void PauseGame()
    {
        if(Time.timeScale == 1)
        {
            gamePausedText.SetActive(true);
            Time.timeScale = 0;
        }
        else if(Time.timeScale == 0)
        {
            gamePausedText.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void UpdateScore(int lines)
    {
        if (lines == 1)
        {
            score += 100;
            scoreTxt.text = score.ToString();
        }
        if (lines == 2)
        {
            score += 400;
            scoreTxt.text = score.ToString();
        }
        if (lines == 3)
        {
            score += 1000;
            scoreTxt.text = score.ToString();
        }
        if (lines == 4)
        {
            score += 3000;
            scoreTxt.text = score.ToString();
        }
    }

    public void CheckForHighScore()
    {
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
        }
    }
}
