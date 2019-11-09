#if ICODE
using UnityEngine;
using System.Collections;
using Unitycoding;
using Unitycoding.UIWidgets;

namespace ICode.Actions.UIWidgets{
	[Category("UI Widgets")]    
	[Tooltip("Remove all items from the UIContainer.")]
	[System.Serializable]
	public class ClearContainer : StateAction {
		[InspectorLabel("Name")]
		[Tooltip("Name of the Container set in the Inspector.")]
		public FsmString _name;
		
		public override void OnEnter ()
		{
			UIWidget window = UIUtility.Find<UIWidget> (_name.Value);
			if (window != null) {
				window.SendMessage("Clear");
			}
			Finish ();
		}
	}
}
#endif