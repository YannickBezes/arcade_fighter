using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorScript : MonoBehaviour
{
    public float gravity;

    private GameObject[] charactersList;
    private GameObject player1, player2;
    public int player1Selection, player2Selection;


    // Start is called before the first frame update
    void Start()
    {
        Transform t = GameObject.Find("CharactersList").transform;

        charactersList = new GameObject[t.childCount];
        for (int i = 0; i < t.childCount; i++)
            charactersList[i] = t.GetChild(i).gameObject;

        foreach (GameObject go in charactersList)
            go.SetActive(false);

        player1 = charactersList[player1Selection];
        player2 = charactersList[player2Selection];

        foreach (GameObject p in new GameObject[] {player1, player2})
        {
            p.GetComponent<Rigidbody2D>().gravityScale = this.gravity;
            p.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
