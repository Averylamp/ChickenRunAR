#if ICODE
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unitycoding.UIWidgets;

namespace ICode.Actions.UIWidgets{
	[Category("UI Widgets")]    
	[Tooltip("Sets the progress of a Progessbar widget.")]
	[System.Serializable]
	public class SetProgress : StateAction {
		[Tooltip("Widget name.")]
		[InspectorLabel("Name")]
		public FsmString _name;
		[Tooltip("Progress to set")]
		public FsmFloat progress;
		[Tooltip("Execute the action every frame.")]
		public bool everyFrame;

		private Progressbar widget;

		public override void OnEnter ()
		{
			base.OnEnter ();
			widget = UIUtility.Find<Progressbar> (_name.Value);
			if (widget != null) {
				widget.SetProgress(progress.Value);
			}
			if (!everyFrame) {
				Finish ();
			}
		}

		public override void OnUpdate ()
		{
			if (widget != null) {
				widget.SetProgress(progress.Value);
			}
		}
	}
}
#endif