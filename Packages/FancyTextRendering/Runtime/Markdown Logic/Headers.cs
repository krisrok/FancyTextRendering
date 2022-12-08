using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using JimmysUnityUtilities;

using HeaderCase = LogicUI.FancyTextRendering.MarkdownRenderingSettings.HeaderSettings.HeaderData.HeaderCase;

namespace LogicUI.FancyTextRendering.MarkdownLogic
{
    class Headers : MarkdownLineProcessorBase
    {
        private static Regex _leadingRichTagsRegex = new Regex(@"\w*(?<!#.*)<.*?>(?>#)", RegexOptions.Compiled);

        protected override void ProcessInternal(IReadOnlyList<MarkdownLine> lines, MarkdownRenderingSettings settings)
        {
            bool inWakeOfHeader = false; // Used to delete empty lines after headers
            for (int i = 0; i < lines.Count; i++)
            {
                MarkdownLine line = lines[i];
                StringBuilder builder = line.Builder;

                if (line.DisableFutureProcessing)
                    continue;

                if (inWakeOfHeader)
                {
                    if (line.Builder.IsEmpty())
                    {
                        line.DeleteLineAfterProcessing = true;
                        continue;
                    }
                    else
                    {
                        inWakeOfHeader = false;
                    }
                }


                if (settings.Headers.RenderPoundSignHeaders)
                {
                    var offset = 0;
                    var match = _leadingRichTagsRegex.Match(line.Builder.ToString());
                    if (match.Success)
                        offset = match.Index + match.Length - 1;

                    int leadingNumberSignsCount = 0;
                    while (offset + leadingNumberSignsCount < builder.Length && builder[offset + leadingNumberSignsCount] == '#')
                        leadingNumberSignsCount++;

                    // Require a space between the # and the header contents. This is different from most markdown implementations,
                    // but it means you can type #relatable and it will actually be a hashtag and not a big header
                    if (builder.Length > offset + leadingNumberSignsCount && builder[offset + leadingNumberSignsCount] == ' ')
                    {
                        if (CanMakeLineHeader(line, leadingNumberSignsCount))
                        {
                            builder.Remove(startIndex: offset, length: leadingNumberSignsCount);
                            MakeLineHeader(line, leadingNumberSignsCount, offset);
                            continue;
                        }
                    }
                }

                if (settings.Headers.RenderLineHeaders)
                {
                    if (i > 0) // Line headers can't apply to the first line
                    {
                        int targetHeaderLevel = -1;

                        if (builder.IsExclusively('='))
                            targetHeaderLevel = 1;
                        else if (builder.IsExclusively('-'))
                            targetHeaderLevel = 2;

                        if (CanMakeLineHeader(lines[i - 1], targetHeaderLevel))
                        {
                            MakeLineHeader(lines[i - 1], targetHeaderLevel);
                            line.DeleteLineAfterProcessing = true;
                        }
                    }
                }


                bool CanMakeLineHeader(MarkdownLine line_, int headerLevel)
                {
                    if (line_.DisableFutureProcessing)
                        return false;

                    if (headerLevel < 1 || headerLevel > settings.Headers.Levels.Length)
                        return false;

                    return true;
                }

                void MakeLineHeader(MarkdownLine line_, int headerLevel, int offset = 0)
                {
                    inWakeOfHeader = true;

                    var builder_ = line_.Builder;
                    var info = settings.Headers.Levels[headerLevel - 1];

                    builder_.Remove(offset, 1);

                    builder_.InsertChain(offset, "<size=", info.Size.ToString(), "em>");
                    builder_.Append("</size>");

                    if (info.Bold)
                        builder_.Insert(offset, "<b>").Append("</b>");

                    if (info.Underline)
                        builder_.Insert(offset, "<u>").Append("</u>");

                    switch (info.Case)
                    {
                        default:
                        case HeaderCase.None:
                            break;

                        case HeaderCase.Uppercase:
                            builder_.Prepend("<uppercase>").Append("</uppercase>");
                            break;

                        case HeaderCase.Smallcaps:
                            builder_.Prepend("<smallcaps>").Append("</smallcaps>");
                            break;

                        case HeaderCase.Lowercase:
                            builder_.Prepend("<lowercase>").Append("</lowercase>");
                            break;
                    }

                    line_.AddVerticalWhitespaceAfter(info.VerticalSpacing);
                }
            }
        }

        protected override bool AllowedToProces(MarkdownRenderingSettings settings)
            => settings.Headers.RenderPoundSignHeaders || settings.Headers.RenderLineHeaders;
    }
}