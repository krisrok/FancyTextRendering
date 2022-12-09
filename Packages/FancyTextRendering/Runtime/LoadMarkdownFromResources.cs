using JimmysUnityUtilities;
using UnityEngine;
using TMPro;
#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#else
using NaughtyAttributes;
#endif

namespace LogicUI.FancyTextRendering
{
    [RequireComponent(typeof(TMP_Text))]
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
            GetComponent<TMP_Text>().SetText(markdown);
        }
    }
}