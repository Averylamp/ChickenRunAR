using UnityEngine;
using System.Collections;
using System;
using System.Globalization;

namespace Unitycoding{
	public static class UnityUtility {
		private static AudioSource audioSource;
		/// <summary>
		/// Play an AudioClip.
		/// </summary>
		/// <param name="clip">Clip.</param>
		/// <param name="volume">Volume.</param>
		public static void PlaySound(AudioClip clip, float volume){
			if (clip == null) {
				return;
			}
			if (audioSource == null) {
				AudioListener listener = GameObject.FindObjectOfType<AudioListener> ();
				if(listener != null){
					audioSource=listener.GetComponent<AudioSource>();
					if(audioSource == null){
						audioSource=listener.gameObject.AddComponent<AudioSource>();
					}
				}
			}
			if (audioSource != null) {
				audioSource.PlayOneShot (clip, volume);
			}
		}

		/// <summary>
		/// Converts a color to hex.
		/// </summary>
		/// <returns>Hex string</returns>
		/// <param name="color">Color.</param>
		public static string ColorToHex(Color32 color)
		{
			string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
			return hex;
		}

		/// <summary>
		/// Converts a hex string to color.
		/// </summary>
		/// <returns>Color</returns>
		/// <param name="hex">Hex.</param>
		public static Color HexToColor(string hex)
		{
			hex = hex.Replace ("0x", "");
			hex = hex.Replace ("#", "");
			byte a = 255;
			byte r = byte.Parse(hex.Substring(0,2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2,2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			if(hex.Length == 8){
				a = byte.Parse(hex.Substring(4,2), System.Globalization.NumberStyles.HexNumber);
			}
			return new Color32(r,g,b,a);
		}

		/// <summary>
		/// Colors the string.
		/// </summary>
		/// <returns>The colored string.</returns>
		/// <param name="value">Value.</param>
		/// <param name="color">Color.</param>
		public static string ColorString(string value,Color color){
			return "<color=#" + UnityUtility.ColorToHex (color) + ">" + value + "</color>";
		}

		/// <summary>
		/// Replaces a string ignoring case.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="oldString">Old string.</param>
		/// <param name="newString">New string.</param>
		public static string Replace(string source, string oldString, string newString)
		{
			int index = source.IndexOf(oldString, StringComparison.CurrentCultureIgnoreCase);
			
			// Determine if we found a match
			bool MatchFound = index >= 0;
			
			if (MatchFound)
			{
				// Remove the old text
				source = source.Remove(index, oldString.Length);
				
				// Add the replacemenet text
				source = source.Insert(index, newString);            
			}
			return source;
		}

		/// <summary>
		/// Determines if the object is numeric.
		/// </summary>
		/// <returns><c>true</c> if is numeric the specified expression; otherwise, <c>false</c>.</returns>
		/// <param name="expression">Expression.</param>
		public static bool IsNumeric(object expression)
		{
			if (expression == null)
				return false;
			
			double number;
			return Double.TryParse(Convert.ToString(expression, CultureInfo.InvariantCulture), System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo, out number);
		}

		/// <summary>
		/// Finds the child by name.
		/// </summary>
		/// <returns>The child.</returns>
		/// <param name="target">Target.</param>
		/// <param name="name">Name.</param>
		/// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
		public static GameObject FindChild(this GameObject target, string name, bool includeInactive)
		{
			if (target != null) {
				if (target.name == name && includeInactive || target.name == name && !includeInactive && target.activeInHierarchy) {
					return target;
				}
				for (int i = 0; i < target.transform.childCount; ++i) {
					GameObject result = target.transform.GetChild (i).gameObject.FindChild ( name,includeInactive);
					
					if (result != null) 
						return result;
				}
			}
			return null;
		}

		public static void Stretch(this RectTransform rectTransform,RectOffset offset){
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.sizeDelta = new Vector2(-(offset.right+offset.left),-(offset.bottom+offset.top));
			rectTransform.anchoredPosition = new Vector2(offset.left+rectTransform.sizeDelta.x*rectTransform.pivot.x,-offset.top-rectTransform.sizeDelta.y*(1f-rectTransform.pivot.y));
		}
		
		public static void Stretch(this RectTransform rectTransform){
			rectTransform.anchorMin = Vector2.zero;
			rectTransform.anchorMax = Vector2.one;
			rectTransform.sizeDelta = Vector2.zero;
			rectTransform.anchoredPosition = Vector2.zero;
		}

		public static void SetActiveObjectsOfType<T>(bool state) where T: Component{

			T[] objects = GameObject.FindObjectsOfType<T> ();
			for (int i=0; i< objects.Length; i++) {
				objects[i].gameObject.SetActive(state);
			}
		}
	}
}

