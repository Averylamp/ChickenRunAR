using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
public class UILogicController : MonoBehaviour
{

  // timer logic
  static float timer = 60.0f; // one minute timer

  public static float GAME_TIME = 60.0f;
  public float lastSecond = timer;
  public Text timerText;
  // num chickens caught
  static public int numChickensCaught = 0;
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
  public PagesEnum activePage;

  // Hash table for GameObject pages.
  public Dictionary<string, GameObject> pageMap;

  public GameObject setupUI;
  public GameObject gameplayUI;


  // Returns the GameObjct associated with the enum value.
  // Ex. call: GetGameObjectFromEnum(PagesEnum.SetupPage);
  public GameObject GetGameObjectFromEnum(PagesEnum pageEnum)
  {
    return (GameObject)pageMap[pageEnum.ToString()];
  }


  // Start is called before the first frame update
  void Start()
  {
    // Initial the (UI) page map.
    pageMap = new Dictionary<string, GameObject>();
    var pagesEnumValues = System.Enum.GetValues(typeof(PagesEnum));
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
    Debug.Log("Finding Timer Text");
    Debug.Log(GameObject.Find("TimerText"));
    // Debug.Log(GameObject.Find("TimerText").GetComponent<Text>());
    // timerText = GameObject.Find("TimerText").GetComponent<Text>();
  }

  // Call this at every screen change to reset all the game data.
  void ResetAllData()
  {
    // Reset the timer.
    timer = GAME_TIME; // one minute timer
    lastSecond = timer;
    timerText.text = timer.ToString("0:00");

    // TODO: Reset the chicken count.
    numChickensCaught = 0;
    GameObject.Find("ChickenCount").GetComponent<UnityEngine.UI.Text>().text = numChickensCaught.ToString();

    // TODO: handle the placement persisting
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
    foreach (PagesEnum pageEnum in System.Enum.GetValues(typeof(PagesEnum)))
    {
      ResetCanvas(pageEnum);
    }
  }

  void ActivateCanvas(PagesEnum pageEnum)
  {
    GetGameObjectFromEnum(pageEnum).SetActive(true);
  }

  // Switch to the new canvas and set the active page.
  public void SwitchCanvas(PagesEnum pageEnum)
  {
    // TODO: Reset all the data.
    // ResetAllData();

    // Reset all screens.
    ResetAllCanvases();

    // Turn off coaching.
    ActivateCoaching(false);

    // Turn on the screen we care about.
    ActivateCanvas(pageEnum);
    activePage = pageEnum;

    switch (pageEnum)
    {
      case PagesEnum.SetupPage:
        // Turn on coaching.
        ActivateCoaching(true);
        break;
      case PagesEnum.GamePage:
        StartGameplay();
        break;
      case PagesEnum.SettingsPage:
        string name = PlayerPrefs.GetString("name");
        if (!name.Equals(""))
        {
          GameObject.Find("SettingsPageName").GetComponent<InputField>().text = name;
        }
        break;

    }
  }


  void ResetGame()
  {
    setupUI.SetActive(true);
    gameplayUI.SetActive(false);

    // reset the game
    timer = GAME_TIME; // one minute timer
    lastSecond = timer;
    timerText.text = timer.ToString("0:00");

  }

  void StartGameplay()
  {
    setupUI.SetActive(false);
    gameplayUI.SetActive(true);

    // reset the game
    timer = GAME_TIME; // one minute timer
    lastSecond = timer;
    timerText.text = timer.ToString("0:00");
  }

  // Turns on coaching mode.
  void ActivateCoaching(bool status)
  {
    if (Application.platform == RuntimePlatform.IPhonePlayer)
    {
      GameObject.Find("AR Session").GetComponent<ARKitCoachingOverlay>().ActivateCoachingMode(status);
    }
  }

  void UpdateGameplay()
  {
    timer -= Time.deltaTime;
    // only update the timer every second
    if (lastSecond - timer > 1.0f)
    {
      timerText.text = timer.ToString("0:00");
      lastSecond -= 1.0f;
    }

    if (timer < 0)
    {
      gameplayUI.SetActive(false);
      GameObject confettiObject = GameObject.Find("ConfettiCelebration");
      GameObject terrain = GameObject.Find("Terrain");
      for (float i = Mathf.Ceil((float)-GameLogicController.CATCH_DISTANCE); i <= Mathf.FloorToInt((float)GameLogicController.CATCH_DISTANCE); i += 0.5f)
      {
        for (float j = Mathf.Ceil((float)-GameLogicController.CATCH_DISTANCE); j <= Mathf.FloorToInt((float)GameLogicController.CATCH_DISTANCE); j += 0.5f)
        {
          GameObject newConfettiObject = Instantiate(confettiObject, new Vector3(i, 0, j), Quaternion.identity);
          newConfettiObject.transform.parent = terrain.transform;
          newConfettiObject.transform.localScale += new Vector3(10, 10, 10);
          newConfettiObject.GetComponent<ParticleSystem>().Play();
          Destroy(newConfettiObject, 3.0f);
        }
      }
      // TODO(averylamp): Activate end game

    }

  }


  // Update is called once per frame
  void Update()
  {


    // if the game is active
    if (gameplayUI.activeSelf)
    {
      UpdateGameplay();
    }
  }
}
