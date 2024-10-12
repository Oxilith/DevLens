using Microsoft.AspNetCore.Components;

namespace Common.MarkdownUtils;

    public class MarkdownSection
    {
        private readonly List<MarkdownSection> _childSections = [];
        public IReadOnlyList<MarkdownSection> ChildSections => _childSections.AsReadOnly();

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownSection"/> class.
        /// </summary>
        /// <param name="title">The title of the markdown section.</param>
        /// <param name="markupString">The content of the markdown section.</param>
        /// <param name="parent">The parent section, if any.</param>
        public MarkdownSection(string title, MarkupString markupString, MarkdownSection? parent = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be null or empty.", nameof(title));

            Title = title;
            MarkupString = markupString;
            Parent = parent;
        }

        /// <summary>
        /// Gets the title of the markdown section.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets the content of the markdown section.
        /// </summary>
        public MarkupString MarkupString { get; private set; }

        /// <summary>
        /// Gets the parent markdown section, if any.
        /// </summary>
        public MarkdownSection? Parent { get; }

        /// <summary>
        /// Sets the content of the markdown section.
        /// </summary>
        /// <param name="markupString">The new content for the markdown section.</param>
        /// <returns>True if the markup string was successfully set; otherwise, false.</returns>
        public bool SetMarkupString(MarkupString markupString)
        {
            if (string.IsNullOrWhiteSpace(markupString.Value))
                return false;

            MarkupString = markupString;
            return true;
        }

        /// <summary>
        /// Adds a child section to this markdown section.
        /// </summary>
        /// <param name="childSection">The child section to add.</param>
        public void AddChildSection(MarkdownSection childSection)
        {
            if (childSection == null)
                throw new ArgumentNullException(nameof(childSection), "Child section cannot be null.");

            _childSections.Add(childSection);
        }
    }