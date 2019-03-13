using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	public Slider healthSlider;
	public float currentHealth;
	public Image damageImage;
	public float flashSpeed = 5.0f;
	public Color flashColor = new Color(1.0f, 0.0f, 0.0f, 0.1f);

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

		if(playerInfo.numberOfThisPlayer == 2)
			PlaceNamePlayer2();
	}

	// Update is called once per frame
	void Update() {
		if (playerInfo.isDamaged) {
			damageImage.color = flashColor;
		} else {
			damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		}

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
			yield return new WaitForSeconds(0.5f);
			sliderFlashImage.color = sliderFlashColorEnd;
		}

		if (sliderFlashImage.color == sliderFlashColorEnd) {
			yield return new WaitForSeconds(0.5f);
			sliderFlashImage.color = sliderFlashColorStart;
		}
	}

	/**
	 * Change the position of the player name 2
	 */
	private void PlaceNamePlayer2() {
		// Get the recTransform of HealthSliderP2.PlayerName
		RectTransform transform = GameObject.Find("HealthSliderP2").GetComponent<Slider>().transform.Find("PlayerName").GetComponent<RectTransform>();
		// Get the text of HealthSliderP2.PlayerName
		Text name = GameObject.Find("HealthSliderP2").GetComponentInChildren<Text>();
		// Update position
		transform.anchoredPosition = new Vector3(- (name.preferredWidth), 0, 0);
	}
}
