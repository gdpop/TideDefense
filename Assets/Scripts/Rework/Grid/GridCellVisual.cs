namespace TideDefense
{
    using UnityEngine;

    public class GridCellVisual : MonoBehaviour
    {
        public GameObject _diggableHintSprite = null;
        public GameObject _buildableHintSprite = null;

        public void DisplayDiggableHints()
        {
            _diggableHintSprite.SetActive(true);
        }

        public void HideDiggableHints()
        {
            _diggableHintSprite.SetActive(false);
        }

        public void DisplayBuildableHints()
        {
            _buildableHintSprite.SetActive(true);
        }

        public void HideBuildableHints()
        {
            _buildableHintSprite.SetActive(false);
        }
    }
}
