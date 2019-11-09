using UnityEngine;
using System.Collections;
using Unitycoding.UIWidgets;

public class MessageBoxExample : MonoBehaviour {
	public string title;
	public string message;
	public Sprite icon;
	public string[] options;

	private MessageBox messageBox;
	private MessageBox verticalMessageBox;

	private void Start(){
		messageBox = UIUtility.Find<MessageBox> ("MessageBox");
		verticalMessageBox = UIUtility.Find<MessageBox> ("VerticalMessageBox");
	}

	public void Show(){
		messageBox.Show(title,message,icon,null,options);
	}

	public void ShowWithCallback(){
		messageBox.Show(title,message,icon,OnMessageBoxResult,options);
	}

	private void OnMessageBoxResult(string result){
		messageBox.Show("Callback","Click result: "+ result,"OK");
	}

	public void ShowVerticalMessageBox(){
		verticalMessageBox.Show(title,message,icon,null,options);
	}
}
