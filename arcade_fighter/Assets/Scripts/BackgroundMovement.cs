using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float speed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 11f)
        {
            speed *= -1.0f;
        }

        if (transform.position.x <= -11f)
        {
            speed *= -1.0f;
        }

        transform.Translate(speed * Time.deltaTime, 0, 0);
    }
}
