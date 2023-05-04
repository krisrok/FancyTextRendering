#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#else
using NaughtyAttributes;
#endif
using System;
using TMPro;
using UnityEngine;

namespace LogicUI.FancyTextRendering
{
    [ExecuteAlways]
    [RequireComponent(typeof(TMP_Text))]
    public class MarkdownRenderer : MonoBehaviour, ITextPreprocessor
    {
        [ContextMenu("Toggle debug")]
        private void ToggleDebug()
        {
            _showDebugText = !_showDebugText;

            if (_showDebugText)
                TextMesh.SetAllDirty();
        }

        private bool _showDebugText;

#if ODIN_INSPECTOR
        [NonSerialized, ShowInInspector] // NaughtyAttributes-compat requires serialized field, so we keep it public and set NonSerialized
#endif
        [ShowIf(nameof(_showDebugText))]
        [EnableIf(nameof(enabled))]
        [TextArea(minLines: 10, maxLines: 50)]
        public string DebugText;

        TMP_Text _TextMesh;
        public TMP_Text TextMesh
        {
            get
            {
                if (_TextMesh == null)
                    _TextMesh = GetComponent<TMP_Text>();

                return _TextMesh;
            }
        }

#if ODIN_INSPECTOR
        [InlineEditor]
        [OnValueChanged(nameof(OnValidate), IncludeChildren = true)]
#else
        [Expandable]
#endif
        [SerializeField]
        private MarkdownRenderingSettings _renderSettings;

        private void OnEnable()
        {
            TextMesh.textPreprocessor = this;
        }

        private void OnDisable()
        {
            TextMesh.textPreprocessor = null;
        }

        private void OnValidate()
        {
            TextMesh.SetAllDirty();

#if UNITY_EDITOR
            if (Application.isPlaying == true)
                return;

            TextMesh.ForceMeshUpdate();
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
        }

        public string PreprocessText(string text)
        {
            var result = Markdown.MarkdownToRichText(text, _renderSettings);
            
            if (_showDebugText)
                DebugText = result;

            return result;
        }
    }
}