using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Events;

namespace Unitycoding.UIWidgets
{
	public class MessageBox : UIWidget
	{
		/// <summary>
		/// Closes the window when a button is clicked.
		/// </summary>
		public bool autoClose = true;
		[Header ("Reference")]
		/// <summary>
		/// The title component reference
		/// </summary>
		public Text title;
		/// <summary>
		/// The text component reference
		/// </summary>
		public Text text;
		/// <summary>
		/// The icon sprite reference
		/// </summary>
		public Image icon;
		/// <summary>
		/// The button prefab reference
		/// </summary>
		public Button button;
	
		private List<Button> buttonCache = new List<Button> ();

		#if UNITY_EDITOR
		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Message Box")]
		static void AddWidgetComponent ()
		{
			UnityEditor.Selection.activeGameObject.AddComponent<MessageBox> ();
		}

		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Message Box", true)]
		static bool ValidateAddWidgetComponent ()
		{
			return UnityEditor.Selection.activeGameObject != null;
		}
		#endif


		public virtual void Show (MessageOptions settings, UnityAction<string> result, params string[] buttons)
		{
			Show (settings.title, UIUtility.ColorString (settings.text, settings.color), settings.icon, result, buttons);
		}

		/// <summary>
		/// Show the MessageBox
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="text">Text.</param>
		/// <param name="buttons">Buttons.</param>
		public virtual void Show (string title, string text, params string[] buttons)
		{
			Show (title, text, null, null, buttons);
		}

		/// <summary>
		/// Show the MessageBox
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="text">Text.</param>
		/// <param name="result">Result.</param>
		/// <param name="buttons">Buttons.</param>
		public virtual void Show (string title, string text, UnityAction<string> result, params string[] buttons)
		{
			Show (title, text, null, result, buttons);
		}

		/// <summary>
		/// Show the MessageBox
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="text">Text.</param>
		/// <param name="icon">Icon.</param>
		/// <param name="result">Result.</param>
		/// <param name="buttons">Buttons.</param>
		public virtual void Show (string title, string text, Sprite icon, UnityAction<string> result, params string[] buttons)
		{
			for (int i = 0; i < buttonCache.Count; i++) {
				buttonCache [i].onClick.RemoveAllListeners ();
				buttonCache [i].gameObject.SetActive (false);
			}
			if (this.title != null) {
				if (!string.IsNullOrEmpty (title)) {
					this.title.text = title;
					this.title.gameObject.SetActive (true);
				} else {
					this.title.gameObject.SetActive (false);
				}
			}
			if (this.text != null) {
				this.text.text = text;
			}

			if (this.icon != null) {
				if (icon != null) {
					this.icon.sprite = icon;
					this.icon.transform.parent.gameObject.SetActive (true);
				} else {
					this.icon.transform.parent.gameObject.SetActive (false);
				}
			}
			base.Show ();
			button.gameObject.SetActive (false);
			for (int i = 0; i < buttons.Length; i++) {
				string caption = buttons [i];
				AddButton (caption).onClick.AddListener (delegate() {
					if (this.autoClose) {
						base.Close ();
					}
					if (result != null) {
						result.Invoke (caption);
					}
				});
			}
		}

		private Button AddButton (string text)
		{
			Button mButton = buttonCache.Find (x => !x.isActiveAndEnabled);
			if (mButton == null) {
				mButton = Instantiate (button) as Button;
				buttonCache.Add (mButton);
			}
			mButton.gameObject.SetActive (true);
			mButton.onClick.RemoveAllListeners ();
			mButton.transform.SetParent (button.transform.parent, false);
			Text[] buttonTexts = mButton.GetComponentsInChildren<Text> (true);
			if (buttonTexts.Length > 0) {
				buttonTexts [0].text = text;
			}
			return mButton;
		}
	}
}