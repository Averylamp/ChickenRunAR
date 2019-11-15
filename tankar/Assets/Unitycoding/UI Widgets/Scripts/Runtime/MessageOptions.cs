using UnityEngine;
using System.Collections;

namespace Unitycoding.UIWidgets{
	[System.Serializable]
	/// <summary>
	/// Message options.
	/// </summary>
	public class MessageOptions {
		/// <summary>
		/// The title to display.
		/// </summary>
		public string title = string.Empty;
		/// <summary>
		/// The message to display.
		/// </summary>
		public string text = string.Empty;
		/// <summary>
		/// The color of the text.
		/// </summary>
		public Color color = Color.white;
		/// <summary>
		/// The icon to display.
		/// </summary>
		public Sprite icon;
		/// <summary>
		/// The delay before fading
		/// </summary>
		public float delay = 2.0f;
		/// <summary>
		/// The duration of fading.
		/// </summary>
		public float duration = 2.0f;
		/// <summary>
		/// Ignore TimeScale.
		/// </summary>
		public bool ignoreTimeScale = true;
		
		public MessageOptions(MessageOptions other){
			this.title = other.title;
			this.text=other.text;
			this.icon = other.icon;
			this.color=other.color;
			this.duration=other.duration;
			this.ignoreTimeScale=other.ignoreTimeScale;
		}
		
		public MessageOptions(){}
	}
}