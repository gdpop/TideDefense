using CodesmithWorkshop.Useful;
using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public class HoldClickSetting
    {
        // Settings
        public int mouseButtonID = -1;
        public float clickEnd = 0.3f;
        public float clickHoldStart = 1f;
        public float clickHoldDuration = 2f;

        // Runtime
        public HoldClickable currentHoldClickable = null;
        public HoldClickBehaviour currentClickBehaviour = null;

        public float currentHoldTime = 0f;

        public float currentHoldProgress
        {
            get { return GetHoldProgressFromHoldTime(currentHoldTime); }
        }

        public HoldClickSetting(int mouseButtonID, float treshold, float duration)
        {
            this.mouseButtonID = mouseButtonID;
            this.clickHoldStart = treshold;
            this.clickHoldDuration = duration;
        }

        public HoldClickStatus currentStatus
        {
            get { return GetClickStatusFromClickTime(currentHoldTime); }
        }

        public HoldClickStatus GetClickStatusFromClickTime(float time)
        {
            if (0f < currentHoldTime && currentHoldTime < clickEnd)
                return HoldClickStatus.inClick;
            if (clickEnd <= currentHoldTime && currentHoldTime < clickHoldStart)
                return HoldClickStatus.inTreshold;
            else if (clickHoldStart <= currentHoldTime && currentHoldTime < clickHoldDuration)
                return HoldClickStatus.inLong;
            else if (clickHoldDuration < currentHoldTime)
                return HoldClickStatus.completed;
            else
                return HoldClickStatus.None;
        }

        private float GetHoldProgressFromHoldTime(float time)
        {
            return Mathf.Clamp(
                UtilsClass.Remap(time, clickHoldStart, clickHoldDuration, 0f, 1f),
                0f,
                1f
            );
        }

        public void SetClickable(HoldClickable clickable)
        {
            currentHoldClickable = clickable;
            currentClickBehaviour = currentHoldClickable.GetBehaviour(mouseButtonID);
        }

        public void UnsetClickable()
        {
            currentHoldClickable = null;
            currentClickBehaviour = null;
        }

        public void InvokeClick(RaycastHit hit)
        {
            if (currentClickBehaviour != null)
                currentClickBehaviour.onClick.Invoke(hit);
        }

        public void InvokeOnMouseDown(RaycastHit hit)
        {
            if (currentClickBehaviour != null)
                currentClickBehaviour.onMouseDown.Invoke(hit);
        }

        public void InvokeStartHoldClick()
        {
            if (currentClickBehaviour != null)
                currentClickBehaviour.onStartHoldClick.Invoke();
        }

        public void InvokeCompleteHoldClick()
        {
            if (currentClickBehaviour != null)
                currentClickBehaviour.onCompleteHoldClick.Invoke();
        }

        public void InvokeCancelHoldClick()
        {
            if (currentClickBehaviour != null)
                currentClickBehaviour.onCancelHoldClick.Invoke();
        }

        public void InvokeProgressHoldClick()
        {
            if (currentClickBehaviour != null)
                currentClickBehaviour.onProgressHoldClick.Invoke(currentHoldProgress);
        }
    }
}
