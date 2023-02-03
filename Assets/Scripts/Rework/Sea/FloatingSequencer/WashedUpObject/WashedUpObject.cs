namespace TideDefense
{
	using UnityEngine;
	using UnityEngine.Events;
	
	public class WashedUpObject : MonoBehaviour {
		
	#region Fields 

	public UnityEvent onInitialize = null;

	public GameObject _object = null;

	public Transform _washedUpContainer = null;

	#endregion 

	#region Methods 

	public virtual void Initialize()
	{
		Quaternion rndRotation = Quaternion.Euler(Random.Range(0f, 359f), Random.Range(0f, 359f), Random.Range(0f, 359f));
		_washedUpContainer.rotation = rndRotation;
		onInitialize.Invoke();

		_object.transform.SetParent(transform.parent);
		Destroy(gameObject);
	}

	#endregion

	}
}