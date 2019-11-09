#if ICODE
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unitycoding.UIWidgets;

namespace ICode.Actions.UIWidgets{
	[Category("UI Widgets")]    
	[Tooltip("Shows the widget if it extsts.")]
	[System.Serializable]
	public class Show : StateAction {
		[Tooltip("Widget name")]
		[InspectorLabel("Name")]
		public FsmString _name;

		public override void OnEnter ()
		{
			UIWidget widget = UIUtility.Find<UIWidget> (_name.Value);
			if (widget != null) {
				widget.Show();
			}
			Finish ();
		}
	}
}
#endif