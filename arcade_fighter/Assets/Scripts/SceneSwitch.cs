using UnityEngine;

public class SceneSwitch : MonoBehaviour {
	private GameObject[] scenes;
	private int current_scene;
	public float moveSpeed;

	// Start is called before the first frame update
	void Start() {
		scenes = GameObject.FindGameObjectsWithTag("scenes");

		moveSpeed = 5.0f;

		if (scenes == null) {
			Debug.Log("Scenes do not exist !");
		}

		foreach (GameObject scene in scenes) {
			scene.SetActive(false);
		}

		current_scene = 0;
		scenes[current_scene].SetActive(true);
	}

	// Update is called once per frame
	void Update() {
		// Up and Down for changing the scene
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			// previous scene
			scenes[current_scene--].SetActive(false);

			if (current_scene < 0)
				current_scene += 6;

			scenes[(current_scene % 6 + 6) % 6].SetActive(true);
		}

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			// next scene
			scenes[current_scene++].SetActive(false);

			if (current_scene > 5)
				current_scene -= 6;

			scenes[current_scene].SetActive(true);
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			// move the camera left
			if (transform.position.x > -7.0f) {
				float step = moveSpeed * Time.deltaTime;
				transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(-0.5f, 0.0f, 0.0f), step);
			}
		}

		if (Input.GetKey(KeyCode.RightArrow)) {
			// move the camera right
			if (transform.position.x < 7.0f) {
				float step = moveSpeed * Time.deltaTime;
				transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0.5f, 0.0f, 0.0f), step);
			}
		}
	}

}
