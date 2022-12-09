using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using LogicUI.FancyTextRendering;
#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#else
using NaughtyAttributes;
#endif

namespace FancyTextRendering.Demo
{
    public class DemoRenderUpdater : MonoBehaviour
    {
        [SerializeField] TMP_InputField MarkdownSourceInputField;
        [SerializeField] TMP_Text MarkdownRenderer;

        private void Start()
        {
            MarkdownSourceInputField.onValueChanged.AddListener(_ => UpdateRender());
        }

        [Button]
        private void UpdateRender()
        {
            MarkdownRenderer.SetText(MarkdownSourceInputField.text);
        }
    }
}
