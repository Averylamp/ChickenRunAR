using UnityEngine;
using System.Collections;

namespace Unitycoding.UIWidgets{
	public interface IValidation<T> {
		bool Validate(T item);
	}
}