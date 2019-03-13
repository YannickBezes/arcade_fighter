using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public void resume()
    {
        GameObject.Find("Decor").GetComponent<DecorScript>().HidePauseMenu();
    }

    public void restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
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
