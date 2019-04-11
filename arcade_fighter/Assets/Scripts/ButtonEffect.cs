using UnityEngine;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour {
	public void OnButtonHoverEnter(GameObject btn) {
		RectTransform transform = btn.GetComponent<RectTransform>();
		transform.localScale = transform.localScale * 0.95f;
		btn.GetComponentInChildren<Image>().color = new Color(0.3018868f, 0.301886800f, 0.3018868f, 0.8627451f);
	}

	public void OnButtonHoverExit(GameObject btn) {
		btn.GetComponent<RectTransform>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
		btn.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}
}
