# DevLens

DevLens is (going to be :D) a powerful, developer-centric tool designed to analyze project health and provide insightful metrics on code quality, dependencies, coupling, and complexity. Its goal is to help developers gain visibility into areas of the codebase that need attention, making it easier to identify problematic parts of a project and take proactive steps to improve quality and maintainability.

## Deployed Application

You can access the live version of the application here:

[![Deployed App](https://img.shields.io/badge/Visit-Live%20App-blue)](https://devlens.azurewebsites.net/) [![GitHub Release](https://github.com/Oxilith/DevLens/actions/workflows/development_devlens.yml/badge.svg?branch=development)](https://github.com/Oxilith/DevLens/actions/workflows/development_devlens.yml)

## Key Features

### Git Commit Analysis - In progress

Devens provides insights based on Git commits to help track changes over time, enabling developers to understand the evolution of the codebase. Key features include:

- **Commit Frequency**: Identify active areas of the project and understand the historical activity.
- **Class Changes**: Track which classes are frequently modified, allowing for early detection of areas prone to bugs.

### Dependency Analysis - TODO

DevLens identifies dependencies within your project to reveal:

- **Tight Coupling**: Discover areas where classes or modules are tightly coupled, making future changes risky.
- **External Dependencies**: Monitor and track third-party dependencies that could introduce issues such as security vulnerabilities or maintenance concerns.

### Coupling and Complexity Assessment- TODO

- **Code Complexity**: Analyze the complexity of your code to identify areas that might need refactoring. Cyclomatic complexity metrics highlight the most complex methods or classes.
- **Coupling Metrics**: Show dependencies and coupling between components, allowing developers to focus on decoupling for a more maintainable architecture.

### Hotspot Detection - TODO

DevLens highlights "hotspots"—areas that have high complexity and frequent changes. This helps teams focus on stabilizing critical parts of the code and ensuring these areas receive extra attention during reviews.

### Developer Insights Dashboard - TODO

DevLens provides an interactive dashboard that consolidates all these insights into one place:

- **Visualize Complexity**: Charts and graphs make it easy to understand the health of different parts of the project.
- **Actionable Recommendations**: Suggests refactoring opportunities and highlights risky code areas to prioritize.

## Getting Started

### Prerequisites

- .NET 8 or later
- Git installed for repository analysis

### Installation

1. Clone the repository:
   ```sh
   git clone https://github.com/Oxilith/DevLens.git
   ```
2. Navigate to the project directory:
   ```sh
   cd DevLens
   ```
3. Install the required dependencies:
   ```sh
   dotnet restore
   ```

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

- **Application Layer**: This layer orchestrates tasks and processes complex data. The `ChangeDataProcessor` resides in this layer and performs operations such as ordering, limiting data, and generating default data models.

- **Infrastructure Layer**: This layer handles interactions with external services like Git repositories and implements repository patterns for accessing commit data.

- **Presentation Layer**: Provides interactive Razor Components and server components for developers to view and interact with DevLens insights.

## Contributing

We welcome contributions! To get started:

1. Fork the repository.
2. Create a new feature branch.
3. Make your changes.
4. Open a pull request.

Please ensure your code follows best practices and is well-documented.

## License

This project is proprietary and intended solely for use within our company. Unauthorized distribution, modification, or use outside of the organization is strictly prohibited.

## Contact

For questions or feedback, please contact the author at [konrad.jagusiak@gmail.com](mailto\:konrad.jagusiak@gmail.com).

## Author

This project was developed by Konrad Jagusiak.

## Acknowledgments

- Thanks to all contributors for their efforts in building DevLens.
- Built using .NET 8, LibGit2Sharp, Bootstrap, and Chart.js for the user interface.
