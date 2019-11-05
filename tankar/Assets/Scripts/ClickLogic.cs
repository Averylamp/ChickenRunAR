using System.Collections;
using System.Collections.Generic;
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

  // modal for setup
  public GameObject gameplay_ui;
  public GameObject setup_ui;

  public GameObject chicken;

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
      Image catchButton = GameObject.Find("Button_CatchChicken").GetComponent<Image>();
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

    // set setup to active but gameplay to not
    setup_ui.SetActive(true);
    gameplay_ui.SetActive(false);
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
    // print current game object if it exists

    Debug.Log(LastClickedObject.name);
    // Make a call to the game object if HandleClick() is defined for the button.
    // Then continue.
    // TODO(ethan): finish a call like this.
    if (LastClickedObject.name == "Button_CatchChicken" && canCatchChicken)
    {

      num_chickens_caught += 1;
      GameObject.Find("TitleText").GetComponent<UnityEngine.UI.Text>().text = num_chickens_caught.ToString();
      RemoveAndReplaceChicken();
    }
    else if (LastClickedObject.name == "Button_StartGame_Text")
    {
      StartGameplay();
    }
    else if (LastClickedObject.name == "Button_Home")
    {
      ResetGame();
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
