using UnityEngine;

public class ProjectileScript : MonoBehaviour {
	public float projectileSpeed;
	public float damage;

	private Rigidbody2D rb;

	// Start is called before the first frame update
	void Start() {
		rb = GetComponent<Rigidbody2D>();
	}

	// Update is called once per frame
	void Update() {
		rb.velocity = new Vector2(projectileSpeed * transform.localScale.x, 0);
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.tag == "Player") {
			Player player = collision.GetComponent<Player>();
			if (player != null)
				player.TakeDamage(damage);
		}
		if (collision.tag == "DecorItem") {
			collision.GetComponent<DecorItemScript>().TakeDamage(damage);
		}
		Destroy(gameObject);
	}
}
