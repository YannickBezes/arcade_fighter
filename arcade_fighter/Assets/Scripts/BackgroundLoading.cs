using UnityEngine;
using UnityEngine.UI;

public class BackgroundLoading : MonoBehaviour {
	private SharedVars vars;
    public string[] audios = {
        "Audios/Funky-Gameplay",
        "Audios/Mech-Street-Fighters",
        "Audios/Rockin-with-Caped-Crusader-Cat"
    };

	private string[] scenePath = {
		"Background/Scene1",
		"Background/Scene2",
		"Background/Scene3",
		"Background/Scene4",
		"Background/Scene5",
		"Background/Scene6"
	};

	private Vector3[] sceneScaleFactors = {
		new Vector3(6.2f, 6.3f, 1.0f),
		new Vector3(6.2f, 6.3f, 1.0f),
		new Vector3(5.8f, 5.9f, 1.0f),
		new Vector3(6.2f, 6.3f, 1.0f),
		new Vector3(6.2f, 6.3f, 1.0f),
		new Vector3(5.5f, 5.6f, 1.0f)
	};

	private Vector3[] groundPositions = {
		new Vector3(-7.5f, -2.2f, 0.0f),
		new Vector3(-7.5f, -2.2f, 0.0f),
		new Vector3(-7.5f, -2.2f, 0.0f),
		new Vector3(-7.5f, -2.2f, 0.0f),
		new Vector3(-7.5f, -2.2f, 0.0f),
		new Vector3(-7.5f, -2.2f, 0.0f)
	};

    public AudioSource audioSource;

	// Start is called before the first frame update
	void Start() {
        vars = GetComponent<SharedVars>();

		int idx = vars.GetSceneBackgroundIdx();

        // Load background music
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Instantiate(Resources.Load(audios[(idx + Random.Range(0, audios.Length)) % audios.Length])) as AudioClip;
        audioSource.loop = true;

        GameObject bg = Instantiate(Resources.Load(scenePath[idx]) as GameObject);

		bg.transform.position = new Vector3(0.0f, 0.0f, 120.0f);
		bg.transform.localScale = sceneScaleFactors[idx];

		GameObject.FindGameObjectWithTag("Ground").transform.position = groundPositions[idx];

		Sprite[] avatars = Resources.LoadAll<Sprite>("Characters"); // Get avatarts

		// Set the avatar of players
		GameObject.FindGameObjectWithTag("TagAvatarP1").GetComponent<Image>().sprite = avatars[vars.GetAvatarIdxP1()];
		GameObject.FindGameObjectWithTag("TagAvatarP2").GetComponent<Image>().sprite = avatars[vars.GetAvatarIdxP2()];
	}

	// Update is called once per frame
	void Update() {
        if (!audioSource.isPlaying && GameObject.Find("Decor").GetComponent<BattleCountdown>().isGameReady)
        {
            audioSource.Play();
        }
    }
}
