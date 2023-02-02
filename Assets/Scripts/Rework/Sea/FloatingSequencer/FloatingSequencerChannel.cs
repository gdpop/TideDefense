namespace TideDefense
{
    using UnityEngine;

    public delegate void MessageBottleDelegate(MessageBottleData data);
    public delegate void BeachToolDelegate(BeachTool tool);

    [CreateAssetMenu(
        fileName = "FloatingSequencerChannel",
        menuName = "TideDefense/FloatingSequencerChannel",
        order = 0
    )]
    public class FloatingSequencerChannel : ScriptableObject
    {
        public MessageBottleDelegate onCreateMessageBottle;
        public BeachToolDelegate onCreateBeachTool;



        private void OnEnable()
        {
            onCreateMessageBottle = (MessageBottleData data) => { };
            onCreateBeachTool = (BeachTool tool) => { };
        }

    }
}
