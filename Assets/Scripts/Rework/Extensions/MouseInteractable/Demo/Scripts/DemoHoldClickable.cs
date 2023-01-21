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
                leftClickBehaviour.OnStartHoldClick += CallbackStartHoldClick;
                leftClickBehaviour.OnProgressHoldClick += CallbackProgressHoldClick;
                leftClickBehaviour.OnCompleteHoldClick += CallbackCompleteHoldClick;
                leftClickBehaviour.OnCancelHoldClick += CallbackCancelHoldClick;
            }
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
