using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIExtensions;

public class SceneFade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UITransitionEffect>().Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
