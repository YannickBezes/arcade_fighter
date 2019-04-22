using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	public void OnButtonHoverEnter(GameObject btn) {
		RectTransform transform = btn.GetComponent<RectTransform>();
		transform.localScale = transform.localScale * 1.1f;
	}

	public void OnButtonHoverExit(GameObject btn) {
		btn.GetComponent<RectTransform>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}


	public void Jeu1Click() {
		System.Diagnostics.Process.Start("C:/Users/Arcade/Desktop/Games/arcade_fighter/TER_Jeu_d_arcade.exe");
	}

	public void Jeu2Click() {
		System.Diagnostics.Process.Start("C:/Users/Arcade/Desktop/Games/super_mario_student/Super_Mario_Student.exe");
	}

	public void Jeu3Click() {
		System.Diagnostics.Process.Start("C:/Users/Arcade/Desktop/Games/Gra/Gra.exe");
	}

	public void QuitClick() {
		Application.Quit();
	}
}
