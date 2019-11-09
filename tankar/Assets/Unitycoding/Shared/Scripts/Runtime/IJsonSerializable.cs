using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unitycoding{
	public interface IJsonSerializable {
		void GetObjectData(Dictionary<string,object> data);
		void SetObjectData(Dictionary<string,object> data);
	}
}