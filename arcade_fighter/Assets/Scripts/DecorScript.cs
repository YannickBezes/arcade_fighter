using UnityEngine;
using UnityEngine.UI;

public class DecorScript : MonoBehaviour {
	public float gravity;

	public Camera cam;
	public float camMinX, camMaxX, camMinY, camMaxY;
	private GameObject[] charactersList;
	private GameObject player1, player2;
	public int player1Selection, player2Selection;

	public KeyCode p2block, p2meleeAttack, p2rangedAttack;

	// Items stuff
	public GameObject[] items;
	public int itemSpawnRate;
	public float minSpawnPos, maxSpawnPos, spawnHeight;
	public bool canSpawnItem = true;

	private bool showPauseMenu = false;
	private GameObject pauseMenu;
	public KeyCode pause;
	private GameObject endMenu;
	private bool endMenuIsActive = false;

	public GameObject scoreP1, scoreP2;

	public AudioSource backgroundAudio;
	public AudioSource[] effectsAudios;

	private BattleCountdown countdown;

	public static bool firstPauseMenu = true;

	// Start is called before the first frame update
	void Start() {
		pauseMenu = GameObject.Find("PauseMenu");
		pauseMenu.SetActive(false);
		endMenu = GameObject.Find("EndMenu");
		endMenu.SetActive(false);

		DataScript.NumberOfGamesToWin = 3;
		scoreP1.GetComponent<Text>().text = DataScript.ScorePlayer1.ToString();
		scoreP2.GetComponent<Text>().text = DataScript.ScorePlayer2.ToString();

		Transform t = GameObject.Find("CharactersList").transform;

		charactersList = new GameObject[t.childCount];
		for (int i = 0; i < t.childCount; i++)
			charactersList[i] = t.GetChild(i).gameObject;

		foreach (GameObject go in charactersList)
			go.SetActive(false);

		Debug.Log("p1 : " + DataScript.P1selection + " | " + player1Selection);
		player1Selection = DataScript.P1selection;
		player2Selection = DataScript.P2selection;

		player1 = charactersList[player1Selection];
		player2 = charactersList[player2Selection];

		// Set Gravity to the players and set them active
		foreach (GameObject p in new GameObject[] { player1, player2 }) {
			p.SetActive(true);
			p.GetComponent<Rigidbody2D>().gravityScale = gravity;
		}

		player2.GetComponent<Player>().numberOfThisPlayer = 2;
		player2.layer = LayerMask.NameToLayer("Player2");
		player2.GetComponent<Player>().whatIsEnnemies = LayerMask.GetMask("Player1");
		player2.GetComponent<Player>().block = p2block;
		player2.GetComponent<Player>().meleAttack = p2meleeAttack;
		player2.GetComponent<Player>().rangedAttack = p2rangedAttack;
		player2.GetComponent<PlayerHealth>().healthSlider = GameObject.Find("HealthSlider2").GetComponent<Slider>();

		player2.transform.position = new Vector3(6.25f, player2.transform.position.y, player2.transform.position.z);
		player2.GetComponent<Player>().FlipPlayer();

		countdown = GameObject.FindGameObjectWithTag("DecorTag").GetComponent<BattleCountdown>();
		SharedVars sharedVars = GameObject.FindGameObjectWithTag("BackgroundLoadingScriptTag").GetComponent<SharedVars>();
		backgroundAudio.volume = sharedVars.GetMusicVolume();
		foreach (AudioSource a in effectsAudios)
			a.volume = sharedVars.GetEffectsVolme();
	}

	// Update is called once per frame
	void Update() {
		if (countdown.isGameReady) {

			MoveCamera();

			if (Random.Range(0, 1000) < itemSpawnRate && canSpawnItem) {
				Vector3 pos = new Vector3(Random.Range(minSpawnPos, maxSpawnPos), spawnHeight, 0);
				Instantiate(items[(int)Random.Range(0, items.Length)], pos, transform.rotation);
				canSpawnItem = false;
			}

			if (Input.GetKeyDown(pause)) {
				showPauseMenu = !showPauseMenu;
				PauseMenu();
			}

			// END MENU
			EndMenu();
		}
	}

	public void HidePauseMenu() {
		showPauseMenu = false;
		pauseMenu.SetActive(false);
		Time.timeScale = 1;
	}

	public void PauseMenu() {
		if (showPauseMenu) {
			if (backgroundAudio.isPlaying)
				backgroundAudio.Pause();
			pauseMenu.SetActive(true);
			Time.timeScale = 0;
			if (firstPauseMenu) {
				// If is the first frame select the button
				firstPauseMenu = false;
				GameObject.Find("ButtonResume").GetComponent<Button>().Select(); // Select the button Resume
			}
		} else {
			if (!backgroundAudio.isPlaying)
				backgroundAudio.Play();
			HidePauseMenu();
			firstPauseMenu = true;
		}
	}

	public void MoveCamera() {
		Vector3 midpoint = (player1.transform.position + player2.transform.position) / 2f;
		if (midpoint.x < camMinX) {
			cam.transform.position = new Vector3(camMinX, cam.transform.position.y, cam.transform.position.z);
		} else if (midpoint.x > camMaxX) {
			cam.transform.position = new Vector3(camMaxX, cam.transform.position.y, cam.transform.position.z);
		} else {
			cam.transform.position = new Vector3(midpoint.x, cam.transform.position.y, cam.transform.position.z);
		}
	}

	private void EndMenu() {
		// If one player is dead and the menu is not already active
		if (player1.GetComponent<Player>() != null && player2.GetComponent<Player>() != null && !endMenuIsActive) {
			if (player1.GetComponent<Player>().hp <= 0 || player2.GetComponent<Player>().hp <= 0) {
				Time.timeScale = 0;
				backgroundAudio.Pause();
				endMenu.SetActive(true); // Display end menu
				endMenuIsActive = true;
				Text playerWinText = GameObject.Find("PlayerWins").GetComponent<Text>();
				if (player1.GetComponent<Player>().hp <= 0) {
					// Player 2 win
					DataScript.ScorePlayer2++;
					DataScript.BuffPlayer1 = 1.25f; // Multiply life of player 1 by 25%
					if (DataScript.ScorePlayer2 >= DataScript.NumberOfGamesToWin) {
						playerWinText.text = player2.GetComponent<Player>().playerName + " win the match !";
						GameObject.Find("ButtonNextMatch").SetActive(false);
						GameObject.Find("ButtonNextMatchBuff").SetActive(false);
					} else {
						playerWinText.text = player2.GetComponent<Player>().playerName + " win the round !";
					}
				} else {
					// Player 1 win
					DataScript.ScorePlayer1++;
					DataScript.BuffPlayer2 = 1.25f; //Multiply life of player 2 by 25%
					if (DataScript.ScorePlayer1 >= DataScript.NumberOfGamesToWin) {
						playerWinText.text = player1.GetComponent<Player>().playerName + " win the match !";
						GameObject.Find("ButtonNextMatch").SetActive(false);
						GameObject.Find("ButtonNextMatchBuff").SetActive(false);
					} else {
						playerWinText.text = player1.GetComponent<Player>().playerName + " win the round !";
					}
				}

				// If the match is over select ButtonMainMenu either select ButtonNextMatch
				if (DataScript.ScorePlayer1 >= DataScript.NumberOfGamesToWin || DataScript.ScorePlayer2 >= DataScript.NumberOfGamesToWin) {
					GameObject.Find("ButtonMainMenu").GetComponent<Button>().Select();
				} else {
					GameObject.Find("ButtonNextMatch").GetComponent<Button>().Select();
				}

				BroadcastMessage("StopGameTime");
			}
		}
	}

	public GameObject GetPlayer1() {
		return player1;
	}

	public GameObject GetPlayer2() {
		return player2;
	}
}
