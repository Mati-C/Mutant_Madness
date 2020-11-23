using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent (typeof(AudioSource))]
public class AllahuAkbar : MonoBehaviour {

    public MovieTexture movie;
    AudioSource audioPlayer;

	void Awake () {
        var allAudios = FindObjectsOfType<AudioSource>();
        foreach (var i in allAudios)
            i.Pause();
        Time.timeScale = 0;
        GetComponent<RawImage>().texture = movie as MovieTexture;
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = movie.audioClip;
        movie.Play();
        audioPlayer.Play();
	}
	
	void LateUpdate () {
		if (!movie.isPlaying)
            SceneManager.LoadScene(2);
	}
}
