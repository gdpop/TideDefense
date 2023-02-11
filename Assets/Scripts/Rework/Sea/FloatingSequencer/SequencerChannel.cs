namespace TideDefense
{
    using UnityEngine;
    using System;

    public delegate void WashedUpObjectDelegate(WashedUpObject washedUpObject);
    public delegate void FloatingObjectDelegate(FloatingObject floatingObject);
    public delegate void FloatingMessageBottleDelegate(MessageBottleData data);

    [CreateAssetMenu(
        fileName = "FloatingSequencerChannel",
        menuName = "TideDefense/FloatingSequencerChannel",
        order = 0
    )]
    public class SequencerChannel : ScriptableObject
    {
        public WashedUpObjectDelegate onCreatedWashedUpObject;
        public FloatingObjectDelegate onCreateFloatingObject;
        public FloatingMessageBottleDelegate onCreateMessageBottle;


        private void OnEnable()
        {

            onCreatedWashedUpObject = (WashedUpObject washedUpObject)=>{};
            onCreateFloatingObject = (FloatingObject floatingObject) => { };

            onCreateMessageBottle = (MessageBottleData data) => { };
        }

    }
}
