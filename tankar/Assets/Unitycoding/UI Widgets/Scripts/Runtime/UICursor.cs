using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Unitycoding.UIWidgets
{
	[RequireComponent (typeof(Image), typeof(CanvasGroup))]
	public class UICursor : MonoBehaviour
	{
		private static UICursor instance;
		
		private RectTransform rectTransform;
		private Image image;
		private CanvasGroup canvasGroup;
		private Canvas canvas;

		#if UNITY_EDITOR
		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Cursor")]
		static void AddWidgetComponent ()
		{
			UnityEditor.Selection.activeGameObject.AddComponent<UICursor> ();
		}

		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Cursor", true)]
		static bool ValidateAddWidgetComponent ()
		{
			return UnityEditor.Selection.activeGameObject != null;
		}
		#endif


		private void Awake ()
		{
			instance = this; 
		}

		private void OnDestroy ()
		{ 
			instance = null;
		}

		private void Start ()
		{
			rectTransform = GetComponent<RectTransform> ();
			image = GetComponent<Image> ();
			canvas = GetComponentInParent<Canvas> ();
			canvasGroup = GetComponent<CanvasGroup> ();
			canvasGroup.alpha = 0f;
			canvasGroup.blocksRaycasts = false;
			canvasGroup.interactable = false;
		}

		void Update ()
		{
			if (instance.canvasGroup.alpha > 0) {
				Vector2 pos;
				RectTransformUtility.ScreenPointToLocalPointInRectangle (canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
				transform.position = canvas.transform.TransformPoint (pos);
			}
		}

		public static void Clear ()
		{	
			Set (null);
		}

		public static void Set (Sprite sprite)
		{
			if (instance != null && instance.image) {
				if (sprite != null) {
					instance.rectTransform.SetAsLastSibling ();
					instance.image.sprite = sprite;
					instance.canvasGroup.alpha = 1f;
				} else {
					instance.canvasGroup.alpha = 0f;
				}
			}
		}
		
	}
}