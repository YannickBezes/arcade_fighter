using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorScript : MonoBehaviour
{
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

        if(Input.GetKeyDown(pause)) {
            showPauseMenu = !showPauseMenu;
        }

        if (showPauseMenu) {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        } else {
            HidePauseMenu();
        }


		if(player1.GetComponent<Player>() != null && player2.GetComponent<Player>() != null) {
			if (player1.GetComponent<Player>().hp <= 0 || player2.GetComponent<Player>().hp <= 0) {
				Time.timeScale = 0;
				endMenu.SetActive(true);
				if (player1.GetComponent<Player>().hp <= 0) {
					if (GameObject.Find("Player1Wins") != null) {
						GameObject.Find("Player1Wins").SetActive(false);
					}
				} else {
					if (GameObject.Find("Player2Wins") != null) {
						GameObject.Find("Player2Wins").SetActive(false);
					}
				}
			}
		}
        
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
}
