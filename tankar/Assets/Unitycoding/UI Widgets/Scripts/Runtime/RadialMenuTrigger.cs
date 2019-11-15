using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Unitycoding.UIWidgets
{
	public class RadialMenuTrigger : MonoBehaviour, IPointerDownHandler
	{
		/// <summary>
		/// The name of the tooltip instance.
		/// </summary>
		[SerializeField]
		protected string instanceName = "RadialMenu";
		/// <summary>
		/// Mouse button Left=0,Right=1,Middle=2
		/// </summary>
		[SerializeField]
		protected PointerEventData.InputButton button;
		/// <summary>
		/// The option icons
		/// </summary>
		[SerializeField]
		protected Sprite[] options;
		/// <summary>
		/// Events that should be called on selection
		/// </summary>
		public TriggerEvent onValueChanged;
		/// <summary>
		/// The radial menu instance
		/// </summary>
		protected RadialMenu radialMenu;

		#if UNITY_EDITOR
		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Triggers/Radial Menu")]
		static void AddWidgetComponent ()
		{
			UnityEditor.Selection.activeGameObject.AddComponent<RadialMenuTrigger> ();
		}

		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Triggers/Radial Menu", true)]
		static bool ValidateAddWidgetComponent ()
		{
			return UnityEditor.Selection.activeGameObject != null;
		}
		#endif


		/// <summary>
		/// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		/// </summary>
		protected virtual void Start ()
		{
			radialMenu = UIUtility.Find<RadialMenu> (instanceName);
		}

		public virtual void Show ()
		{
			//Show the radial menu
			radialMenu.Show (options, delegate(int index) {
				//Invoke events with index of selected option
				onValueChanged.Invoke (index);
			});
			
		}

		/// <summary>
		/// Called by a BaseInputModule when an OnPointerDown event occurs.
		/// </summary>
		/// <param name="eventData">Event data.</param>
		public virtual void OnPointerDown (PointerEventData eventData)
		{
			//Call the show method
			if (eventData.button == button) {
				Show ();
			}
		}

		protected virtual void OnMouseOver ()
		{
			if (Input.GetMouseButtonDown ((int)button)) {
				Show ();
			}
		}

		[System.Serializable]
		public class TriggerEvent:UnityEvent<int>
		{

		}
	}
}