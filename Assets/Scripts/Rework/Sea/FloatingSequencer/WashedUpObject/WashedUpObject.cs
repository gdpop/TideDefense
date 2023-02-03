namespace TideDefense
{
	using UnityEngine;
	using UnityEngine.Events;
	using CodesmithWorkshop.Useful;
	
	public class WashedUpObject : MonoBehaviour {
		
	#region Fields 

	public UnityEvent onInitialize = null;

	public GameObject _object = null;

	public Transform _washedUpContainer = null;

	#endregion 

	#region Methods 

	public virtual void Initialize()
	{
		Quaternion rndRotation = UtilsClass.RandomRotation();
		_washedUpContainer.rotation = rndRotation;
		onInitialize.Invoke();

		_object.transform.SetParent(transform.parent);
		Destroy(gameObject);
	}

	#endregion

	}
}