using UnityEngine;
using System.Collections.Generic;

namespace TideDefense
{
    [CreateAssetMenu(
        fileName = "MessageBottleSettings",
        menuName = "TideDefense/MessageBottleSettings",
        order = 0
    )]
    public class MessageBottleSettings : ScriptableObject
    {
        [SerializeField]
        private FloatingMessageBottle narrationMessageBottle = null;

        [SerializeField]
        private FloatingMessageBottle tutorialMessageBottle = null;

        [Header("Narration")]
        public float minDelayMessageNarration = 20;

        public float maxDelayMessageNarration = 40;

        public List<MessageBottleData> narrationMessageBottleDatas = new List<MessageBottleData>();

        public FloatingMessageBottle PrefabFromType(MessageBottleType type)
        {
            switch (type)
            {
                case MessageBottleType.Narration:
                    return narrationMessageBottle;
                case MessageBottleType.Tutorial:
                    return tutorialMessageBottle;
                default:
                    return null;
            }
        }
    }
}
