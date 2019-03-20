﻿using UnityEngine;
using UnityEngine.UI;

public class DecorScript : MonoBehaviour {
    public float gravity;

    public Camera cam;
    public float camMinX, camMaxX, camMinY, camMaxY;
    private GameObject[] charactersList;
    private GameObject player1, player2;
    public int player1Selection, player2Selection;

    // Items stuff
    public GameObject[] items;
    public int itemSpawnRate;
    public float minSpawnPos, maxSpawnPos, spawnHeight;
    public bool canSpawnItem = true;

    private bool showPauseMenu = false;
    private GameObject pauseMenu;
    public KeyCode pause;
    private GameObject endMenu;

    // Start is called before the first frame update
    void Start() {
        pauseMenu = GameObject.Find("PauseMenu");
        endMenu = GameObject.Find("EndMenu");
        endMenu.SetActive(false);

        Transform t = GameObject.Find("CharactersList").transform;

        charactersList = new GameObject[t.childCount];
        for (int i = 0; i < t.childCount; i++)
            charactersList[i] = t.GetChild(i).gameObject;

        foreach (GameObject go in charactersList)
            go.SetActive(false);

        player1 = charactersList[player1Selection];
        player2 = charactersList[player2Selection];

		// Set Gravity to the players and set them active
        foreach (GameObject p in new GameObject[] {player1, player2}) {
            p.SetActive(true);
            p.GetComponent<Rigidbody2D>().gravityScale = gravity;
        }

    }

    // Update is called once per frame
    void Update() {
        MoveCamera();

        if(Random.Range(0, 1000) < itemSpawnRate && canSpawnItem) {
            Vector3 pos = new Vector3(Random.Range(minSpawnPos, maxSpawnPos), spawnHeight, 0);
            Instantiate(items[(int)Random.Range(0, items.Length)], pos, transform.rotation);
            canSpawnItem = false;
        }

		// Pause
		if(Input.GetKeyDown(pause)) {
            showPauseMenu = !showPauseMenu;
        }

        if (showPauseMenu) {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        } else {
            HidePauseMenu();
        }

		// END MENU
		EndMenu();
    }

    public void HidePauseMenu() {
        showPauseMenu = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void MoveCamera() {
        Vector3 midpoint = (player1.transform.position + player2.transform.position) / 2f;

        if(midpoint.x < camMinX) {
            cam.transform.position = new Vector3(camMinX, cam.transform.position.y, cam.transform.position.z);
        } else if (midpoint.x > camMaxX) {
            cam.transform.position = new Vector3(camMaxX, cam.transform.position.y, cam.transform.position.z);
        } else {
            cam.transform.position = new Vector3(midpoint.x, cam.transform.position.y, cam.transform.position.z);
        }
    }

	private void EndMenu() {
		if (player1.GetComponent<Player>() != null && player2.GetComponent<Player>() != null) {
			if (player1.GetComponent<Player>().hp <= 0 || player2.GetComponent<Player>().hp <= 0) {
				Time.timeScale = 0;
				endMenu.SetActive(true); // Display end menu
				Text playerWinText = GameObject.Find("PlayerWins").GetComponent<Text>();
				if (player1.GetComponent<Player>().hp <= 0) {
					// Player 2 win
					playerWinText.text = player2.GetComponent<Player>().playerName + " win !";
				} else {
					// Player 1 win
					playerWinText.text = player1.GetComponent<Player>().playerName + " win !";
				}
			}
		}
	}
    
    public GameObject GetPlayer1()
    {
        return player1;
    }

    public GameObject GetPlayer2()
    {
        return player2;
    }
}
