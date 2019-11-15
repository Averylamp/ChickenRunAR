using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unitycoding{
	public class CoroutineQueue
	{
		MonoBehaviour m_Owner = null;
		Coroutine m_InternalCoroutine = null;
		Queue<IEnumerator> actions = new Queue<IEnumerator>();
		public CoroutineQueue(MonoBehaviour aCoroutineOwner)
		{
			m_Owner = aCoroutineOwner;
		}
		public void Start()
		{
			m_InternalCoroutine = m_Owner.StartCoroutine(Process());
		}
		public void Stop()
		{
			m_Owner.StopCoroutine(m_InternalCoroutine);
			m_InternalCoroutine = null;
		}
		public void EnqueueAction(IEnumerator aAction)
		{
			actions.Enqueue(aAction);
		}
		
		private IEnumerator Process()
		{
			while (true)
			{
				if (actions.Count > 0)
					yield return m_Owner.StartCoroutine(actions.Dequeue());
				else
					yield return null;
			}
		}
	}
}