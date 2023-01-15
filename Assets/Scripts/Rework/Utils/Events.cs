using UnityEngine;
using UnityEngine.Events;

namespace TideDefense
{
	public delegate void ColliderDelegate(Collider collider);
    public delegate void FloatEvent(float value);
    public delegate void BoolEvent(bool value);
}