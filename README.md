# FancyTextRendering
Render markdown & clickable links with TextMeshPro in Unity.

[![demo.png](_img/demo.png)](https://raw.githubusercontent.com/JimmyCushnie/FancyTextRendering/main/_img/demo.png)

[![inspector example.png](_img/inspector-example.png)](https://raw.githubusercontent.com/JimmyCushnie/FancyTextRendering/main/_img/inspector-example.png)

Try it yourself -- check out the [live demo!](https://jimmycushnie.itch.io/fancytextrendering-demo)

## What is this?

Unity's advanced text rendering library, [TextMeshPro](https://docs.unity3d.com/Manual/com.unity.textmeshpro.html), supports using [rich text tags](http://digitalnativestudios.com/textmeshpro/docs/rich-text/) to format the text. While rich text tags are powerful, they are an absolute pain in the ass to use. FancyTextRendering offers convenient conversion from [markdown](https://en.wikipedia.org/wiki/Markdown) to TMP Rich Text, which makes it much easier to write styled text in Unity.

| Markdown source                                          | Converted to rich text                                       | Looks like                                             |
| -------------------------------------------------------- | ------------------------------------------------------------ | ------------------------------------------------------ |
| `Text can be *italic*, or **bold**, or even ***both!***` | `Text can be <i>italic</i>, or <b>bold</b>, or even <i><b>both!</b></i>` | Text can be *italic*, or **bold**, or even ***both!*** |

Additionally, while TMP contains a [link tag](http://digitalnativestudios.com/textmeshpro/docs/rich-text/#link), this only provides metadata; TMP lacks any functions to make links actually clickable with an action that occurs when you click them. FancyTextRendering contains modules for rendering links properly: they are given a separate color from the rest of the text, then a second color when the cursor hovers over them, and a third color when the mouse button is held down on them. When the mouse button is released, the links can now execute arbitrary code, such as opening a URL in a browser.

Also, it's pretty darn fast, with minimal GC. On my machine it parses a 12,000 word markdown document in under 100ms.

## Project context

FancyTextRendering was written for our game [Logic World](https://logicworld.net/). It is primarily for our [in-game chat](https://www.youtube.com/watch?v=KE2E_pE5XBM&list=PLmwbsR--E7-anvM89nzzqTTUfyhGo2mkU), but it's also used for displaying various pages of information like the in-game changelog.

FancyTextRendering is part of LogicUI, the custom UI library we're writing for Logic World. The entire LogicUI library will be open sourced at some point; FancyTextRendering is just the first module to be released from it.

## Disclaimer

If you use this you should keep in mind that I don't actually know what the hell I'm doing. There are probably established coding conventions for how you're supposed to write a markdown parser, but I didn't look them up because I wanted the personal challenge of puzzling out how to do it myself. As such, there are many parts of markdown spec that are missing, and there are several bugs with the parser.

If you have a bug report or feature request, please put it on the [issues page](https://github.com/JimmyCushnie/FancyTextRendering/issues). And if you'd like to help out with development, I am absolutely accepting pull requests, so please go ahead and make one :D

## How to use

#### Rendering markdown:

1. Create a standard TextMeshPro GameObject
1. Add the MarkdownRenderer component
1. Type markdown in the MarkdownRenderer's `Source` textbox
1. Adjust the MarkdownRenderer's `Render Settings` properties to preference

#### Rendering links:

1. Start with a TextMeshPro GameObject (it can be the one used in the steps above)
1. Give the text some links, either directly using TMP's `<link>` tag or by using a MarkdownRenderer
1. Add the TextLinkHelper component
1. Adjust link color properties to preference
1. Optional: add the SimpleLinkBehavior component to make the links actually do something when you click on them

## Dependencies

* Odin Inspector (or [NaughtyAttributes](https://github.com/dbrizov/NaughtyAttributes)) can be used to draw the inspectors.

* [JimmysUnityUtilities](https://github.com/JimmyCushnie/JimmysUnityUtilities) is used for many things, but in particular the [StringBuilder extensions](https://github.com/JimmyCushnie/JimmysUnityUtilities/blob/master/Scripts/Extensions/Csharp%20types/StringBuilderExtensions.cs) are used extensively.

Both of these libraries are pre-installed if you simply clone the project, but you will need to add them yourself if you want to use FancyTextRendering in another project.

# Install package

## Via Package manager
Add from git URL:
`https://github.com/krisrok/FancyTextRendering.git?path=/Packages/FancyTextRendering`

and the dependency:
`https://github.com/JimmyCushnie/JimmysUnityUtilities.git`

## Or via manifest.json:
```
"dependencies": {
  "com.jimmycushnie.fancytextrendering": "https://github.com/krisrok/FancyTextRendering.git?path=/Packages/FancyTextRendering",
  "com.jimmycushnie.jimmysunityutilities": "https://github.com/JimmyCushnie/JimmysUnityUtilities.git",
  ...
```

# This fork's changes
1. [Package](https://github.com/krisrok/FancyTextRendering/tree/feature/packaging) the codebase
1. [Remove](https://github.com/krisrok/FancyTextRendering/tree/feature/odin-compat) NaughtyAttributes dependency and make it compatible with Odin Inspector as an alternative.
    * If you already have Odin installed, you do *not* have to install NaughtyAttributes.
    * If you don't have Odin, just install NaughtyAttributes (see [Dependencies](https://github.com/krisrok/FancyTextRendering/edit/main/README.md#Dependencies))
1. [Implement](https://github.com/krisrok/FancyTextRendering/tree/feature/itextpreprocessor) TextMeshPro's [ITextPreprocessor](https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/api/TMPro.ITextPreprocessor.html)
    * MarkdownRenderer just acts as an "addon" and does not need to be referenced in your code
    * You just write Markdown to [TMP_Text.text](https://docs.unity3d.com/Packages/com.unity.textmeshpro@3.0/api/TMPro.TMP_Text.html#TMPro_TMP_Text_text)
1. [Allow](https://github.com/krisrok/FancyTextRendering/tree/feature/leading-tags-poundsign) leading richtags before pound-sign-style headers.
    * This way headlines' underlines match the font color :)
    * `<color=red># My headline</color>`
