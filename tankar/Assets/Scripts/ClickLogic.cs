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


  // Start is called before the first frame update
  void Start()
  {
    Debug.Log("Starting ClickLogic.");

    gameLogicController = GetComponent<GameLogicController>();
    uiLogicController = GetComponent<UILogicController>();
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
          uiLogicController.LandingLageClick(lastClickedObject);
          break;
        }
      case UILogicController.PagesEnum.SetupPage:
        {
          uiLogicController.SetupPageClick(lastClickedObject);
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
            SoundController.instance.PlayChickenCaughtFX();
            GameObject.Find("ChickenCount").GetComponent<UnityEngine.UI.Text>().text = UILogicController.numChickensCaught.ToString();
            gameLogicController.RemoveAndReplaceChicken();
          }
          break;
        }
      case UILogicController.PagesEnum.SettingsPage:
        {
          uiLogicController.SettingsPageClick(lastClickedObject);
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
