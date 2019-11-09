#if ICODE
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unitycoding.UIWidgets;

namespace ICode.Actions.UIWidgets{
	[Category("UI Widgets")]    
	[Tooltip("Adds a message to the MessageContainer.")]
	[System.Serializable]
	public class AddMessage : StateAction {
		[Tooltip("Widget name.")]
		[InspectorLabel("Name")]
		public FsmString _name;
		[Tooltip("Message to add.")]
		public FsmString message;
		[Tooltip("Text color.")]
		public FsmColor color;
		[Tooltip("Duration of fade out.")]
		public FsmFloat duration;
		[Tooltip("Delay fade out.")]
		public FsmFloat delay;

		public override void OnEnter ()
		{
			MessageContainer mWindow = UIUtility.Find<MessageContainer> (_name.Value);
			if (mWindow != null) {
				MessageOptions settings= new MessageOptions();
				settings.text=message.Value;
				settings.color=color.Value;
				settings.duration=duration.Value;
				settings.delay=delay.Value;
				mWindow.Add(settings);
			}
			Finish ();
		}
	}
}
#endif