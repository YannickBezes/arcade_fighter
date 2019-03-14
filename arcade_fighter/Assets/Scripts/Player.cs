using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

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

	private float timeBtwnMeleAttack;
	public float startTimeBtwnMeleAttack; // Start time between attack

	public LayerMask whatIsEnnemies;
    public LayerMask whatIsBreakable;
    public float attackRange;

	public KeyCode left;
	public KeyCode right;
	public KeyCode jump;
	public KeyCode crouch;
	public KeyCode rangedAttack;
	public KeyCode block;
	public KeyCode meleAttack;

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
    public KeyCode[] combo1;
    private int[] currentComboIndex = new int[6];
    public float timeBetweenAttacks = 0.5f;
    private float timeLastButtonPressed;

	// For query of PlayerHealth
	public bool isDamaged = false;

	// Start is called before the first frame update
	void Start() {
		groundCheckPoint = transform.Find("GroundCheck");
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		hp = maxHp;
	}

	// Update is called once per frame
	void Update() {
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
			RangedAttack();
			MeleAttack();

			if (CheckCombo(1, combo1)) {
				Debug.Log("Combo 1");
			}

			// Move
			float move = 0;
			if (Input.GetKey(left)) {
				move = -1;
			} else if (Input.GetKey(right)) {
				move = 1;
			}
			bool isJumping = Input.GetKeyDown(jump);
			bool isCrouching = Input.GetKey(crouch);
			Move(move, isCrouching, isJumping);
		}
	}

	public void Move(float d, bool isCrouching, bool isJumping) {
		animator.SetBool("Crouch", isCrouching);
		d = (isCrouching ? d * crouchSpeedMultiplier : d);
		animator.SetFloat("Speed", Mathf.Abs(d));

		rigidBody.velocity = new Vector2(d * speed, rigidBody.velocity.y);

		// If the input is moving the player right and the player is facing left...
		if (d > 0 && !faceRight) {
			// ... flip the player.
			FlipPlayer();
		} else if (d < 0 && faceRight) { // Otherwise if the input is moving the player left and the player is facing right...
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
			isDamaged = true;
			Debug.Log("Damage Taken !");
		} else {
			isDamaged = false;
		}
	}

	public void MeleAttack() {
		if (Input.GetKeyDown(meleAttack)) {
			animator.SetTrigger("MeleAttack"); // Start the animation "meleAttack"
            float postion = faceRight ? 2f : -2f;
            Collider2D[] ennemyColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + postion, transform.position.y), attackRange, whatIsEnnemies);
            if (ennemyColliders.Length != 0) { // if the array isn't empty there is an ennemy in range
                                               // Get the first collider of the ennemy and hit it
                ennemyColliders[0].GetComponent<Player>().TakeDamage(attack * 1.5f);
            }
            Collider2D[] breakableColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + postion, transform.position.y), attackRange, whatIsBreakable);
            if (breakableColliders.Length != 0)
            {
                breakableColliders[0].GetComponent<DecorItemScript>().TakeDamage(attack * 1.5f);
            }
        }
	}

	public void Block() {
		if (Input.GetKey(block)) {
			isBlocking = true;
			Move(0, false, false);
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
	public bool CheckCombo(int comboNumber, KeyCode[] combo) {
		if (Time.time > timeLastButtonPressed + timeBetweenAttacks) {
			currentComboIndex[comboNumber] = 0;
		}

		if (currentComboIndex[comboNumber] < combo.Length) {
			if (Input.GetKeyDown(combo[currentComboIndex[comboNumber]])) {
				timeLastButtonPressed = Time.time;
				currentComboIndex[comboNumber]++;
			} else if (Input.anyKeyDown) {
				currentComboIndex[comboNumber] = 0;
				return false;
			}

			if (currentComboIndex[comboNumber] == combo.Length) {
				currentComboIndex[comboNumber] = 0;
				return true;
			}
		}

		return false;
	}

	public void ChangeStats(float hpModifier, float attackModifier, float rangeModifier, float speedModifier)
    {
        hp += hpModifier;
        attack += attackModifier;
        range += rangeModifier;
        speed += speedModifier;
    }
}   
