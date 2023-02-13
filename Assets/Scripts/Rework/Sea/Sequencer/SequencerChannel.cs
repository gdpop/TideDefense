namespace TideDefense
{
    using UnityEngine;
    using System;

    public delegate void WashedUpObjectDelegate(WashedUpObject washedUpObject);
    public delegate void FloatingObjectDelegate(FloatingObject floatingObject);
    public delegate void MessageBottleDelegate(MessageBottleData data);

    [CreateAssetMenu(
        fileName = "FloatingSequencerChannel",
        menuName = "TideDefense/FloatingSequencerChannel",
        order = 0
    )]
    public class SequencerChannel : ScriptableObject
    {
        public WashedUpObjectDelegate onCreatedWashedUpObject;
        public FloatingObjectDelegate onCreateFloatingObject;
        public MessageBottleDelegate onCreateFloatingMessageBottle;
        public MessageBottleDelegate onCreateWashedUpMessageBottle;


        private void OnEnable()
        {

            onCreatedWashedUpObject = (WashedUpObject washedUpObject)=>{};
            onCreateFloatingObject = (FloatingObject floatingObject) => { };

            onCreateFloatingMessageBottle = (MessageBottleData data) => { };
            onCreateWashedUpMessageBottle = (MessageBottleData data) => { };
        }

    }
}
