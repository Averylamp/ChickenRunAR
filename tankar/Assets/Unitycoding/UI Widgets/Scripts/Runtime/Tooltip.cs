using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Unitycoding.UIWidgets{
	public class Tooltip : UIWidget {
		[Header("Reference")]
		/// <summary>
		/// The Text component to display tooltip text.
		/// </summary>
		public Text text;
		/// <summary>
		/// The Image component to display the icon.
		/// </summary>
		public Image icon;
		/// <summary>
		/// The background image.
		/// </summary>
		public Image background;
		/// <summary>
		/// Update position to follow mouse 
		/// </summary>
		public bool updatePosition;

		private float width=300f;
		private Canvas canvas;
		private bool _updatePosition;

		protected override void OnStart ()
		{
			base.OnStart ();
			canvas = GetComponentInParent<Canvas> ();
			width = rectTransform.sizeDelta.x;
			if (IsVisible) {
				Close ();
			}
		}

		protected virtual void Update ()
		{
			if (updatePosition && canvasGroup.alpha > 0f && _updatePosition) {
				UpdatePosition ();
			}
		}
		
		private void UpdatePosition(){
			Vector2 pos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
			Vector2 offset=Vector2.zero;
			
			if (Input.mousePosition.x < rectTransform.sizeDelta.x) {
				offset += new Vector2 (rectTransform.sizeDelta.x * 0.5f, 0);
			} else {
				offset += new Vector2(-rectTransform.sizeDelta.x*0.5f,0);
			}
			if(Screen.height- Input.mousePosition.y > rectTransform.sizeDelta.y) {
				offset += new Vector2 (0, rectTransform.sizeDelta.y * 0.5f);
			} else {
				offset += new Vector2 (0, -rectTransform.sizeDelta.y * 0.5f);
			}
			pos=pos+offset;
			
			transform.position = canvas.transform.TransformPoint(pos);
			Focus ();
		}

		/// <summary>
		/// Show this widget.
		/// </summary>
		public override void Show ()
		{
			base.Show ();
			canvasGroup.interactable = false;
			canvasGroup.blocksRaycasts = false;
		}

		/// <summary>
		/// Show this widget.
		/// </summary>
		/// <param name="tooltipText">Tooltip text.</param>
		public virtual void Show(string tooltipText){
			Show (tooltipText, this.width, true);
		}

		/// <summary>
		/// Show this widget.
		/// </summary>
		/// <param name="tooltipText">Tooltip text.</param>
		/// <param name="width">Width.</param>
		/// <param name="showBackground">If set to <c>true</c> show background.</param>
		public virtual void Show(string tooltipText, float width, bool showBackground){
			Show (tooltipText,null, width, showBackground);
		}

		/// <summary>
		/// Show this widget.
		/// </summary>
		/// <param name="tooltipText">Tooltip text.</param>
		/// <param name="icon">Icon.</param>
		/// <param name="width">Width.</param>
		/// <param name="showBackground">If set to <c>true</c> show background.</param>
		public virtual void Show(string tooltipText,Sprite icon, float width, bool showBackground){
			if (!string.IsNullOrEmpty (tooltipText)) {
				this.text.text = tooltipText;
				if (icon != null) {
					this.icon.sprite = icon;
					this.icon.transform.parent.gameObject.SetActive (true);
				} else {
					this.icon.transform.parent.gameObject.SetActive (false);
				}
				rectTransform.sizeDelta = new Vector2 (width, rectTransform.sizeDelta.y);
				this.background.enabled = showBackground;
				this._updatePosition=true;
				UpdatePosition ();
				Show ();
			} 
		}

		/// <summary>
		/// Close this widget.
		/// </summary>
		public override void Close ()
		{
			base.Close ();
			_updatePosition = false;
		}
	}
}