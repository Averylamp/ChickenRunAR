#if ICODE
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unitycoding.UIWidgets;

namespace ICode.Actions.UIWidgets{
	[Category("UI Widgets")]    
	[Tooltip("Closes the widget.")]
	[System.Serializable]
	public class Close: StateAction {
		[Tooltip("Widget name")]
		[InspectorLabel("Name")]
		public FsmString _name;

		public override void OnEnter ()
		{
			UIWidget widget = UIUtility.Find<UIWidget> (_name.Value);
			if (widget != null) {
				widget.Close();
			}
			Finish ();
		}
	}
}
#endif