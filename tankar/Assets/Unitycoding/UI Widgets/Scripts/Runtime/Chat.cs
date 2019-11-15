using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Unitycoding.UIWidgets
{
	public class Chat : UIWidget
	{
		[Header ("Behaviour")]
		[SerializeField]
		protected string filterMask = "*";
		[SerializeField]
		[TextArea]
		protected string filter = "fuck, ass, piss, cunt, shit";

		[Header ("Reference")]
		[SerializeField]
		protected Text text;
		[SerializeField]
		protected InputField input;
		[SerializeField]
		protected Button submit;

		private string[] filterWords;


		#if UNITY_EDITOR
		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Chat")]
		static void AddWidgetComponent ()
		{
			UnityEditor.Selection.activeGameObject.AddComponent<Chat> ();
		}

		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Chat", true)]
		static bool ValidateAddWidgetComponent ()
		{
			return UnityEditor.Selection.activeGameObject != null;
		}
		#endif


		protected override void OnStart ()
		{
			base.OnStart ();
			input.onEndEdit.AddListener (Submit);
			if (this.submit != null) {
				submit.onClick.AddListener (delegate {
					Submit (input.text);
				});
			}
			filterWords = filter.Replace (" ", "").Split (',');
		}

		private void Submit (string text)
		{
			if (!string.IsNullOrEmpty (text)) {
				text = ApplyFilter (text);
				OnSubmit (text);
			}
			this.input.text = "";
		}

		protected virtual void OnSubmit (string text)
		{
			this.text.text += "\n" + text;
		}

		protected virtual string ApplyFilter (string text)
		{
			string result = text;
			for (int i = 0; i < this.filterWords.Length; i++) {
				string filter = this.filterWords [i];
				result = result.Replace (filter, new System.Text.StringBuilder ().Insert (0, filterMask, filter.Length).ToString ());
			}
			return result;
		}
	}
}
