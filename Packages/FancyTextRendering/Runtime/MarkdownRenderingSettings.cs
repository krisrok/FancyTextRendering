using System;
using UnityEngine;
#if ODIN_INSPECTOR_3
using Sirenix.OdinInspector;
#else
using NaughtyAttributes;
#endif

namespace LogicUI.FancyTextRendering
{
#if ODIN_INSPECTOR_3
    [AttributeUsage(AttributeTargets.All)]
    internal class AllowNestingAttribute : Attribute { }
#endif

    [Serializable]
    public class MarkdownRenderingSettings
    {
        public static MarkdownRenderingSettings Default => new MarkdownRenderingSettings();


        public BoldSettings Bold = new BoldSettings();
        [Serializable] public class BoldSettings
        {
            public bool RenderBold = true;
        }

        public ItalicSettings Italics = new ItalicSettings();
        [Serializable] public class ItalicSettings
        {
            public bool RenderItalics = true;
        }

        public StrikethroughSettings Strikethrough = new StrikethroughSettings();
        [Serializable] public class StrikethroughSettings
        {
            public bool RenderStrikethrough = true;
        }

        public MonospaceSettings Monospace = new MonospaceSettings();
        [Serializable] public class MonospaceSettings
        {
            public bool RenderMonospace = true;

            private bool Condition1 => RenderMonospace && UseCustomFont;
            private bool Condition2 => RenderMonospace && DrawOverlay;//EConditionOperator.And, nameof(RenderMonospace), nameof(DrawOverlay)

            private bool Condition3 => RenderMonospace && ManuallySetCharacterSpacing;//EConditionOperator.And, nameof(RenderMonospace), nameof(ManuallySetCharacterSpacing)
            private bool Condition4 => RenderMonospace && AddSeparationSpacing; // EConditionOperator.And, nameof(RenderMonospace), nameof(AddSeparationSpacing)

            [Space]
            [ShowIf(nameof(RenderMonospace)), AllowNesting]
            public bool UseCustomFont = true;

            [ShowIf(nameof(Condition1)), AllowNesting]
            public string FontAssetPathRelativeToResources = "Noto/Noto Mono/NotoMono-Regular";

            [Space]
            [ShowIf(nameof(RenderMonospace)), AllowNesting]
            public bool DrawOverlay = true;

            [ShowIf(nameof(Condition2)), AllowNesting]
            public Color OverlayColor = new Color32(0, 0, 0, 60);

            [ShowIf(nameof(Condition2)), AllowNesting]
            public float OverlayPaddingPixels = 25;

            [Space]
            [ShowIf(nameof(RenderMonospace)), AllowNesting]
            public bool ManuallySetCharacterSpacing = false;

            [ShowIf(nameof(Condition3)), AllowNesting]
            public float CharacterSpacing = 0.69f;

            [Space]
            [ShowIf(nameof(RenderMonospace)), AllowNesting]
            public bool AddSeparationSpacing = true;

            [ShowIf(nameof(Condition4)), AllowNesting]
            public float SeparationSpacing = 0.3f;
        }


        public ListSettings Lists = new ListSettings();
        [Serializable] public class ListSettings
        {
            private bool Condition1 => RenderUnorderedLists || RenderOrderedLists; //EConditionOperator.Or, nameof(RenderUnorderedLists), nameof(RenderOrderedLists)

            public bool RenderUnorderedLists = true;
            public bool RenderOrderedLists = true;

            [Space]
            [ShowIf(nameof(RenderUnorderedLists)), AllowNesting]
            public string UnorderedListBullet = "•";

            [ShowIf(nameof(RenderOrderedLists)), AllowNesting]
            public string OrderedListNumberSuffix = ".";

            [Space]
            [ShowIf(nameof(Condition1)), AllowNesting]
            public float VerticalOffset = 0.76f;

            [ShowIf(nameof(Condition1)), AllowNesting]
            public float BulletOffsetPixels = 100f;

            [ShowIf(nameof(Condition1)), AllowNesting]
            public float ContentSeparationPixels = 20f;
        }

        public LinkSettings Links = new LinkSettings();
        [Serializable] public class LinkSettings
        {
            private bool Condition1 => RenderLinks || RenderAutoLinks; //EConditionOperator.Or, nameof(RenderLinks), nameof(RenderAutoLinks)

            public bool RenderLinks = true;
            public bool RenderAutoLinks = true;

            [ShowIf(nameof(Condition1)), AllowNesting]
            [ColorUsage(showAlpha: false)]
            public Color LinkColor = new Color32(29, 124, 234, a: byte.MaxValue);
        }

        public HeaderSettings Headers = new HeaderSettings();
        [Serializable] public class HeaderSettings
        {
            public bool RenderPoundSignHeaders = true;
            public bool RenderLineHeaders = true;

            [Space]
            // [ShowIf(nameof(RenderHeaders)), AllowNesting]
            // Can't use ShowIf here yet -- https://github.com/dbrizov/NaughtyAttributes/issues/142
            public HeaderData[] Levels = new HeaderData[]
            {
                new HeaderData(2f, true, true, 0.45f),
                new HeaderData(1.7f, true, true, 0.3f),
                new HeaderData(1.5f, true, false),
                new HeaderData(1.3f, true, false),
            };


            [Serializable]
            public class HeaderData
            {
                public float Size;
                public bool Bold;
                public bool Underline;
                public HeaderCase Case = HeaderCase.None;
                public float VerticalSpacing;


                public HeaderData() { } // Needs a default constructor so it can be deserialized by SUCC
                public HeaderData(float size, bool bold, bool underline, float verticalSpacing = 0)
                {
                    Size = size;
                    Bold = bold;
                    Underline = underline;
                    VerticalSpacing = verticalSpacing;
                }


                public enum HeaderCase
                {
                    None = 0,
                    Uppercase,
                    Smallcaps,
                    Lowercase
                }
            }
        }
        
        public SuperscriptSettings Superscript = new SuperscriptSettings();
        [Serializable] public class SuperscriptSettings
        {
            public bool RenderSuperscript = false;

            [ShowIf(nameof(RenderSuperscript))]
            [AllowNesting]
            public bool RenderChainSuperscript = true;
        }
    }
}