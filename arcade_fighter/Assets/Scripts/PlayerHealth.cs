using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	public Slider healthSlider;
	public float currentHealth;

	private Color sliderFlashColorStart;
	private Color sliderFlashColorEnd;
	private Player playerInfo;
	private Image sliderFlashImage;

	// Start is called before the first frame update
	void Start() {
		playerInfo = GetComponent<Player>();
		sliderFlashImage = healthSlider.GetComponentInChildren<Image>();
		sliderFlashColorStart = sliderFlashImage.color;
		sliderFlashColorEnd = sliderFlashImage.color;
		currentHealth = playerInfo.hp;
		healthSlider.GetComponentInChildren<Text>().text = playerInfo.playerName;
		healthSlider.minValue = 0;
		healthSlider.maxValue = playerInfo.maxHp;
	}

	// Update is called once per frame
	void Update() {
		healthSlider.value = playerInfo.hp;

		float percentage = playerInfo.hp / playerInfo.maxHp;
		if (percentage < 0.2f)
			sliderFlashColorEnd = new Color(1.0f, 0.0f, 0.0f, 0.8f);
		else if (percentage < 0.5f)
			sliderFlashColorEnd = new Color(0.8f, 0.3f, 0.2f, 0.8f);
		else if (percentage < 0.8f)
			sliderFlashColorEnd = new Color(1.0f, 1.0f, 0.0f, 0.8f);

		StartCoroutine(HealthBarFlash());
	}

	IEnumerator HealthBarFlash() {
		if (sliderFlashImage.color == sliderFlashColorStart) {
			yield return new WaitForSeconds(0.05f);
			sliderFlashImage.color = sliderFlashColorEnd;
		}

		if (sliderFlashImage.color == sliderFlashColorEnd) {
			yield return new WaitForSeconds(0.05f);
			sliderFlashImage.color = sliderFlashColorStart;
		}
	}
}
