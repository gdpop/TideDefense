using CodesmithWorkshop.Useful;
using UnityEngine;

namespace PierreMizzi.MouseInteractable
{
    public class HoldClickSetting
    {
        // Settings
        public int mouseButtonID = -1;
        public float clickHoldTreshold = 1f;
        public float clickHoldDuration = 2f;

        // Runtime
        public HoldClickable currentHoldClickable = null;

        public float currentHoldTime = 0f;

        public float currentHoldProgress
        {
            get { return GetHoldProgressFromHoldTime(currentHoldTime); }
        }

        public HoldClickSetting(int mouseButtonID, float treshold, float duration)
        {
            this.mouseButtonID = mouseButtonID;
            this.clickHoldTreshold = treshold;
            this.clickHoldDuration = duration;
        }

        public HoldClickStatus currentStatus
        {
            get { return GetClickStatusFromClickTime(currentHoldTime); }
        }

        public HoldClickStatus GetClickStatusFromClickTime(float time)
        {
            if (0 <= currentHoldTime && currentHoldTime < clickHoldTreshold)
                return HoldClickStatus.inTreshold;
            else if (clickHoldTreshold <= currentHoldTime && currentHoldTime < clickHoldDuration)
                return HoldClickStatus.inLong;
            else if (clickHoldDuration < currentHoldTime)
                return HoldClickStatus.completed;
            else
                return HoldClickStatus.None;
        }

        private float GetHoldProgressFromHoldTime(float time)
        {
            return Mathf.Clamp(
                UtilsClass.Remap(time, clickHoldTreshold, clickHoldDuration, 0f, 1f),
                0f,
                1f
            );
        }

        public void InvokeStartHoldClick()
        {
            if (currentHoldClickable != null)
                currentHoldClickable.GetBehaviour(mouseButtonID).OnStartHoldClick.Invoke();
        }

        public void InvokeCompleteHoldClick()
        {
            if (currentHoldClickable != null)
                currentHoldClickable.GetBehaviour(mouseButtonID).OnCompleteHoldClick.Invoke();
        }

        public void InvokeCancelHoldClick()
        {
            if (currentHoldClickable != null)
                currentHoldClickable.GetBehaviour(mouseButtonID).OnCancelHoldClick.Invoke();
        }

        public void InvokeProgressHoldClick()
        {
            if (currentHoldClickable != null)
                currentHoldClickable
                    .GetBehaviour(mouseButtonID)
                    .OnProgressHoldClick.Invoke(currentHoldProgress);
        }
    }
}
