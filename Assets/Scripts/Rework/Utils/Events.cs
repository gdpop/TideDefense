using UnityEngine;
using UnityEngine.Events;

namespace TideDefense
{
	public delegate void ColliderDelegate(Collider collider);
    public delegate void FloatDelegate(float value);
    public delegate void BoolEvent(bool value);
public delegate void GameObjectEvent(GameObject value);

}