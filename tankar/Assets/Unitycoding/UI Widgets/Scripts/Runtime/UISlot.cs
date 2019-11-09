using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace Unitycoding.UIWidgets
{
	public class UISlot<T> : MonoBehaviour,IDropHandler,IBeginDragHandler,IDragHandler, IEndDragHandler,IPointerUpHandler, IPointerDownHandler where T:class
	{
		/// <summary>
		/// The id of the slot.
		/// </summary>
		[HideInInspector]
		[System.NonSerialized]
		public int id = -1;
		/// <summary>
		/// The container this slot belongs to.
		/// </summary>
		public UIContainer<T> container;
		/// <summary>
		/// Currently dragged item.
		/// </summary>
		public static T draggedItem;
		public static bool draggedReference;

		/// <summary>
		/// Gets the observed item of the slot.
		/// </summary>
		/// <value>The observed item.</value>
		public T observedItem {
			get {
				return (container != null) ? container.GetItem (id) : null;
			}
		}

		protected ScrollRect scrollRect;
		protected bool placingItem;

		/// <summary>
		/// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
		/// </summary>
		protected virtual void Start ()
		{
			container = GetComponentInParent<UIContainer<T>> ();
			id = container.Slots.IndexOf (this);
			scrollRect = GetComponentInParent<ScrollRect> ();
		}

		/// <summary>
		/// Replace the item and returns the previous item. 
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual T Replace (T item)
		{
			return (container != null) ? container.Replace (id, item) : item;
		}

		/// <summary>
		/// Updates the slot when an item changes.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual void UpdateSlot (T item)
		{
		}

		public virtual void OnBeginDrag (PointerEventData eventData)
		{
			
			if (observedItem != null && container.canDragItems) {
				if (!container.removeDraggedItems) {
					draggedItem = observedItem;
				} else {
					draggedItem = Replace (null);
				}
			}
			if (scrollRect != null) {
				scrollRect.OnBeginDrag (eventData);
			}
		}

		public virtual void OnDrag (PointerEventData data)
		{
			if (scrollRect != null) {
				scrollRect.OnDrag (data);
			}
		}

		public virtual void OnEndDrag (PointerEventData eventData)
		{
			if (Validate (draggedItem) && container.canDropItems) {
				T item = Replace (draggedItem);
				draggedItem = item;
			}
			if (scrollRect != null) {
				scrollRect.OnEndDrag (eventData);
			}
		}

		public virtual void OnDrop (PointerEventData data)
		{
			if (Validate (draggedItem) && container.canDropItems) {
				T item = Replace (draggedItem);
				draggedItem = item;
			} 
			container.OnDrop (data);	
		}

		public virtual void OnPointerUp (PointerEventData eventData)
		{
			if (eventData.clickCount > 1 && container.onDoubleClick != null) {
				container.onDoubleClick.Invoke (observedItem);
			} 
			if (eventData.clickCount == 1 && container.onClick != null) {
				container.onClick.Invoke (observedItem);
			}
			placingItem = draggedItem != null;
		}

		public virtual void OnPointerDown (PointerEventData eventData)
		{
			placingItem = draggedItem != null;
			//Unstacking...
			if (Validate (draggedItem) && container.canDropItems) {
				T item = Replace (draggedItem);
				draggedItem = item;
			}
		}

		/// <summary>
		/// Validate the item.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual bool Validate (T item)
		{
			if (item != null) {
				List<Component> validations = GetComponents (typeof(Component)).ToList ();
				if (container != null) {
					validations.AddRange (container.GetComponents (typeof(Component)));
				}
				for (int i = 0; i < validations.Count; i++) {
					if (validations [i] is IValidation<T>) {
						IValidation<T> validation = validations [i] as IValidation<T>;
						return validation.Validate (item);
					}
				}
				return true;
			}
			return false;
		}

		public virtual void MoveTo (string container)
		{
			UIContainer<T> mContainer = UIUtility.Find<UIContainer<T>> (container);
			if (mContainer != null) {
				T item = Replace (null);
				if (!mContainer.Add (item)) {
					Replace (item);
				}
			}
		}
	}
}