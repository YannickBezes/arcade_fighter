using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	private GameObject[] scenes;
	private int current_scene;
	private int current_avatar_p1;
	private int current_avatar_p2;
	private Sprite[] avatars;
	private GameObject avatar_p1;
	private GameObject avatar_p2;
	private GameObject battleImg;
	public Slider musicSlider;
	public Slider soundSlider;
    public int numberOfCharacters;

	private float prevMusicVolume;
	private float prevSoundVolume;

	private SharedVars vars;

	public void Start() {
		prevMusicVolume = musicSlider.value;
		prevSoundVolume = soundSlider.value;
	}

	public void StartGame() {
		Debug.Log("Start game");

		vars = this.GetComponent<SharedVars>();

		vars.SetSceneBackgroundIdx(current_scene);
		vars.SetAvatarIdxP1(current_avatar_p1);
		vars.SetAvatarIdxP2(current_avatar_p2);

        DataScript.P1selection = current_avatar_p1;
        DataScript.P2selection = current_avatar_p2;

        Debug.Log(vars.GetSceneBackgroundIdx());

		DataScript.ScorePlayer1 = 0;
		DataScript.ScorePlayer2 = 0;
		DataScript.NumberOfGamesToWin = 3;
		DataScript.BuffPlayer1 = 1;
		DataScript.BuffPlayer2 = 1;

		StartCoroutine(WaitSecondsBeforeLoadGame(0.5f));
		StartCoroutine(BattleImageCoroutine(0.01f));
	}


	private IEnumerator BattleImageCoroutine(float seconds) {
		yield return new WaitForSecondsRealtime(seconds);
		battleImg = GameObject.FindGameObjectWithTag("TagBattle");
		battleImg.AddComponent<Outline>();
		battleImg.GetComponent<Outline>().effectColor = new Color(0.0f, 0.0f, 0.0f);
		battleImg.GetComponent<Outline>().effectDistance = new Vector2(2, 2);
		battleImg.transform.localScale = new Vector2(2.0f, -2.0f);
	}

	private IEnumerator WaitSecondsBeforeLoadGame(float seconds) {
		yield return new WaitForSecondsRealtime(seconds);
		SceneManager.LoadScene("Game");
	}

	public void quitGame() {
		Application.Quit();
	}

	public void OnButtonSubmit(GameObject btn) {
		string btn_to_find;
		switch (btn.name) {
			case "ButtonBack":
				// Back in the main menu
				// In the main menu there is not a ButtonBack so select another button
				if (btn.transform.parent.name == "OptionsPanel" || btn.transform.parent.name == "PreGameMenu") {
					btn_to_find = "ButtonStart";
				} else {
					btn_to_find = "ButtonBack";
				}
				break;

			case "ButtonConfirm":
				btn_to_find = "ButtonStart";
				break;

			case "ButtonStart":
				btn_to_find = "ButtonStart";
				break;

			default:
				btn_to_find = "ButtonBack";
				break;
		}

		Button next_button = GameObject.Find(btn_to_find).GetComponent<Button>();
		next_button.Select();

		OnButtonHoverExit(btn); // Clear effect
	}

	public void OnButtonHoverEnter(GameObject btn) {
		RectTransform transform = btn.GetComponent<RectTransform>();
		transform.localScale = transform.localScale * 0.95f;
	}

	public void OnButtonHoverExit(GameObject btn) {
		btn.GetComponent<RectTransform>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}

	public void OnPreMenuEnter(GameObject bg) {
		bg.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.25f, 0.3f, 1.0f);
		load_resources();
	}

	public void OnPreMenuExit(GameObject bg) {
        foreach (GameObject scene in scenes)
        {
            scene.SetActive(true);
        }
        bg.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}

	public void NextScene() {
		scenes[current_scene++].SetActive(false);

		if (current_scene > 5)
			current_scene -= 6;

		scenes[current_scene].SetActive(true);
	}

	public void PrevScene() {
		scenes[current_scene--].SetActive(false);

		if (current_scene < 0)
			current_scene += 6;

		scenes[(current_scene % 6 + 6) % 6].SetActive(true);
	}

	public void NextAvatar(bool isP1) {
		if (isP1) {
			Debug.Log("P1");
			current_avatar_p1 += 1;
			current_avatar_p1 %= numberOfCharacters;
			Debug.Log(current_avatar_p1);
			avatar_p1.GetComponent<Image>().sprite = avatars[current_avatar_p1];
		} else {
			Debug.Log("P2");
			current_avatar_p2 += 1;
			current_avatar_p2 %= numberOfCharacters;
			Debug.Log(current_avatar_p2);
			avatar_p2.GetComponent<Image>().sprite = avatars[current_avatar_p2];
		}
	}

	public void PrevAvatar(bool isP1) {
		if (isP1) {
			Debug.Log("P1");
			if (current_avatar_p1 == 0)
				current_avatar_p1 = numberOfCharacters - 1;
			else
				current_avatar_p1 -= 1;
			Debug.Log(current_avatar_p1);
			avatar_p1.GetComponent<Image>().sprite = avatars[current_avatar_p1];
		} else {
			Debug.Log("P2");
			if (current_avatar_p2 == 0)
				current_avatar_p2 = numberOfCharacters - 1;
			else {
				current_avatar_p2 -= 1;
			}
			Debug.Log(current_avatar_p2);
			avatar_p2.GetComponent<Image>().sprite = avatars[current_avatar_p2];
		}
	}

	private void load_resources() {
		scenes = GameObject.FindGameObjectsWithTag("scenes");

		if (scenes == null) {
			Debug.Log("Scenes do not exist !");
		}

		foreach (GameObject scene in scenes) {
			scene.SetActive(false);
		}

		current_scene = 0;
		scenes[current_scene].SetActive(true);

		avatars = Resources.LoadAll<Sprite>("Characters");
		Debug.Log(avatars.Length);

		current_avatar_p1 = 0;
		current_avatar_p2 = 0;

		avatar_p1 = GameObject.FindGameObjectWithTag("TagAvatarP1");
		avatar_p2 = GameObject.FindGameObjectWithTag("TagAvatarP2");
		avatar_p1.GetComponent<Image>().sprite = avatars[current_avatar_p1];
		avatar_p2.GetComponent<Image>().sprite = avatars[current_avatar_p2];

		//battleImg = GameObject.FindGameObjectWithTag("TagBattle");
	}

	public void UpdateVolume() {
		SharedVars sharedVars = GameObject.FindGameObjectWithTag("ScriptTag").GetComponent<SharedVars>();
		sharedVars.SetMusicVolume(musicSlider.value);
		sharedVars.SetEffectsVolume(soundSlider.value);
	}

	public void CancelUpdateVolume() {
		musicSlider.value = prevMusicVolume;
		soundSlider.value = prevSoundVolume;
	}

	public void Update() {

	}
}
