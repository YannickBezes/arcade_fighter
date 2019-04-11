using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	// Character's specifications (can be changed in unity's inspector)
	public string playerName;
	public float maxHp; // Initial health
	public float hp;
	public float attack;
	public float range;
	public float speed;
	public float crouchSpeedMultiplier;
	public float jumpForce;
	public int numberOfThisPlayer;

	public LayerMask whatIsEnnemies;
	public LayerMask whatIsBreakable;
	public float attackRange;

	public KeyCode block;
	public KeyCode meleAttack;
	public KeyCode rangedAttack;

	private bool isBlocking = false;

	public Transform groundCheckPoint;
	public bool isGrounded;
	public float groundCheckRadius;
	public LayerMask whatIsGround;

	private Animator animator;
	private Rigidbody2D rigidBody;
	public bool faceRight;

	public GameObject projectile;

	// Combos part
	public ListCombos combos;

	private BattleCountdown countdown;

	private int[] currentComboIndex = new int[6];
	public float timeBetweenAttacks = 0.5f;
	private float timeLastButtonPressed;

	// For query of PlayerHealth
	public bool isDamaged = false;

	// Boosted by item
	public bool attackBoosted = false;

	// Start is called before the first frame update
	void Start() {
		countdown = GameObject.FindGameObjectWithTag("DecorTag").GetComponent<BattleCountdown>();
		groundCheckPoint = transform.Find("GroundCheck");
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		if (numberOfThisPlayer == 1)
			hp *= DataScript.BuffPlayer1;
		else
			hp *= DataScript.BuffPlayer2;
		// hp = maxHp; // useful ??? rather maxhp = hp ?
	}

	// Update is called once per frame
	void Update() {
		if (countdown.isGameReady) {
			isGrounded = false;
			Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPoint.position, groundCheckRadius, whatIsGround);
			for (int i = 0; i < colliders.Length; i++) {
				if (colliders[i].gameObject != gameObject)
					isGrounded = true;
			}
			animator.SetBool("Ground", isGrounded);
			animator.SetFloat("vSpeed", rigidBody.velocity.y);

			Block();
			// Attack method
			if (!isBlocking) { // If the player isn't blocking he can move and attack
							   // if this not a combo launch an normal attack
				if (!IsCombo()) {
					RangedAttack();
					MeleAttack();
				}


				// Move
				Move();

				//float move = 0;
				//if (Input.GetKey(left)) {
				//	move = -1;
				//} else if (Input.GetKey(right)) {
				//	move = 1;
				//}
				//bool isJumping = Input.GetKeyDown(jump);
				//bool isCrouching = Input.GetKey(crouch);
				//Move(move, isCrouching, isJumping);
			}
		}
	}

	public void Move(float distance = 1f) {
		if (distance == 1f) { // Default parameter 
			distance = Input.GetAxis("Horizontal");
		}

		bool isCrouching = Input.GetAxis("Vertical") < 0 ? true : false;
		bool isJumping = Input.GetAxis("Vertical") > 0 ? true : false;

		animator.SetBool("Crouch", isCrouching);
		distance = (isCrouching ? distance * crouchSpeedMultiplier : distance);
		animator.SetFloat("Speed", Mathf.Abs(distance));

		rigidBody.velocity = new Vector2(distance * speed, rigidBody.velocity.y);

		// If the input is moving the player right and the player is facing left...
		if (distance > 0 && !faceRight) {
			// ... flip the player.
			FlipPlayer();
		} else if (distance < 0 && faceRight) { // Otherwise if the input is moving the player left and the player is facing right...
												// ... flip the player.
			FlipPlayer();
		}

		if (isGrounded && isJumping) {
			// Add a vertical force to the player.
			isGrounded = false;
			animator.SetBool("Ground", false);
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
		}
	}

	public void FlipPlayer() {
		faceRight = !faceRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void TakeDamage(float damage) {
		// If the player doesn't counter the attack he take damage
		if (!isBlocking) {
			hp -= damage;
			Debug.Log("Damage Taken !" + damage);
		} else {
			hp -= damage * 0.25f;
			Debug.Log("Damage Taken !" + damage * 0.25f);
		}

		StartCoroutine(SpriteFlash());
	}

	IEnumerator SpriteFlash() {
		SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
		sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
		yield return new WaitForSeconds(0.05f);
		sprite.color = new Color(1.0f, 1.0f, 1.0f);
		yield return new WaitForSeconds(0.1f);
		sprite.color = new Color(1.0f, 1.0f, 1.0f, 0.4f);
		yield return new WaitForSeconds(0.05f);
		sprite.color = new Color(1.0f, 1.0f, 1.0f);
	}

	public void MeleAttack() {
		if (Input.GetKeyDown(meleAttack)) {
			animator.SetTrigger("MeleAttack"); // Start the animation "meleAttack"
			float postion = faceRight ? 2f : -2f;
			Collider2D[] ennemyColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + postion, transform.position.y), attackRange, whatIsEnnemies);
			if (ennemyColliders.Length != 0) { // if the array isn't empty there is an ennemy in range
											   // Get the first collider of the ennemy and hit it
				ennemyColliders[0].GetComponent<Player>().TakeDamage(attack);
			}
			Collider2D[] breakableColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + postion, transform.position.y), attackRange, whatIsBreakable);
			if (breakableColliders.Length != 0) {
				breakableColliders[0].GetComponent<DecorItemScript>().TakeDamage(attack * 1.5f);
			}
		}
	}

	public void Block() {
		if (Input.GetKey(block)) {
			isBlocking = true;
			Move(0);
		} else isBlocking = false;
		// Launch the annimation
		animator.SetBool("Block", isBlocking);
	}

	public void RangedAttack() {
		if (Input.GetKeyDown(rangedAttack)) {
			//Ranged attack here
			GameObject p;
			if (!faceRight) {
				p = Instantiate(projectile, new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), transform.rotation);
				Vector3 theScale = p.transform.localScale;
				theScale.x *= -1;
				p.transform.localScale = theScale;
			} else {
				p = Instantiate(projectile, new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), transform.rotation);
			}
			p.GetComponent<ProjectileScript>().damage = attack * 0.5f; // Set the damage of the projectile
			animator.SetTrigger("RangedAttack");
		}
	}

	// Combos part
	private bool CheckCombo(int comboNumber, List<KeyCode> combo) {
		// If the time is greater than 0.5 ms, reset combo
		if (Time.time > timeLastButtonPressed + timeBetweenAttacks) {
			currentComboIndex[comboNumber] = 0;
		}

		if (currentComboIndex[comboNumber] < combo.Count) {
			if (Input.GetKeyDown(combo[currentComboIndex[comboNumber]])) {
				timeLastButtonPressed = Time.time;
				currentComboIndex[comboNumber]++;
			} else if (Input.anyKeyDown) {
				currentComboIndex[comboNumber] = 0;
				return false;
			}

			if (currentComboIndex[comboNumber] == combo.Count) {
				currentComboIndex[comboNumber] = 0;
				return true;
			}
		}
		return false;
	}

	public bool IsCombo() {
		for (int i = 0; i < combos.listCombos.Count; i++) {
			Combo combo = combos.listCombos[i];
			if (CheckCombo(i, combo.listKeyCode)) {
				// Detect if the last input of the combo is an range attack or mele attack
				float old_attack = attack; // Save attack
				attack = combo.damage;
				if (combo.listKeyCode[combo.listKeyCode.Count - 1] == rangedAttack) {
					RangedAttack();
				} else {
					MeleAttack();
				}
				attack = old_attack;
				Debug.Log("Combo " + i);
				return true; // This is a combo
			}
		}
		return false; // This is not a combo
	}

	public void ChangeStats(float hpModifier, float attackModifier, float rangeModifier, float speedModifier) {
		// HP can't be superior at 100
		if (hp < 100)
			hp = (hp + hpModifier) > 100 ? 100 : hp + hpModifier;

		if (!attackBoosted) {
			attack += attackModifier;
			attackRange += rangeModifier;
			attackBoosted = true;
		}
		speed += speedModifier;
	}
}
