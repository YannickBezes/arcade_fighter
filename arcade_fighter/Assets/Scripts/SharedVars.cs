using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedVars : MonoBehaviour
{
    // Un nombre qui indique le numero de la scene entre 0~5
    private static int sceneBackgroundIdx = 0;    

    public void SetSceneBackgroundIdx(int idx)
    {
        sceneBackgroundIdx = idx;
    }

    public int GetSceneBackgroundIdx()
    {
        return sceneBackgroundIdx;
    }

}
