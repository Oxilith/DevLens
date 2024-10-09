# DevLens

DevLens is a powerful, developer-centric tool designed to analyze project health and provide insightful metrics on code quality, dependencies, coupling, and complexity. Its goal is to help developers gain visibility into areas of the codebase that need attention, making it easier to identify problematic parts of a project and take proactive steps to improve quality and maintainability.

## Features

### 1. Git Commit Analysis - Initial version

DevLens provides insights based on Git commits to help track changes over time. It helps developers understand the evolution of the codebase, including:

- **Commit Frequency**: Identify active areas of the project and understand historical activity.
- **Class Changes**: Track which classes are frequently modified, allowing for early detection of areas prone to bugs.

### 2. Dependency Analysis - TODO

DevLens identifies dependencies within your project to reveal:

- **Tight Coupling**: Discover areas where classes or modules are tightly coupled, making future changes risky.

### 3. Coupling and Complexity Assessment - TODO

- **Code Complexity**: Analyze the complexity of your code to identify areas that might need refactoring. Cyclomatic complexity metrics are used to highlight the most complex methods or classes.
- **Coupling Metrics**: Show dependencies and coupling between components, allowing developers to focus on decoupling to achieve a more maintainable architecture.

### 4. Hotspot Detection - TODO

DevLens highlights "hotspots"—areas that have high complexity and frequent changes. This helps teams focus on stabilizing critical parts of the code and ensuring those areas receive extra attention in reviews.

### 5. Developer Insights Dashboard - TODO

DevLens provides an interactive dashboard that consolidates all these insights into one place:

- **Visualize Complexity**: Charts and graphs make it easy to understand the health of different parts of the project.
- **Actionable Recommendations**: Suggests refactoring opportunities and highlights risky code areas to prioritize.

## Getting Started

### Prerequisites

- .NET 8 or later
- Git installed for repository analysis

### Installation


### Configuration

Add a `repositoryPath` to the `appsettings.json` file to specify the Git repository that DevLens will analyze:

```json
{
  "RepositorySettings": {
    "Path": "C:\\path\\to\\your\\repository"
  }
}
```

### Running the Application

To run the application:

```sh
   dotnet run
```

Once started, open a browser and navigate to `http://localhost:44377` to interact with the DevLens dashboard.

## Architecture

DevLens follows a clean, domain-driven architecture with the following layers:

- **Domain Layer**: Contains the core business logic, including entities like `ClassChange`, `Commit`, and `ProjectChange`. The domain models encapsulate the key rules and data structures for Git commit analysis and project health assessment.

- **Application Layer**: Responsible for orchestrating tasks and handling complex data processing. The `ChangeDataProcessor` lives in this layer to perform operations like ordering, limiting data, and generating default data models.

- **Infrastructure Layer**: Handles interactions with external services like Git repositories. Implements repository patterns for accessing commit data.

- **Presentation Layer**: Provides the interactive Razor Components and server components for developers to view and interact with DevLens insights.

## Contributing

We welcome contributions! To get started:

1. Fork the repository.
2. Create a new feature branch.
3. Make your changes.
4. Open a pull request.

Please ensure your code follows best practices and is well-documented.

## License

MIT

## Contact

For questions or feedback, please reach out to the author at [[konrad.jagusiak@dnv.com]](mailto\:konrad.jagusiak@dnv.com)].

## Author

This project was developed by **Konrad Jagusiak**.

## Acknowledgements

- Thanks to all contributors for their efforts in building DevLens.
- Built using .NET 8, LibGit2Sharp, Chart.js and Blazor and Bootstrap for the user interface.
