using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private Animator anim;

    public float bombDamage;
    public float bombExplosionRadius;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //StartCoroutine(Die());
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
        {
            GameObject player1 = GameObject.Find("Decor").GetComponent<DecorScript>().GetPlayer1();

            GameObject player2 = GameObject.Find("Decor").GetComponent<DecorScript>().GetPlayer2();

            CircleCollider2D collider = GetComponent<CircleCollider2D>();

            if (collider.Distance(player1.GetComponent<BoxCollider2D>()).distance < bombExplosionRadius)
            {
                player1.GetComponent<Player>().TakeDamage(bombDamage);
            }

            if (collider.Distance(player2.GetComponent<BoxCollider2D>()).distance < bombExplosionRadius)
            {
                player2.GetComponent<Player>().TakeDamage(bombDamage);
            }

            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (!(collision.gameObject.tag == "Player") && !(collision.gameObject.tag == "Ground"))
            Destroy(this.gameObject);
    }

}
