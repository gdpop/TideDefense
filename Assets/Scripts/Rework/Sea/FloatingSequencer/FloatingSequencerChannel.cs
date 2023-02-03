namespace TideDefense
{
    using UnityEngine;

    public delegate void FloatingMessageBottleDelegate(MessageBottleData data);
    public delegate void FloatingBeachToolDelegate(BeachTool tool);
    public delegate void WashedUpObjectDelegate(WashedUpObject washedUp);

    [CreateAssetMenu(
        fileName = "FloatingSequencerChannel",
        menuName = "TideDefense/FloatingSequencerChannel",
        order = 0
    )]
    public class FloatingSequencerChannel : ScriptableObject
    {
        public FloatingMessageBottleDelegate onCreateMessageBottle;
        public FloatingBeachToolDelegate onCreateBeachTool;
        public WashedUpObjectDelegate onCreatedWashedUpObject;


        private void OnEnable()
        {
            onCreateMessageBottle = (MessageBottleData data) => { };
            onCreateBeachTool = (BeachTool tool) => { };

            onCreatedWashedUpObject =(WashedUpObject value)=>{};
        }

    }
}
