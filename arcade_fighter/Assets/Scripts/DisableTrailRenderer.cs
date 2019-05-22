using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTrailRenderer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (TrailRenderer tr in FindObjectsOfType<TrailRenderer>())
        {
            tr.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
