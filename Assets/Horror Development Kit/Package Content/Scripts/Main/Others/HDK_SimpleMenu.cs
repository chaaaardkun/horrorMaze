//Script written by Giovanni Cartella - giovanni.cartella@hotmail.com || www.giovannicartella.weebly.com
//You are allowed to use this only if you have "Horror Development Kit" license, so only if you bought it officially

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HDK_SimpleMenu : MonoBehaviour {

    public HDK_UIFade BlackGui;
    public AudioSource Music;
    bool fadeOutMusic;
    bool fadeInMusic;
    public float LoadDelay = 2f;

    void Start()
    {
        fadeInMusic = true;
        fadeOutMusic = !fadeInMusic;
    }

    void Update()
    {
        fadeOutMusic = !fadeInMusic;

        if (fadeOutMusic)
        {
            Music.volume -= Time.deltaTime;
        }else if (fadeInMusic)
        {
            Music.volume += Time.deltaTime;
        }
    }

    public void Exit()
    {
		Application.Quit ();
	}

    public void Play(string levelToPlay)
    {
        fadeInMusic = false;
        StartCoroutine(PlayDemo(levelToPlay));
    }

	public IEnumerator PlayDemo(string level)
    {
        BlackGui.TextOut = false;
        BlackGui.TextIn = true;
        yield return new WaitForSeconds(LoadDelay);
        SceneManager.LoadScene (level, LoadSceneMode.Single);
	}
}
