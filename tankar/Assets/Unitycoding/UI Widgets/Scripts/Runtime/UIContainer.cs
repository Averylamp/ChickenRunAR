using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Unitycoding.UIWidgets
{
	public class UIContainer<T> : UIWidget, IDropHandler where T : class
	{
		[Header ("Behaviour")]
		/// <summary>
		/// Sets the container as dynamic. Slots are instantiated at runtime.
		/// </summary>
		[SerializeField]
		protected bool dynamicContainer = false;
		/// <summary>
		/// The parent transform of slots. 
		/// </summary>
		[SerializeField]
		protected Transform grid;
		/// <summary>
		/// The slot prefab. This game object should contain the ItemSlot component or a child class of ItemSlot. 
		/// </summary>
		[SerializeField]
		protected GameObject slotPrefab;
		/// <summary>
		/// Is the containr referenced?
		/// </summary>
		public bool reference = false;
		/// <summary>
		/// Can items be draged from the container.
		/// </summary>
		public bool canDragItems = true;
		/// <summary>
		/// Can items be droped to the container.
		/// </summary>
		public bool canDropItems = true;
		/// <summary>
		/// Remove items when dragged.
		/// </summary>
		public bool removeDraggedItems = true;
		
		private List<UISlot<T>> slots = new List<UISlot<T>> ();

		/// <summary>
		/// All slots of the container.
		/// </summary>
		/// <value>The slots.</value>
		public List<UISlot<T>> Slots { 
			get { return slots; } 
		}

		private List<T> items = new List<T> ();

		/// <summary>
		/// All items of the container. Item can be null!
		/// </summary>
		/// <value>The items.</value>
		public List<T> Items { 
			get { 
				while (items.Count < slots.Count) { 
					items.Add (null); 
				}
				return items; 
			} 
		}

		#region Events

		/// <summary>
		/// Called when a new item is added to the container.
		/// </summary>
		public ContainerEvent onAdd = new ContainerEvent ();
		/// <summary>
		/// Called when a new item is added to the container.
		/// </summary>
		public ContainerEvent onFailedToAdd = new ContainerEvent ();
		/// <summary>
		/// Called when an item is removed from the container.
		/// </summary>
		public ContainerEvent onRemove = new ContainerEvent ();
		/// <summary>
		/// Called when an ItemSlot is double clicked.
		/// </summary>
		public ContainerEvent onDoubleClick = new ContainerEvent ();
		/// <summary>
		/// Called when a new item is dropped to the container.
		/// </summary>
		public ContainerEvent onDrop = new ContainerEvent ();
		/// <summary>
		/// Called when an ItemSlot is clicked.
		/// </summary>
		public ContainerEvent onClick = new ContainerEvent ();

		public UnityEvent onChange = new UnityEvent ();

		#endregion

		private bool prevDragState, prevDropState, prevRemoveState;
		private bool invokeOnChange = true;

		protected override void OnAwake ()
		{
			base.OnAwake ();
			RefreshSlots ();
			prevDragState = canDragItems;
			prevDropState = canDropItems;
			prevRemoveState = removeDraggedItems;
			onAdd.AddListener (delegate(T arg) {
				if (onChange != null && invokeOnChange) {
					onChange.Invoke ();
				}
			});
			onRemove.AddListener (delegate(T arg) {
				if (onChange != null && invokeOnChange) {
					onChange.Invoke ();
				}
			});
		}

		
		/// <summary>
		/// Replace the item and returns the previous item. 
		/// </summary>
		/// <param name="slotId">Slot identifier.</param>
		/// <param name="item">Item.</param>
		public virtual T Replace (int slotId, T item)
		{
			if (slotId < slots.Count) {
				T prev = Items [slotId];
				if (onRemove != null) {
					onRemove.Invoke (prev);
				}
				if (onAdd != null) {
					onAdd.Invoke (item);
				}
				Items [slotId] = item;
				slots [slotId].UpdateSlot (item);
				return prev;
			}
			return item;
		}

		/// <summary>
		/// Add an item to the container.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual bool Add (T item)
		{
			for (int i = 0; i < slots.Count; i++) {
				if (Items [i] == null && slots [i].Validate (item)) {
					Replace (i, item);
					return true;
				}
			}
			if (dynamicContainer) {
				CreateSlot ();
				return Add (item);
				
			}
			if (onFailedToAdd != null) {
				onFailedToAdd.Invoke (item);
			}
			return false;
		}

		public GameObject CreateSlot ()
		{
			if (slotPrefab != null && grid != null) {
				GameObject go = (GameObject)Instantiate (slotPrefab);
				go.SetActive (true);
				go.transform.SetParent (grid, false);
				RefreshSlots ();
				return go;
			}
			return null;
		}

		/// <summary>
		/// Refreshs the slots.
		/// </summary>
		public void RefreshSlots ()
		{
			if (dynamicContainer && grid != null) {
				slots = grid.GetComponentsInChildren<UISlot<T>> (true).ToList ();
				slots.Remove (slotPrefab.GetComponent<UISlot<T>> ());
			} else {
				slots = GetComponentsInChildren<UISlot<T>> (true).ToList ();
			}
		}

		/// <summary>
		/// Removes all items from this container and destroys slots if dynamic.
		/// </summary>
		public virtual void Clear ()
		{
			for (int i = 0; i < slots.Count; i++) {
				Replace (i, null);
			}
			if (dynamicContainer) {
				for (int i = 0; i < slots.Count; i++) {
					DestroyImmediate (slots [i].gameObject);		
				}
				RefreshSlots ();
			}
		}

		/// <summary>
		/// Gets the item by slot id.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="slotId">Slot identifier.</param>
		public T GetItem (int slotId)
		{ 
			return (slotId < Items.Count) ? Items [slotId] : null; 
		}

		/// <summary>
		/// Moves the items.
		/// </summary>
		/// <param name="container">Container.</param>
		public void MoveTo (string container)
		{
			UIContainer<T> mContainer = UIUtility.Find<UIContainer<T>> (container);
			if (mContainer != null) {
				invokeOnChange = false;
				for (int i = 0; i < Items.Count; i++) {
					T item = Replace (i, null);
					if (item != null && !mContainer.Add (item)) {
						Replace (i, item);

					}
				}
				invokeOnChange = true;
				if (onChange != null) {
					onChange.Invoke ();
				}
			}
		}

		public void OnDrop (PointerEventData eventData)
		{
			if (UISlot<T>.draggedItem != null && onDrop != null) {
				onDrop.Invoke (UISlot<T>.draggedItem);
			}
		}


		public virtual void Lock (bool state)
		{
			if (state) {
				canDragItems = false;
				canDropItems = false;
				removeDraggedItems = false;
			} else {
				canDragItems = prevDragState;
				canDropItems = prevDropState;
				removeDraggedItems = prevRemoveState;
			}

		}

		[System.Serializable]
		public class ContainerEvent:UnityEvent<T>
		{

		}
		
	}
}