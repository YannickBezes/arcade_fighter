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

    public KeyCode left;
    public KeyCode right;
    public KeyCode jump;
    public KeyCode crouch;

    public Transform groundCheckPoint;
    public bool isGrounded;
    public float groundCheckRadius;
    public LayerMask whatIsGround;

    private Animator animator;
    private Rigidbody2D rigidBody;
    private bool faceRight = true;

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
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                isGrounded = true;
        }
        animator.SetBool("Ground", isGrounded);
        animator.SetFloat("vSpeed", rigidBody.velocity.y);

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
        this.Move(move, isCrouching, isJumping);
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
}   
