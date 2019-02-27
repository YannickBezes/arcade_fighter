using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerScript : MonoBehaviour
{
    // Character's specifications (can be changed in unity's inspector)
    public float hp;
    public float attack;
    public float range;
    public float speed;
    public float crouchSpeedMultiplier;
    public float jumpForce;
    public int numberOfThisPlayer;

	private float timeBtwnAttack;
	public float startTimeBtwnAttack; // Start time between attack

	public LayerMask whatIsEnnemies;
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
    private bool faceRight = true;

    public GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        if (numberOfThisPlayer == 2)
            FlipPlayer();
        groundCheckPoint = transform.Find("GroundCheck");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
		if(hp <= 0) {
			Destroy(gameObject);
		}

        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                isGrounded = true;
        }
        animator.SetBool("Ground", isGrounded);
        animator.SetFloat("vSpeed", rigidBody.velocity.y);

        isBlocking = false;
        if(Input.GetKey(block))
        {
            isBlocking = true;
        }
        animator.SetBool("Block", isBlocking);
        if (isBlocking == true)
        {
            Move(0, false, false);
        }
        else
        {
			if (timeBtwnAttack <= 0) {
				// Then we can attack
				if (Input.GetKey(meleAttack)) {
					animator.SetTrigger("MeleAttack");
					Collider2D[] ennemiesToDamage = Physics2D.OverlapCircleAll(new Vector2(transform.position.x + 2f, transform.position.y), attackRange, whatIsEnnemies);

					foreach (Collider2D colider in ennemiesToDamage) {
						colider.GetComponent<PlayerScript>().TakeDamage(attack * 1.5f);
					}
				}

				timeBtwnAttack = startTimeBtwnAttack;
			} else {
				timeBtwnAttack -= Time.deltaTime;
			}

			if (Input.GetKeyDown(rangedAttack))
            {
                //Ranged attack here

                if (!faceRight)
                {
                    GameObject p = Instantiate(projectile, new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z), transform.rotation);
                    Vector3 theScale = p.transform.localScale;
                    theScale.x *= -1;
                    p.transform.localScale = theScale;
                }
                else
                {
                    GameObject p = Instantiate(projectile, new Vector3(transform.position.x + 1.5f, transform.position.y, transform.position.z), transform.rotation);
                }

                animator.SetTrigger("RangedAttack");
            }

            float move = 0;
            if (Input.GetKey(left))
            {
                move = -1;
            }
            else if (Input.GetKey(right))
            {
                move = 1;
            }
            bool isJumping = Input.GetKeyDown(jump);
            bool isCrouching = Input.GetKey(crouch);
            Move(move, isCrouching, isJumping);
        }
        
    }

    public void Move (float d, bool isCrouching, bool isJumping)
    {
        animator.SetBool("Crouch", isCrouching);
        d = (isCrouching ? d * crouchSpeedMultiplier : d);
        animator.SetFloat("Speed", Mathf.Abs(d));

        rigidBody.velocity = new Vector2(d * speed, rigidBody.velocity.y);

        // If the input is moving the player right and the player is facing left...
        if (d > 0 && !faceRight)
        {
            // ... flip the player.
            FlipPlayer();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (d < 0 && faceRight)
        {
            // ... flip the player.
            FlipPlayer();
        }
        if (isGrounded && isJumping)
        {
            // Add a vertical force to the player.
            isGrounded = false;
            animator.SetBool("Ground", false);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }
    }

    public void FlipPlayer()
    {
        faceRight = !faceRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

	public void TakeDamage(float damage) {
		hp -= damage;
		Debug.Log("damage Taken !");
	}
}   
