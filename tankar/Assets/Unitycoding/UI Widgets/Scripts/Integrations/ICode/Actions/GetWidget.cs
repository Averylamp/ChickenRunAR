#if ICODE
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unitycoding.UIWidgets;

namespace ICode.Actions.UIWidgets{
	[Category("UI Widgets")]    
	[Tooltip("Get the UIWidget if extsts.")]
	[System.Serializable]
	public class GetWidget : StateAction {
		[Tooltip("Widget name")]
		[InspectorLabel("Name")]
		public FsmString _name;
		[Shared]
		public FsmGameObject store;

		public override void OnEnter ()
		{
			UIWidget widget = UIUtility.Find<UIWidget> (_name.Value);
			if (widget != null) {
				store.Value=widget.gameObject;
			}
			Finish ();
		}
	}
}
#endif