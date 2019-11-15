using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//Import Namespace
using Unitycoding.UIWidgets;

/// <summary>
/// Message container example.
/// </summary>
public class MessageContainerExample : MonoBehaviour {
	//Reference to the MessageContainer in scene
	private MessageContainer messageContainer;
	//Options to display containing information about text, icon, fading duration...
	public MessageOptions[] options;

	private void Start(){
		//Find the reference to the MessageContainer
		messageContainer = UIUtility.Find<MessageContainer> ("Message");
	}

	/// <summary>
	/// Called from a button OnClick event in the example
	/// </summary>
	public void AddMessage(){
		//Get a random MessageOption from the array
		MessageOptions option=options[Random.Range(0,options.Length)];
		//Add the message
		messageContainer.Add(option);
	}

	/// <summary>
	/// Called from a button OnClick event in the example
	/// </summary>
	public void AddMessage(InputField input){
		//Add a text message
		messageContainer.Add (input.text);
	}

	/// <summary>
	/// Called from a Slider OnValueChanged event in the example
	/// </summary>
	public void AddMessage(float index){
		//Round the index to int and get the option from options array.
		MessageOptions option = options [Mathf.RoundToInt (index)];
		//Add the message
		messageContainer.Add (option);
	}
}
