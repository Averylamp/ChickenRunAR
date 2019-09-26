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


    public bool mouse_button_down = false;
    public bool finger_touch_down = false;
    public static GameObject last_clicked_game_object = null;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting ClickLogic.");
        ar_origin = FindObjectOfType<ARRaycastManager>();
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

        // print current game object if it exists
        if (last_clicked_game_object != null) {
            Debug.Log(last_clicked_game_object.name);
            // Make a call to the game object if HandleClick() is defined for the button.
            // Then continue.
            // TODO(ethan): finish a call like this.
            if (last_clicked_game_object.name == "FireButton") {
                GameObject.Find("TitleText").GetComponent<UnityEngine.UI.Text>().text += "a";
            }
            last_clicked_game_object = null;
        }

        mouse_button_down = false;
        finger_touch_down = false;
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
