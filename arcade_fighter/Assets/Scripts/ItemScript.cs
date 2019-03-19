using UnityEngine;

public class ItemScript : MonoBehaviour {
    public float hpModifier, attackModifier, rangeModifier, speedModifier, duration;
    private float timeItemTaken;
    private Collider2D player;
    private bool gathered;

	// Start is called before the first frame update
	void Start() { }

    // Update is called once per frame
    void Update() {
        if(Time.time > timeItemTaken + duration && gathered == true) {
            player.GetComponent<Player>().ChangeStats(0, -attackModifier, -rangeModifier, -speedModifier);
			player.GetComponent<Player>().attackBoosted = false;
            Debug.Log("L'effet de l'item se dissipe....");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Debug.Log("Le joueur " + other.GetComponent<Player>().numberOfThisPlayer + " vient de ramasser un item : " + Time.time);
            timeItemTaken = Time.time;
            player = other;
            gathered = true;
            other.GetComponent<Player>().ChangeStats(hpModifier, attackModifier, rangeModifier, speedModifier);
            GameObject.Find("Decor").GetComponent<DecorScript>().canSpawnItem = true;
            transform.position = new Vector3(5000, 0);
        }
    }
}
