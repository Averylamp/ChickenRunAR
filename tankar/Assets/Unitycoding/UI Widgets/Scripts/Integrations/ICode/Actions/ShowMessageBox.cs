#if ICODE
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unitycoding.UIWidgets;

namespace ICode.Actions.UIWidgets{
	[Category("UI Widgets")]    
	[Tooltip("Shows a message box window. When a button is clicked an event will be sended with the name of the button text.")]
	[System.Serializable]
	public class ShowMessageBox : StateAction {
		[Tooltip("Widget name.")]
		[InspectorLabel("Name")]
		public FsmString _name;
		[NotRequired]
		[Tooltip("Title to display.")]
		public FsmString title;
		[NotRequired]
		[Tooltip("Text to display.")]
		public FsmString text;
		[NotRequired]
		[Tooltip("Icon to display.")]
		public FsmObject icon;
		[Tooltip("Buttons to display")]
		public string[] buttons;

		public override void OnEnter ()
		{
			MessageBox mWindow = UIUtility.Find<MessageBox> (_name.Value);
			if (mWindow != null) {
				mWindow.Show(title.IsNone?string.Empty:title.Value,text.Value,icon.IsNone?null: icon.Value as Sprite,delegate(string result){this.Root.Owner.SendEvent(result,null);},buttons);
			}
			Finish ();
		}
	}
}
#endif