using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void startGame()
    {
        DataScript.ScorePlayer1 = 0;
        DataScript.ScorePlayer2 = 0;
        DataScript.NumberOfGamesToWin = 3;
        DataScript.BuffPlayer1 = 1;
        DataScript.BuffPlayer2 = 1;
        SceneManager.LoadScene("Game");
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void OnButtonHoverEnter(GameObject btn)
    {
        RectTransform transform = btn.GetComponent<RectTransform>();
        transform.localScale = transform.localScale * 0.95f;
    }

    public void OnButtonHoverExit(GameObject btn)
    {
        btn.GetComponent<RectTransform>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }
}
