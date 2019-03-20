using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedVars : MonoBehaviour
{
    // Un nombre qui indique le numero de la scene entre 0~5
    private static int sceneBackgroundIdx = 0;
    private static int avatarIdxP1 = 0;
    private static int avatarIdxP2 = 0;

    public void SetSceneBackgroundIdx(int idx)
    {
        sceneBackgroundIdx = idx;
    }

    public int GetSceneBackgroundIdx()
    {
        return sceneBackgroundIdx;
    }

    public void SetAvatarIdxP1(int idx)
    {
        avatarIdxP1 = idx;
    }

    public void SetAvatarIdxP2(int idx)
    {
        avatarIdxP2 = idx;
    }

    public int GetAvatarIdxP1()
    {
        return avatarIdxP1;
    }

    public int GetAvatarIdxP2()
    {
        return avatarIdxP2;
    }
}