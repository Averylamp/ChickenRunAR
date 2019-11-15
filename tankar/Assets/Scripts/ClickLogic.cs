using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

// Example of click on the canvas:
// https://answers.unity.com/questions/1526663/detect-click-on-canvas.html

public class ClickLogic : MonoBehaviour
{

  private ARRaycastManager ar_origin;

  public double CATCH_DISTANCE = 2.5;
  private const float CHICKEN_RESPAWN_RANGE = 3.3f;


  public bool mouse_button_down = false;
  public bool finger_touch_down = false;


  // timer logic
  static float timer = 60.0f; // one minute timer

  public float last_second = timer;
  public Text timer_text;

  // num chickens caught
  static int num_chickens_caught = 0;

  // Enum for all of the pages.
  public enum PagesEnum
  {
    [Description("SetupPage")]
    SetupPage,
    [Description("GamePage")]
    GamePage,
    [Description("LeaderboardPage")]
    LeaderboardPage,
    [Description("SettingsPage")]
    SettingsPage,
    [Description("LandingPage")]
    LandingPage,
    [Description("InteractiveTutorialPage")]
    InteractiveTutorialPage,
    [Description("InstructionsPage")]
    InstructionsPage
  }

  // Keep track of the active page.
  PagesEnum activePage;

  // Hash table for GameObject pages.
  public Dictionary<string, GameObject> pageMap;

  public GameObject setup_ui;
  public GameObject gameplay_ui;

  // Chicken that should be set in the Unity UI.
  public GameObject chicken;

  // Returns the GameObjct associated with the enum value.
  // Ex. call: GetGameObjectFromEnum(PagesEnum.SetupPage);
  public GameObject GetGameObjectFromEnum(PagesEnum pageEnum)
  {
    Debug.Log("Calling GetGameObjectFromEnum");
    Debug.Log(pageEnum.ToString());
    return (GameObject) pageMap[pageEnum.ToString()];
  }

  void RemoveAndReplaceChicken()
  {
    Destroy(chicken, 0.3f);
    Vector3 humanPosition = Vector3.zero;
    GameObject terrain = GameObject.Find("Terrain");
    if (Application.platform == RuntimePlatform.IPhonePlayer)
    {
      humanPosition = GameObject.Find("AR Camera").transform.position;
    }
    else
    {
      humanPosition = GameObject.Find("Human Cube").transform.position;
    }
    Vector3 newChickenPosition = Vector3.zero;
    do
    {
      newChickenPosition = terrain.transform.position 
      + (UnityEngine.Random.Range(-CHICKEN_RESPAWN_RANGE, CHICKEN_RESPAWN_RANGE)) * terrain.transform.right 
      + (UnityEngine.Random.Range(-CHICKEN_RESPAWN_RANGE, CHICKEN_RESPAWN_RANGE)) * terrain.transform.forward; 
    } while (Vector3.Distance(newChickenPosition, humanPosition) < CHICKEN_RESPAWN_RANGE);


    chicken = Instantiate(chicken, newChickenPosition, Quaternion.identity);
    chicken.transform.parent = terrain.transform;
    (chicken.GetComponent("ChickenCharacter") as ChickenCharacter).chickenSpeed += 0.3f;
  }

  bool UpdateChickenDistance()
  {
    float chickenDistance = 100; // Default is not close enough
    if (chicken != null)
    {
      if (Application.platform == RuntimePlatform.IPhonePlayer)
      {
        GameObject mainCamera = GameObject.Find("AR Camera");
        chickenDistance = Vector3.Distance(chicken.transform.position, mainCamera.transform.position);
      }
      else
      {
        GameObject humanCube = GameObject.Find("Human Cube");
        chickenDistance = Vector3.Distance(chicken.transform.position, humanCube.transform.position);
      }
    }
    else
    {
      print("Can't find chicken");
    }


    try
    {
      Image catchButton = GameObject.Find("CatchChickenButton").GetComponent<Image>();
      if (chickenDistance < CATCH_DISTANCE)
      {
        catchButton.color = Color.green;
      }
      else
      {
        catchButton.color = Color.red;
      }

    }
    catch
    {
      // no-op
    }
    return chickenDistance < CATCH_DISTANCE;
  }

  // Start is called before the first frame update
  void Start()
  {
    Debug.Log("Starting ClickLogic.");
    ar_origin = FindObjectOfType<ARRaycastManager>();

    // Initial the (UI) page map.
    pageMap = new Dictionary<string, GameObject>();
    var pagesEnumValues = Enum.GetValues(typeof(PagesEnum));
    Debug.Log("Setting up pageMap.");
    foreach (PagesEnum pageEnum in pagesEnumValues)
    {
      // Add the GameObjects that correspond to the page name
      // to the pageMap.
      string pageName = pageEnum.ToString();
      Debug.Assert(pageMap.ContainsKey(pageName) == false);
      pageMap.Add(pageName, GameObject.Find(pageName));
      Debug.Assert(pageMap.ContainsKey(pageName) == true);
    }

    // Switch to the Landing Page, where the game should start.
    SwitchCanvas(PagesEnum.LandingPage);
  }

  // Reset a canvas based on enum.
  void ResetCanvas(PagesEnum pageEnum)
  {
    GetGameObjectFromEnum(pageEnum).SetActive(false);
  }

  // This will reset and hide all canvases.
  void ResetAllCanvases()
  {
    // Deactivate all of the pages.
    foreach (PagesEnum pageEnum in Enum.GetValues(typeof(PagesEnum)))
    {
      ResetCanvas(pageEnum);
    }
  }

  void ActivateCanvas(PagesEnum pageEnum)
  {
    GetGameObjectFromEnum(pageEnum).SetActive(true);
  }

  // Switch to the new canvas and set the active page.
  void SwitchCanvas(PagesEnum pageEnum)
  {
    // TODO: Reset all the data.
    // ResetAllData();

    // Reset all screens.
    ResetAllCanvases();

    // Turn on the screen we care about.
    ActivateCanvas(pageEnum);
    activePage = pageEnum;
  }

  // Call this at every screen change to reset all the game data.
  void ResetAllData()
  {
    // Reset the timer.
    timer = 60.0f; // one minute timer
    last_second = timer;
    timer_text.text = timer.ToString("0:00");

    // TODO: Reset the chicken count.
    num_chickens_caught = 0;
    GameObject.Find("ChickenCount").GetComponent<UnityEngine.UI.Text>().text = num_chickens_caught.ToString();

    // TODO: handle the placement persisting
    }

  void ResetGame()
  {
    setup_ui.SetActive(true);
    gameplay_ui.SetActive(false);

    // reset the game
    timer = 60.0f; // one minute timer
    last_second = timer;
    timer_text.text = timer.ToString("0:00");

  }

  void StartGameplay()
  {
    setup_ui.SetActive(false);
    gameplay_ui.SetActive(true);

    // reset the game
    timer = 60.0f; // one minute timer
    last_second = timer;
  }

  void UpdateGameplay()
  {
    timer -= Time.deltaTime;
    // only update the timer every second
    if (last_second - timer > 1.0f)
    {
      timer_text.text = timer.ToString("0:00");
      last_second -= 1.0f;
    }

    if (timer < 0)
    {
      gameplay_ui.SetActive(false);
      // TODO(averylamp): Activate end game
    }

  }



  // Update is called once per frame
  void Update()
  {
    GameObject LastClickedObject = CheckForGameplayObjectClick();

    bool CanCatchChicken = UpdateChickenDistance();
    if (LastClickedObject != null)
    {
      HandleGameplayObjectClick(LastClickedObject, CanCatchChicken);
    }

    mouse_button_down = false;
    finger_touch_down = false;
        
    // if the game is active
    if (gameplay_ui.activeSelf)
    {
      UpdateGameplay();
    }

  }

  GameObject CheckForGameplayObjectClick()
  {
    // If a mouse click is detected
    if (Input.GetMouseButtonDown(0))
    {
      // Debug.Log("Button down.");
      mouse_button_down = true;
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
      finger_touch_down = true;
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

  void HandleGameplayObjectClick(GameObject LastClickedObject, bool canCatchChicken)
  {
    // TODO: handle canCatchChicken better

    Debug.Log(LastClickedObject.name);
    // Use different logic depending on the page.
    switch(activePage)
    {
      case PagesEnum.LandingPage:
      {
        if (LastClickedObject.name == "SinglePlayerButton")
        {
          SwitchCanvas(PagesEnum.SetupPage);
        }
        else if (LastClickedObject.name == "MultiPlayerButton")
        {

        }
        else if (LastClickedObject.name == "LeaderboardButton")
        {

        }
        else if (LastClickedObject.name == "SettingsButton")
        {
          // TODO(Moin): add settings popup menu.
        }
        break;
      }
      case PagesEnum.SetupPage:
      {
        if (LastClickedObject.name == "StartButton")
        {
          SwitchCanvas(PagesEnum.GamePage);
        }
        else if (LastClickedObject.name == "CloseButton")
        {
          SwitchCanvas(PagesEnum.LandingPage);
        }
        break;
      }
      case PagesEnum.GamePage:
      {
        if (LastClickedObject.name == "CloseButton")
        {
          SwitchCanvas(PagesEnum.SetupPage);
        }
        else if (LastClickedObject.name == "CatchChickenButton" && canCatchChicken)
        {
          num_chickens_caught += 1;
          GameObject.Find("ChickenCount").GetComponent<UnityEngine.UI.Text>().text = num_chickens_caught.ToString();
          RemoveAndReplaceChicken();
        }
        break;
      }
      default: break;
    }
    LastClickedObject = null;

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
