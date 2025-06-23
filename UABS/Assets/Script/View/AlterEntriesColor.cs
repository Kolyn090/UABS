using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

namespace UABS.Assets.Script.View
{
    public class AlterEntriesColor : MonoBehaviour
    {
        [SerializeField]
        private Color _alternateColor;
        [SerializeField]
        private GameObject _context;

        private void Start()
        {
            AlterChildrenColorIfImage();
        }

        private void AlterChildrenColorIfImage()
        {
            int counter = 0;
            foreach (Transform child in _context.transform)
            {
                if (child.TryGetComponent<UnityEngine.UI.Image>(out var image))
                {
                    if (counter % 2 != 0)
                    {
                        image.color = _alternateColor;
                    }
                    counter++;
                }
            }
        }
    }
}