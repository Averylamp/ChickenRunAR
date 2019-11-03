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

    private const double CATCH_DISTANCE = 0.5;


    public bool mouse_button_down = false;
    public bool finger_touch_down = false;
    public static GameObject last_clicked_game_object = null;

    // timer logic
    static float timer = 60.0f; // one minute timer
    public float last_second = timer;
    public Text timer_text;

    // num chickens caught
    static int num_chickens_caught = 0;

    // modal for setup
    public GameObject gameplay_ui;
    public GameObject setup_ui;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting ClickLogic.");
        ar_origin = FindObjectOfType<ARRaycastManager>();

        // set setup to active but gameplay to not
        setup_ui.SetActive(true);
        gameplay_ui.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // If a mouse click is detected
        if (Input.GetMouseButtonDown(0)) {
            // Debug.Log("Button down.");
            mouse_button_down = true;
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            foreach (RaycastResult result in results)
            {
                last_clicked_game_object = result.gameObject;
                break;
            }
        }

        // TODO(ethan): confirm that these two if/else components are mutually exclusive
        // If a finger click is detected
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            finger_touch_down = true;
            Touch touch = Input.GetTouch(0);
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = touch.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);
            foreach (RaycastResult result in results)
            {
                last_clicked_game_object = result.gameObject;
                break;
            }
        }

        
        GameObject chicken = GameObject.Find("Chicken");
        float chickenDistance;
        if(Application.platform == RuntimePlatform.IPhonePlayer){ 
            GameObject mainCamera = GameObject.Find("AR Camera");        
            chickenDistance = Vector3.Distance(chicken.transform.position, mainCamera.transform.position);
        }else{
             GameObject humanCube = GameObject.Find("Human Cube");
            chickenDistance = Vector3.Distance(chicken.transform.position, humanCube.transform.position);
        }
       
         
        try{
            Image catchButton = GameObject.Find("Button_CatchChicken").GetComponent<Image>();
            print(catchButton);
            if (chickenDistance < CATCH_DISTANCE){
                catchButton.color = Color.green;
            }else{
                catchButton.color = Color.red;
            }
    
        }
        catch{
            
        }
        
        // print current game object if it exists
        if (last_clicked_game_object != null) {
            Debug.Log(last_clicked_game_object.name);
            // Make a call to the game object if HandleClick() is defined for the button.
            // Then continue.
            // TODO(ethan): finish a call like this.
            if (last_clicked_game_object.name == "Button_CatchChicken" && chickenDistance < CATCH_DISTANCE) {
                num_chickens_caught += 1;
                GameObject.Find("TitleText").GetComponent<UnityEngine.UI.Text>().text = num_chickens_caught.ToString();
            } else if (last_clicked_game_object.name == "Button_StartGame_Text") {
                setup_ui.SetActive(false);
                gameplay_ui.SetActive(true);

                // reset the game
                timer = 60.0f; // one minute timer
                last_second = timer;
            } else if (last_clicked_game_object.name == "Button_Home") {
                setup_ui.SetActive(true);
                gameplay_ui.SetActive(false);

                // reset the game
                timer = 60.0f; // one minute timer
                last_second = timer;
                timer_text.text = timer.ToString("0:00");
            }
            last_clicked_game_object = null;
        }

        mouse_button_down = false;
        finger_touch_down = false;

        // if the game is active
        if (gameplay_ui.activeSelf) {
            timer -= Time.deltaTime;
            // only update the timer every second
            if (last_second - timer > 1.0f) {
                timer_text.text = timer.ToString("0:00");
                last_second -= 1.0f;
            }
        }
    }

    public static bool OnUI() {
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
