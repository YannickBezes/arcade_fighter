using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatBehavior : MonoBehaviour
{
    private Animator animator;
    private bool isBusy;
    private bool isBombReady;

    public float speed;
    public GameObject bombPrefab;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isBombReady = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 60% => walk
        // 10% => sleep
        // 30% => idle
        float f = Random.Range(0.0f, 1.0f);
        //Debug.Log(f);

        //float f = 0.2f;
        if (!isBusy)
        {

            if (f > 0.9f)
            {

                StartCoroutine(Sleep(2));

            }
            else
            {

                if (f > 0.6f)
                {

                    StartCoroutine(Idle(1));

                }
                else
                {

                    StartCoroutine(Walk(2));

                }
            }
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(speed * Time.fixedDeltaTime, 0, 0);
    }

    private IEnumerator Walk(int seconds)
    {
        isBusy = true;
        speed = Random.Range(-2.0f, 2.0f);
        if (speed < 0)
            {
                transform.localScale = new Vector3(-2, 2, 2);
            }
        else
            {
                transform.localScale = new Vector3(2, 2, 2);
            }

        animator.SetTrigger("Walk");
        yield return new WaitForSeconds(seconds);

        if (Random.Range(0.0f, 1.0f) > 0.0f) {
            GameObject bomb = (GameObject)Instantiate(bombPrefab, transform.position, Quaternion.identity);
            bomb.GetComponent<Rigidbody2D>().velocity = new Vector3(Mathf.Clamp(speed * 3, Mathf.Sign(speed) * 1.0f, Mathf.Sign(speed) * 5.0f), Mathf.Clamp(Mathf.Abs(speed) * 2, 1.0f, 4.0f), 0);
        }

        isBusy = false;
    }

    private IEnumerator Sleep(int seconds)
    {
        isBusy = true;
        speed = 0;
        animator.SetTrigger("Sleep");
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }


    private IEnumerator Idle(int seconds)
    {
        isBusy = true;
        speed = 0;
        animator.SetTrigger("Idle");
        yield return new WaitForSeconds(seconds);
        isBusy = false;
    }

}
