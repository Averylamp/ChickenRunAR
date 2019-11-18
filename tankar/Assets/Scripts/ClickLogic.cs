using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;



// Example of click on the canvas:
// https://answers.unity.com/questions/1526663/detect-click-on-canvas.html

public class ClickLogic : MonoBehaviour
{

  GameLogicController gameLogicController;
  UILogicController uiLogicController;
  public bool mouseButtonDown = false;
  public bool fingerTouchDown = false;
  AudioSource audioFX;

  // Start is called before the first frame update
  void Start()
  {
    Debug.Log("Starting ClickLogic.");
    audioFX = GameObject.Find("CatchChickenButton").GetComponent<AudioSource>();
    gameLogicController = GetComponent<GameLogicController>();
    uiLogicController = GetComponent<UILogicController>();
    AudioSource audioMusic = GetComponent<AudioSource>();
    audioMusic.Play(0);
  }

  // Update is called once per frame
  void Update()
  {
    GameObject lastClickedObject = CheckForGameplayObjectClick();

    if (lastClickedObject != null)
    {
      HandleGameplayObjectClick(lastClickedObject);
    }

    mouseButtonDown = false;
    fingerTouchDown = false;

  }

  GameObject CheckForGameplayObjectClick()
  {
    // If a mouse click is detected
    if (Input.GetMouseButtonDown(0))
    {
      // Debug.Log("Button down.");
      mouseButtonDown = true;
      PointerEventData pointerData = new PointerEventData(EventSystem.current);
      pointerData.position = Input.mousePosition;
      List<RaycastResult> results = new List<RaycastResult>();
      EventSystem.current.RaycastAll(pointerData, results);
      foreach (RaycastResult result in results)
      {
        return result.gameObject;
      }
    }

    // TODO(ethan): confirm that these two if/else components are mutually exclusive
    // If a finger click is detected
    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    {
      fingerTouchDown = true;
      Touch touch = Input.GetTouch(0);
      PointerEventData pointerData = new PointerEventData(EventSystem.current);
      pointerData.position = touch.position;
      List<RaycastResult> results = new List<RaycastResult>();
      EventSystem.current.RaycastAll(pointerData, results);
      foreach (RaycastResult result in results)
      {
        return result.gameObject;
      }
    }
    return null;
  }

  void HandleGameplayObjectClick(GameObject lastClickedObject)
  {
    // TODO: handle canCatchChicken better

    Debug.Log(lastClickedObject.name);
    // Use different logic depending on the page.
    switch (uiLogicController.activePage)
    {
      case UILogicController.PagesEnum.LandingPage:
        {
          if (lastClickedObject.name == "SinglePlayerButton")
          {
            uiLogicController.SwitchCanvas(UILogicController.PagesEnum.SetupPage);
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
              GameObject.Find("AR Session").GetComponent<ARKitCoachingOverlay>().ActivateCoaching(true);
            }
          }
          else if (lastClickedObject.name == "MultiPlayerButton")
          {

          }
          else if (lastClickedObject.name == "LeaderboardButton")
          {
            uiLogicController.SwitchCanvas(UILogicController.PagesEnum.LeaderboardPage);
          }
          else if (lastClickedObject.name == "SettingsButton")
          {
            uiLogicController.SwitchCanvas(UILogicController.PagesEnum.SettingsPage);
          }
          break;
        }
      case UILogicController.PagesEnum.SetupPage:
        {
          if (lastClickedObject.name == "StartButton")
          {
            uiLogicController.SwitchCanvas(UILogicController.PagesEnum.GamePage);
          }
          else if (lastClickedObject.name == "CloseButton")
          {
            uiLogicController.SwitchCanvas(UILogicController.PagesEnum.LandingPage);
          }
          break;
        }
      case UILogicController.PagesEnum.GamePage:
        {
          bool canCatchChicken = gameLogicController.GetChickenDistance() < GameLogicController.CATCH_DISTANCE;
          if (lastClickedObject.name == "CloseButton")
          {
            uiLogicController.SwitchCanvas(UILogicController.PagesEnum.SetupPage);
          }
          else if (lastClickedObject.name == "CatchChickenButton" && canCatchChicken)
          {
            UILogicController.numChickensCaught += 1;
            lastClickedObject.GetComponent<AudioSource>().Play();
            GameObject.Find("ChickenCount").GetComponent<UnityEngine.UI.Text>().text = UILogicController.numChickensCaught.ToString();
            gameLogicController.RemoveAndReplaceChicken();
          }
          break;
        }
      case UILogicController.PagesEnum.SettingsPage:
        {
          if (lastClickedObject.name == "SettingsCloseButton")
          {
            string name = GameObject.Find("SettingsPageName").GetComponent<InputField>().text;
            PlayerPrefs.SetString("name", name);
            PlayerPrefs.Save();
            Debug.Log(PlayerPrefs.GetString("name"));
            uiLogicController.SwitchCanvas(UILogicController.PagesEnum.LandingPage);
          }
          else if (lastClickedObject.name == "SettingsNameInputField")
          {
            
          }
          else if (lastClickedObject.name == "SettingsPageMusicButton")
          {
            AudioSource audioMusic = GetComponent<AudioSource>();

            Text buttonText = lastClickedObject.GetComponentInChildren<Text>();

            if (audioMusic.isPlaying)
            {
                buttonText.text = "MUSIC ON";
                audioMusic.Stop();
            } else
            {
                buttonText.text = "MUSIC OFF";
                audioMusic.Play();
            }
          }
          else if (lastClickedObject.name == "SettingsPageSoundButton")
          {
            Text buttonText = lastClickedObject.GetComponentInChildren<Text>();
            audioFX.mute = !audioFX.mute;

            if (audioFX.mute)
            {
                buttonText.text = "SOUND EFFECTS OFF";
            } else
            {
                buttonText.text = "SOUND EFFECTS ON";
            }
          }
          break;
        }
      case UILogicController.PagesEnum.LeaderboardPage:
        {
          if (lastClickedObject.name == "LeaderboardCloseButton")
          {
            uiLogicController.SwitchCanvas(UILogicController.PagesEnum.LandingPage);
          }
          break;
        }
      default: break;
    }
    lastClickedObject = null;

  }

  public static bool OnUI()
  {
    Touch touch = Input.GetTouch(0);
    PointerEventData pointerData = new PointerEventData(EventSystem.current);
    pointerData.position = touch.position;
    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerData, results);
    foreach (RaycastResult result in results)
    {
      return true;
    }
    return false;
  }
}
