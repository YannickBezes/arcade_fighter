using UnityEngine;

public class ButtonEffect : MonoBehaviour {
	public void OnButtonHoverEnter(GameObject btn) {
		RectTransform transform = btn.GetComponent<RectTransform>();
		transform.localScale = transform.localScale * 0.95f;
	}

	public void OnButtonHoverExit(GameObject btn) {
		btn.GetComponent<RectTransform>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
	}
}
