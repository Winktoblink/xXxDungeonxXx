using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioSource efxSource;
    public AudioSource musicSource;
    public static SoundManager INSTANCE = null;

    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

	// Use this for initialization
	void Awake () {
	    if(INSTANCE == null) {
            INSTANCE = this;
        } else if(INSTANCE != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
	}

    //Play a single audio clip.
    public void playSingle(AudioClip clip) {
        efxSource.clip = clip;
        efxSource.Play();
    }

    //Take any number of clips and randomize the sound and pitch played for variety.
    public void randomizeSfx(params AudioClip[] clips) {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        efxSource.pitch = randomPitch;
        efxSource.clip = clips[randomIndex];
        efxSource.Play();
    }
}
