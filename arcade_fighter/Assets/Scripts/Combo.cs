using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Combo {
	public List<KeyCode> listKeyCode;
	public float damage;
}

[System.Serializable]
public class ListCombos {
	public List<Combo> listCombos;
}