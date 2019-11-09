using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Unitycoding.UIWidgets
{
	public class Progressbar : UIWidget
	{
		[Header ("Reference")]
		[SerializeField]
		protected Image progressbar;
		[SerializeField]
		protected Text progressLabel;
		[SerializeField]
		protected string format = "F0";

		#if UNITY_EDITOR
		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Progressbar")]
		static void AddWidgetComponent ()
		{
			UnityEditor.Selection.activeGameObject.AddComponent<Progressbar> ();
		}

		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Progressbar", true)]
		static bool ValidateAddWidgetComponent ()
		{
			return UnityEditor.Selection.activeGameObject != null;
		}
		#endif


		protected override void OnStart ()
		{
			progressbar.type = Image.Type.Filled;
		}

		public virtual void SetProgress (float progress)
		{
			progressbar.fillAmount = progress;
			if (progressLabel != null) {
				progressLabel.text = (progress * 100f).ToString (format) + "%";
			}
		}

		public override void Show ()
		{
			progressbar.fillAmount = 0f;
			if (progressLabel != null) {
				progressLabel.text = "0%";
			}
			base.Show ();
		}
	}
}