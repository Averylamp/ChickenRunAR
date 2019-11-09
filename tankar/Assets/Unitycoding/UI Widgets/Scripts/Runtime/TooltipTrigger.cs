using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Unitycoding.UIWidgets
{
	/// <summary>
	/// Tooltip trigger to display fixed tooltips
	/// </summary>
	public class TooltipTrigger : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
	{
		/// <summary>
		/// The name of the tooltip instance.
		/// </summary>
		[SerializeField]
		private string instanceName = "Tooltip";
		/// <summary>
		/// Show the background element
		/// </summary>
		[SerializeField]
		private bool showBackground;
		//Width to use, Height is set based on width
		[SerializeField]
		private float width = 300;
		/// <summary>
		/// Color of the text.
		/// </summary>
		[SerializeField]
		private Color color = Color.white;
		/// <summary>
		/// The text to display
		/// </summary>
		[TextArea]
		public string tooltip;
		/// <summary>
		/// Optionally show an icon
		/// </summary>
		public Sprite icon;

		private Tooltip instance;

		#if UNITY_EDITOR
		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Triggers/Tooltip")]
		static void AddWidgetComponent ()
		{
			UnityEditor.Selection.activeGameObject.AddComponent<TooltipTrigger> ();
		}

		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Triggers/Tooltip", true)]
		static bool ValidateAddWidgetComponent ()
		{
			return UnityEditor.Selection.activeGameObject != null;
		}
		#endif


		/// <summary>
		/// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		/// </summary>
		private void Start ()
		{
			//Find tooltip instance with name "Tooltip"
			instance = UIUtility.Find<Tooltip> (instanceName);
			//Check if an instance of UITooltip is located in scene
			if (enabled && instance == null) {
				//No instance -> disable trigger
				enabled = false;
			}
		}

		/// <summary>
		/// Called when the mouse pointer starts hovering the ui element.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerEnter (PointerEventData eventData)
		{
			//Show tooltip
			instance.Show (UIUtility.ColorString (tooltip, color), icon, width, showBackground);
		}

		/// <summary>
		/// Called when the mouse pointer exits the element
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public void OnPointerExit (PointerEventData eventData)
		{
			//Hide tooltip
			instance.Close ();
		}

		/// <summary>
		/// Called when the mouse enters a 3d model with a collider in scene.
		/// </summary>
		private void OnMouseEnter ()
		{
			//Show tooltip
			instance.Show (UIUtility.ColorString (tooltip, color), icon, width, showBackground);
		}

		/// <summary>
		/// Called when the mouse pointer exits a 3d model with a collider in scene
		/// </summary>
		private void OnMouseExit ()
		{
			//Hide tooltip
			instance.Close ();
		}
	}
}