using UnityEngine;

public class DecorItemScript : MonoBehaviour {
	public float hp;
	public GameObject explosionEffect;
	public float explosionDamage, explosionRadius;
	private BoxCollider2D hitbox;

	// Start is called before the first frame update
	void Start() {
		hitbox = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update() {
		if (hp <= 0) {
			Explode();
		}
	}

	public void TakeDamage(float dmg) {
		hp -= dmg;
	}

	public void Explode() {
		Instantiate(explosionEffect, transform.position, transform.rotation);
		GameObject player1 = GameObject.Find("Decor").GetComponent<DecorScript>().GetPlayer1();
		GameObject player2 = GameObject.Find("Decor").GetComponent<DecorScript>().GetPlayer2();
		Debug.Log(hitbox.Distance(player1.GetComponent<BoxCollider2D>()).distance);
		if (hitbox.Distance(player1.GetComponent<BoxCollider2D>()).distance < explosionRadius) {
			player1.GetComponent<Player>().TakeDamage(explosionDamage);
		}
		if (hitbox.Distance(player2.GetComponent<BoxCollider2D>()).distance < explosionRadius) {
			player2.GetComponent<Player>().TakeDamage(explosionDamage);
		}
		Destroy(gameObject);
	}
}
