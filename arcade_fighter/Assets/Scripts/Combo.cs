using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Combo {
	public List<listKey> listKeyCode;
	public float damage;

}

[System.Serializable]
public class ListCombos {
	public List<Combo> listCombos;
}

public enum listKey {
	meleAttack,
	rangeAttack,
	block,
	button3,
	button4
}