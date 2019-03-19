using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private GameObject[] scenes;
    private int current_scene;
    private int current_avatar_p1;
    private int current_avatar_p2;
    private Sprite[] avatars;
    private GameObject avatar_p1;
    private GameObject avatar_p2;
    private GameObject battleImg;

    public void Start() {
       
    }

    public void StartGame() {
        SceneManager.LoadScene("Game");
    }

    public void quitGame() {
        Application.Quit();
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
            current_avatar_p1 %= 17;
            Debug.Log(current_avatar_p1);
            avatar_p1.GetComponent<Image>().sprite = avatars[current_avatar_p1];
        } else {
            Debug.Log("P2");
            current_avatar_p2 += 1;
            current_avatar_p2 %= 17;
            Debug.Log(current_avatar_p2);
            avatar_p2.GetComponent<Image>().sprite = avatars[current_avatar_p2];
        }
    }

    public void PrevAvatar(bool isP1) {
        if (isP1) {
            Debug.Log("P1");
            if (current_avatar_p1 == 0)
                current_avatar_p1 = 16;
            else
                current_avatar_p1 -= 1;
            Debug.Log(current_avatar_p1);
            avatar_p1.GetComponent<Image>().sprite = avatars[current_avatar_p1];
        } else {
            Debug.Log("P2");
            if (current_avatar_p2 == 0)
                current_avatar_p2 = 16;
            else
                current_avatar_p2 -= 1;
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
        current_avatar_p2 = 4;

        avatar_p1 = GameObject.FindGameObjectWithTag("TagAvatarP1");
        avatar_p2 = GameObject.FindGameObjectWithTag("TagAvatarP2");

        //battleImg = GameObject.FindGameObjectWithTag("TagBattle");
    }
}