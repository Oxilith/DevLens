using System.Text;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Microsoft.AspNetCore.Components;

namespace Common.MarkdownUtils;

public class MarkdownParser
{
     /// <summary>
    /// Parses the given markdown content into a tree of MarkdownSection objects.
    /// </summary>
    /// <param name="markdownContent">The markdown content to parse.</param>
    /// <param name="pipeline">The Markdown pipeline to use for parsing.</param>
    /// <returns>The root MarkdownSection representing the parsed content.</returns>
    public MarkdownSection ParseMarkdown(string markdownContent, MarkdownPipeline pipeline)
    {
        if (pipeline == null)
        {
            throw new ArgumentNullException(nameof(pipeline), "Pipeline cannot be null.");
        }

        var document = Markdown.Parse(markdownContent, pipeline);

        var rootSection = new MarkdownSection("Root", new MarkupString(string.Empty), null);

        using (var writer = new StringWriter())
        {
            var renderer = new HtmlRenderer(writer);
            renderer.Render(document);
            rootSection.SetMarkupString(new MarkupString(writer.ToString()));
        }

        ParseSections(document, rootSection);

        return rootSection;
    }

    /// <summary>
    /// Parses the MarkdownDocument into sections and adds them to the root section.
    /// </summary>
    /// <param name="document">The markdown document to parse.</param>
    /// <param name="rootSection">The root section to add parsed sections to.</param>
    private void ParseSections(MarkdownDocument document, MarkdownSection rootSection)
    {
        var sectionStack = new Stack<MarkdownSection>();
        sectionStack.Push(rootSection);

        foreach (var block in document)
        {
            if (block is HeadingBlock headingBlock)
            {
                var headingText = GetHeadingText(headingBlock);
                var headingLevel = headingBlock.Level;

                AdjustSectionStack(sectionStack, headingLevel);
                
                var newSection = new MarkdownSection(headingText, new MarkupString(string.Empty), sectionStack.Peek());

                var parentSection = newSection.Parent;
                parentSection?.AddChildSection(newSection);

                sectionStack.Push(newSection);
            }
            else
            {
                var currentSection = sectionStack.Peek();
                using (var writer = new StringWriter())
                {
                    var renderer = new HtmlRenderer(writer);
                    renderer.Render(block);
                    currentSection.SetMarkupString(new MarkupString(currentSection.MarkupString + writer.ToString()));
                }
            }
        }
    }

    /// <summary>
    /// Adjusts the section stack to find the correct parent for the current heading level.
    /// </summary>
    /// <param name="sectionStack">The stack of sections being processed.</param>
    /// <param name="headingLevel">The current heading level.</param>
    private void AdjustSectionStack(Stack<MarkdownSection> sectionStack, int headingLevel)
    {
        while (sectionStack.Count > 1 && sectionStack.Peek().Title != "Root" && headingLevel <= sectionStack.Count - 1)
        {
            sectionStack.Pop();
        }
    }

    /// <summary>
    /// Extracts the text from a HeadingBlock.
    /// </summary>
    /// <param name="headingBlock">The heading block to extract text from.</param>
    /// <returns>The extracted heading text.</returns>
    private string GetHeadingText(HeadingBlock headingBlock)
    {
        return headingBlock.Inline == null ? string.Empty : string.Concat(headingBlock.Inline.Select(x => x.ToString()));
    }

}
