using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleCountdown : MonoBehaviour {
	public Text countdownText;
	public Text gameTimeText;
    public AudioSource countDownAudio;

	private int gameTime = 0;
	private int countdownTime = 3;

	private string countdownStr;

	public bool isGameReady = false;

	// Start is called before the first frame update
	void Start() {
		countdownStr = countdownTime.ToString();
		StartCoroutine("GTime");
		Time.timeScale = 1;
	}

	// Update is called once per frame
	void Update() {
		//countdownText.text = countdownStr.ToString();
		gameTimeText.text = "Time:" + gameTime.ToString();
	}

	IEnumerator GTime() {
		StartCoroutine("Countdown");
        countDownAudio.Play();
		yield return new WaitForSeconds(3);
        countDownAudio.Stop();
        yield return new WaitForSeconds(1);
		StopCoroutine("Countdown");
		StartCoroutine("GameTime");
		countdownText.text = "";
		isGameReady = true;
	}

	IEnumerator Countdown() {
        while (true)
        {
            yield return new WaitForSeconds(1);
            --countdownTime;
            if (countdownTime < 1)
            {
                countdownText.text = " Fight!";
                countdownText.fontSize = 96;
                countdownText.color = new Color32(0xC6, 0x28, 0x28, 0xFF);
            }
            else
            {
                countdownText.text = countdownTime.ToString();
            }
        }
	}

	IEnumerator GameTime() {
		while (true) {
            //yield return new WaitForFixedUpdate();
            ++gameTime;
            yield return new WaitForSeconds(1);
		}
	}

	public void StopGameTime() {
		StopCoroutine("GameTime");
		isGameReady = false;
	}
}