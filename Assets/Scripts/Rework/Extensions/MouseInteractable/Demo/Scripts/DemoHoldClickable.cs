namespace PierreMizzi.MouseInteractable
{
    using UnityEngine;

    public class DemoHoldClickable : MonoBehaviour
    {
        [SerializeField]
        private HoldClickable _holdClickable = null;

        private void Start()
        {
            HoldClickBehaviour leftClickBehaviour = _holdClickable.GetBehaviour(
                InteractableManager.MOUSE_LEFT
            );
            if (leftClickBehaviour != null)
            {
                leftClickBehaviour.onMouseDown += CallbackOnMouseDown;
                leftClickBehaviour.onClick += CallbackClick;
                
                leftClickBehaviour.onStartHoldClick += CallbackStartHoldClick;
                leftClickBehaviour.onProgressHoldClick += CallbackProgressHoldClick;
                leftClickBehaviour.onCompleteHoldClick += CallbackCompleteHoldClick;
                leftClickBehaviour.onCancelHoldClick += CallbackCancelHoldClick;
            }
        }

        void CallbackClick(RaycastHit hit)
        {
            Debug.Log($"CallbackClick");
        }

        void CallbackOnMouseDown(RaycastHit hit)
        {
            Debug.Log($"CallbackOnMouseDown");
        }


        void CallbackStartHoldClick()
        {
            Debug.Log($"OnStartHoldClick");
        }

        void CallbackProgressHoldClick(float progress)
        {
            Debug.Log($"OnProgressHoldClick : {progress}");
        }

        void CallbackCompleteHoldClick()
        {
            Debug.Log($"OnCompleteHoldClick");
        }

        void CallbackCancelHoldClick()
        {
            Debug.Log($"OnCancelHoldClick");
        }
    }
}
