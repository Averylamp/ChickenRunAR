using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
  public static SoundController instance = null;
  public AudioSource chickenCatchFX;
  public AudioSource audioMusic;

  private static string PREF_MUSIC_KEY = "PREF_MUSIC_KEY";
  private static string PREF_FXS_KEY = "PREF_FXS_KEY";
  // Start is called before the first frame update
  void Start()
  {
    if (PlayerPrefs.GetInt(PREF_MUSIC_KEY, 1) == 1)
    {
      audioMusic.Play();
    }

  }

  void Awake()
  {
    //Check if instance already exists
    if (instance == null)

      //if not, set instance to this
      instance = this;

    //If instance already exists and it's not this:
    else if (instance != this)

      //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
      Destroy(gameObject);

    //Sets this to not be destroyed when reloading scene
    DontDestroyOnLoad(gameObject);
  }

  // Update is called once per frame
  void Update()
  {

  }

  public bool GetMusicPreference()
  {
    return PlayerPrefs.GetInt(PREF_MUSIC_KEY, 1) == 1;
  }

  public bool GetFXSPreference()
  {
    return PlayerPrefs.GetInt(PREF_FXS_KEY, 1) == 1;
  }

  public void SetMusicPreference(bool musicOn)
  {
    PlayerPrefs.SetInt(PREF_MUSIC_KEY, musicOn ? 1 : 0);
    if (musicOn)
    {
      audioMusic.Play();
    }
    else
    {
      audioMusic.Stop();
    }
  }

  public void SetChickenCatchFXPreference(bool fxsOn)
  {
    PlayerPrefs.SetInt(PREF_FXS_KEY, fxsOn ? 1 : 0);
    if (fxsOn)
    {
      chickenCatchFX.Play();
    }
    else
    {
      chickenCatchFX.Stop();
    }
  }
  public void PlayChickenCaughtFX()
  {
    if (PlayerPrefs.GetInt(PREF_FXS_KEY, 1) == 1)
    {
      chickenCatchFX.Play();
    }

  }

}
