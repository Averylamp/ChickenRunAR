using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

  public static AudioSource chickenCatchFX;
  public static AudioSource audioMusic;

  // Start is called before the first frame update
  void Start()
  {
    audioMusic.Play();
  }

  // Update is called once per frame
  void Update()
  {

  }

  public static void SetMusicPreference(bool musicOn)
  {

  }
  public static void PlayChickenCaughtFX()
  {
    chickenCatchFX.Play();
  }

}
