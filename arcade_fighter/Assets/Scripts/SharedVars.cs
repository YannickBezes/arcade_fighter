using UnityEngine;

public class SharedVars : MonoBehaviour {
	// Un nombre qui indique le numero de la scene entre 0~5
	private static int sceneBackgroundIdx = 0;
	private static int avatarIdxP1 = 0;
	private static int avatarIdxP2 = 0;

	[Range(0.0f, 1.0f)]
	private static float musicVolume = 1.0f;

	[Range(0.0f, 1.0f)]
	private static float effectsVolume = 1.0f;

	public void SetMusicVolume(float val) {
		musicVolume = val;
	}

	public void SetEffectsVolume(float val) {
		effectsVolume = val;
	}

	public float GetMusicVolume() {
		return musicVolume;
	}

	public float GetEffectsVolme() {
		return effectsVolume;
	}

	public void SetSceneBackgroundIdx(int idx) {
		sceneBackgroundIdx = idx;
	}

	public int GetSceneBackgroundIdx() {
		return sceneBackgroundIdx;
	}

	public void SetAvatarIdxP1(int idx) {
		avatarIdxP1 = idx;
	}

	public void SetAvatarIdxP2(int idx) {
		avatarIdxP2 = idx;
	}

	public int GetAvatarIdxP1() {
		return avatarIdxP1;
	}

	public int GetAvatarIdxP2() {
		return avatarIdxP2;
	}
}