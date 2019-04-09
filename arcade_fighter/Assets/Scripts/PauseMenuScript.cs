using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {
	public void resume() {
		GameObject.Find("Decor").GetComponent<DecorScript>().HidePauseMenu();
	}

	public void restart() {
		DataScript.ScorePlayer1 = 0;
		DataScript.ScorePlayer2 = 0;
		DataScript.BuffPlayer1 = 1;
		DataScript.BuffPlayer2 = 1;
		SceneManager.LoadScene("Game");
	}

	public void nextMatch() {
		DataScript.BuffPlayer1 = 1;
		DataScript.BuffPlayer2 = 1;
		SceneManager.LoadScene("Game");
	}

	public void nextMatchBuff() {
		SceneManager.LoadScene("Game");
	}

	public void mainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void OnButtonHoverEnter(GameObject btn) {
		RectTransform transform = btn.GetComponent<RectTransform>();
		transform.localScale = transform.localScale * 0.95f;
	}

	public void OnButtonHoverExit(GameObject btn) {
		btn.GetComponent<RectTransform>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}
}
