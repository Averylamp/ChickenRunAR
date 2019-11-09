using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Unitycoding.UIWidgets
{
	[RequireComponent (typeof(CanvasGroup))]
	public class UIWidget : MonoBehaviour
	{
		[SerializeField]
		private new string name;

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name {
			get{ return name; }
			set{ name = value; }
		}

		[Header ("Appearence")]
		[SerializeField]
		/// <summary>
		/// Brings this window to front in Show()
		/// </summary>
		private bool focus = true;
		[SerializeField]
		/// <summary>
		/// The type of the ease.
		/// </summary>
		private EasingEquations.EaseType easeType;
		[SerializeField]
		/// <summary>
		/// The duration to tween this widget.
		/// </summary>
		private float duration = 1.0f;
		/// <summary>
		/// The AudioClip that will be played when this widget shows.
		/// </summary>
		public AudioClip showSound;
		/// <summary>
		/// The AudioClip that will be played when this widget closes.
		/// </summary>
		public AudioClip closeSound;
		
		[SerializeField]
		protected bool deactivateOnClose = true;
		
		[SerializeField]
		private List<Entry> m_Delegates = new List<Entry> ();

		public List<Entry> triggers {
			get {
				if (this.m_Delegates == null) {
					this.m_Delegates = new List<Entry> ();
				}
				return this.m_Delegates;
			}
			set {
				this.m_Delegates = value;
			}
		}

		
		/// <summary>
		/// Events that will be invoked when this widget is shows.
		/// </summary>
		[HideInInspector]
		public WidgetEvent onShow;
		/// <summary>
		/// Events that will be invoked when this widget closes.
		/// </summary>
		[HideInInspector]
		public WidgetEvent onClose;
		/// <summary>
		/// Events that will be invoked when this widget finished tween.
		/// </summary>
		[HideInInspector]
		public WidgetEvent onShowFinished;
		/// <summary>
		/// Events that will be invoked when this widget finished tween.
		/// </summary>
		[HideInInspector]
		public WidgetEvent onCloseFinished;

		/// <summary>
		/// Gets a value indicating whether this widget is visible.
		/// </summary>
		/// <value><c>true</c> if this instance is open; otherwise, <c>false</c>.</value>
		public bool IsVisible { 
			get { 
				return canvasGroup.alpha == 1f; 
			} 
		}

		/// <summary>
		/// The RectTransform of the widget.
		/// </summary>
		protected RectTransform rectTransform;
		/// <summary>
		/// The CanvasGroup of the widget.
		/// </summary>
		protected CanvasGroup canvasGroup;
		
		private TweenRunner<FloatTween> m_AlphaTweenRunner;
		private TweenRunner<Vector3Tween> m_ScaleTweenRunner;
		
		#if UNITY_EDITOR
		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Widget")]
		static void AddWidgetComponent ()
		{
			UnityEditor.Selection.activeGameObject.AddComponent<UIWidget> ();
		}

		[UnityEditor.MenuItem ("Tools/Unitycoding/UI Widgets/Components/Widget", true)]
		static bool ValidateAddWidgetComponent ()
		{
			return UnityEditor.Selection.activeGameObject != null;
		}
		#endif

		
		private void Awake ()
		{
			rectTransform = GetComponent<RectTransform> ();
			canvasGroup = GetComponent<CanvasGroup> ();
			if (!IsVisible) {
				rectTransform.localScale = Vector3.zero;
			}
			if (this.m_AlphaTweenRunner == null)
				this.m_AlphaTweenRunner = new TweenRunner<FloatTween> ();
			this.m_AlphaTweenRunner.Init (this);
			
			if (this.m_ScaleTweenRunner == null)
				this.m_ScaleTweenRunner = new TweenRunner<Vector3Tween> ();
			this.m_ScaleTweenRunner.Init (this);
			
			onShow.AddListener (delegate {
				Execute (TriggerType.OnShow);
			});
			onClose.AddListener (delegate {
				Execute (TriggerType.OnClose);
			});
			onCloseFinished.AddListener (delegate {
				Execute (TriggerType.OnCloseFinished);
			});
			onShowFinished.AddListener (delegate {
				Execute (TriggerType.OnShowFinished);
			});
			OnAwake ();
		}

		protected virtual void OnAwake ()
		{
		}

		private void Start ()
		{
			OnStart ();
			StartCoroutine (OnDelayedStart ());
		}

		protected virtual void OnStart ()
		{
		}

		private IEnumerator OnDelayedStart ()
		{
			yield return null;
			if (!IsVisible && deactivateOnClose) {
				gameObject.SetActive (false);
			}
		}

		/// <summary>
		/// Show this widget.
		/// </summary>
		public virtual void Show ()
		{
			deactivate = false;
			gameObject.SetActive (true);
			if (focus) {
				Focus ();
			}
			TweenCanvasGroupAlpha (canvasGroup.alpha, 1f);
			
			TweenTransformScale (Vector3.ClampMagnitude (rectTransform.localScale, 1.9f), Vector3.one);
			
			UIUtility.PlaySound (showSound, 1.0f);
			canvasGroup.interactable = true;
			canvasGroup.blocksRaycasts = true;
			if (onShow != null && canvasGroup.alpha < 1f) {
				onShow.Invoke ();
			}
		}

		/// <summary>
		/// Close this widget.
		/// </summary>
		public virtual void Close ()
		{
			deactivate = true;
			TweenCanvasGroupAlpha (canvasGroup.alpha, 0f);
			TweenTransformScale (rectTransform.localScale, Vector3.zero);
			
			UIUtility.PlaySound (closeSound, 1.0f);
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
			if (onClose != null && canvasGroup.alpha > 0f) {
				onClose.Invoke ();
			}
		}

		private FloatTween alphaTween;
		private bool deactivate;

		private void TweenCanvasGroupAlpha (float startValue, float targetValue)
		{
			if (!alphaTween.ValidTarget ()) {
				alphaTween = new FloatTween {
					easeType = easeType,
					duration = duration,
					startValue = startValue,
					targetValue = targetValue
				};
				alphaTween.AddOnChangedCallback ((float value) => {
					canvasGroup.alpha = value;
				});
				alphaTween.AddOnFinishCallback (() => {
					if (alphaTween.startValue > alphaTween.targetValue) {
						onCloseFinished.Invoke ();
						if (deactivateOnClose && deactivate) {
							gameObject.SetActive (false);
						}
					} else {
						onShowFinished.Invoke ();
					}
					
				});
			} else {
				alphaTween.startValue = startValue;
				alphaTween.targetValue = targetValue;
			}
			m_AlphaTweenRunner.StartTween (alphaTween);
		}

		private Vector3Tween scaleTween;

		private void TweenTransformScale (Vector3 startValue, Vector3 targetValue)
		{
			if (!scaleTween.ValidTarget ()) {
				scaleTween = new Vector3Tween {
					easeType = easeType,
					duration = duration,
					startValue = startValue,
					targetValue = targetValue
				};
				scaleTween.AddOnChangedCallback ((Vector3 value) => {
					rectTransform.localScale = value;
				});
			} else {
				scaleTween.startValue = startValue;
				scaleTween.targetValue = targetValue;
			}
			m_ScaleTweenRunner.StartTween (scaleTween);
		}

		/// <summary>
		/// Toggle the visibility of this widget.
		/// </summary>
		public virtual void Toggle ()
		{
			if (!IsVisible) {
				Show ();
			} else {
				Close ();
			}
		}

		/// <summary>
		/// Brings the widget to the top
		/// </summary>
		public virtual void Focus ()
		{
			rectTransform.SetAsLastSibling ();
		}

		private void Execute (TriggerType id)
		{
			int num = 0;
			int count = this.triggers.Count;
			while (num < count) {
				Entry item = this.triggers [num];
				if (item.eventID == id && item.callback != null) {
					item.callback.Invoke ();
				}
				num++;
			}
		}

		public enum TriggerType
		{
			OnShow,
			OnClose,
			OnShowFinished,
			OnCloseFinished
		}

		[System.Serializable]
		public class Entry
		{
			public TriggerType eventID;
			
			public WidgetEvent callback;

			public Entry ()
			{
			}
		}

		[System.Serializable]
		public class WidgetEvent:UnityEvent
		{

		}
	}
}