using UnityEngine;

public class ItemScript : MonoBehaviour {
	public float hpModifier, attackModifier, rangeModifier, speedModifier, duration;
    public string audioSourceName;
	private float timeItemTaken;
	private Collider2D player;
	private bool gathered;
    private AudioSource audioSource;

	// Start is called before the first frame update
	void Start() {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = Resources.Load("Audios/" + audioSourceName) as AudioClip;
        audioSource.playOnAwake = false;
    }

	// Update is called once per frame
	void Update() {
		if (Time.time > timeItemTaken + duration && gathered == true) {
			player.GetComponent<Player>().ChangeStats(0, -attackModifier, -rangeModifier, -speedModifier);
			player.GetComponent<Player>().attackBoosted = false;
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player") {
			timeItemTaken = Time.time;
			player = other;
			gathered = true;
			other.GetComponent<Player>().ChangeStats(hpModifier, attackModifier, rangeModifier, speedModifier);
			GameObject.Find("Decor").GetComponent<DecorScript>().canSpawnItem = true;
			transform.position = new Vector3(5000, 0);
            audioSource.Play();
        }
	}
}
