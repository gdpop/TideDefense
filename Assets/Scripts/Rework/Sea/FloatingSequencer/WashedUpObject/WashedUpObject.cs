namespace TideDefense
{
	using UnityEngine;
	using UnityEngine.Events;
	using CodesmithWorkshop.Useful;
	
	public class WashedUpObject : MonoBehaviour {
		
	#region Fields 

	public UnityEvent onWashUpComplete = null;

	public GameObject _object = null;

	public Transform _washedUpContainer = null;

	#endregion 

	#region Random Rotation

	[SerializeField] private Vector3 _minRotation = new Vector3(0, 0, 0);
	[SerializeField] private Vector3 _maxRotation = new Vector3(359, 359, 359);
		
	#endregion

	#region Methods 

	public virtual void Initialize()
	{
		_washedUpContainer.rotation = UtilsClass.RandomRotation(_minRotation, _maxRotation);
		onWashUpComplete.Invoke();

		_object.transform.SetParent(transform.parent);
		Destroy(gameObject);
	}

	#endregion

	}
}