using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Unitycoding.UIWidgets{
	public static class UIUtility {
		/// <summary>
		/// The widget cache.
		/// </summary>
		private static Dictionary<string,UIWidget> widgetCache= new Dictionary<string, UIWidget>();
		/// <summary>
		/// Get an UIWidget by name.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T Find<T>(string name) where T: UIWidget{
			
			UIWidget current = null;
			if(!widgetCache.TryGetValue (name, out current) || current==null || current.GetType().IsAssignableFrom(typeof(T))){;
				Canvas[] canvas = GameObject.FindObjectsOfType<Canvas>();
				for(int c=0;c<canvas.Length;c++){
					T[] windows = canvas[c].GetComponentsInChildren<T> (true);
					for (int i = 0; i < windows.Length; i++) {
						T window=windows[i];
						if(window.Name == name){
							current=window;
						}
						if(!widgetCache.ContainsKey(window.Name)){
							widgetCache.Add(window.Name,window);
						}
					}
				}
			}
			return (T)current;
		}
		
		/// <summary>
		/// Get UIWidgets by names
		/// </summary>
		/// <param name="names">Names.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T[] Find<T>(params string[] names) where T: UIWidget{
			List<T> list = new List<T> ();
			if (names.Length > 0) {
				for (int i=0; i<names.Length; i++) {
					T window = Find<T> (names [i]);
					if (window != null) {
						list.Add (window);
					}
				}
			} else {
				foreach(KeyValuePair<string,UIWidget> kvp in widgetCache){
					if(typeof(T).IsAssignableFrom(kvp.Value.GetType())){
						list.Add((T)kvp.Value);
					}
				}
			} 
			return list.ToArray ();
		}

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
			return "<color=#" + ColorToHex (color) + ">" + value + "</color>";
		}
	}
}