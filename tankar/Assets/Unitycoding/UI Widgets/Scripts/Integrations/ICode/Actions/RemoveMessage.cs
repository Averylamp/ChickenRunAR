#if ICODE
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unitycoding.UIWidgets;

namespace ICode.Actions.UIWidgets{
	[Category("UI Widgets")]    
	[Tooltip("Removes a message from the MessageContainer.")]
	[System.Serializable]
	public class RemoveMessage : StateAction {
		[Tooltip("Widget name.")]
		[InspectorLabel("Name")]
		public FsmString _name;
		[Tooltip("Message to remove.")]
		public FsmString message;

		public override void OnEnter ()
		{
			MessageContainer mWindow = UIUtility.Find<MessageContainer> (_name.Value);
			if (mWindow != null) {
				mWindow.Remove(message.Value);
			}
			Finish ();
		}
	}
}
#endif