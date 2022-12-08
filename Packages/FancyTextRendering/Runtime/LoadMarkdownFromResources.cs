using JimmysUnityUtilities;
using UnityEngine;
#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#else
using NaughtyAttributes;
#endif

namespace LogicUI.FancyTextRendering
{
    [RequireComponent(typeof(MarkdownRenderer))]
    public class LoadMarkdownFromResources : MonoBehaviour
    {
        [SerializeField] string MarkdownResourcesPath;

        private void Awake()
        {
            LoadMarkdown();
        }

        [Button]
        private void LoadMarkdown()
        {
            string markdown = ResourcesUtilities.ReadTextFromFile(MarkdownResourcesPath);
            GetComponent<MarkdownRenderer>().Source = markdown;
        }
    }
}