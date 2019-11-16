using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;
public class UILogicController : MonoBehaviour
{

  // timer logic
  static float timer = 60.0f; // one minute timer

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
    timer = 60.0f; // one minute timer
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

    // Turn on the screen we care about.
    ActivateCanvas(pageEnum);
    activePage = pageEnum;
  }


  void ResetGame()
  {
    setupUI.SetActive(true);
    gameplayUI.SetActive(false);

    // reset the game
    timer = 60.0f; // one minute timer
    lastSecond = timer;
    timerText.text = timer.ToString("0:00");

  }

  void StartGameplay()
  {
    setupUI.SetActive(false);
    gameplayUI.SetActive(true);

    // reset the game
    timer = 60.0f; // one minute timer
    lastSecond = timer;
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
      gameplayUI
.SetActive(false);
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
