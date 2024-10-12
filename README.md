# Welcome to DevLens

DevLens is (going to be :D) a powerful, developer-centric tool designed to analyze project health and provide insightful metrics on code quality, dependencies, coupling, and complexity. Its goal is to help developers gain visibility into areas of the codebase that need attention, making it easier to identify problematic parts of a project and take proactive steps to improve quality and maintainability.

## Deployed Application

You can access the live version of the application here:

[![Deployed App](https://img.shields.io/badge/Visit-Live%20App-blue)](https://devlens-test.azurewebsites.net/) [![Build and deploy DevLens Test](https://github.com/Oxilith/DevLens/actions/workflows/test_devlens.yml/badge.svg)](https://github.com/Oxilith/DevLens/actions/workflows/test_devlens.yml)

## Key Features

### Git Commit Analysis - In progress

DevLens provides insights based on Git commits to help track changes over time, enabling developers to understand the evolution of the codebase. Key features include:

- **Commit Frequency**: Identify active areas of the project and understand the historical activity.
- **Class Changes**: Track which classes are frequently modified, allowing for early detection of areas prone to bugs.

### Dependency Analysis - TODO

DevLens identifies dependencies within your project to reveal:

- **Tight Coupling**: Discover areas where classes or modules are tightly coupled, making future changes risky.

### Coupling and Complexity Assessment - TODO

- **Code Complexity**: Analyze the complexity of your code to identify areas that might need refactoring. Cyclomatic complexity metrics highlight the most complex methods or classes.
- **Coupling Metrics**: Show dependencies and coupling between components, allowing developers to focus on decoupling for a more maintainable architecture.

### Hotspot Detection - TODO

**DevLens highlights "hotspots"**—areas that have high complexity and frequent changes. This helps teams focus on stabilizing critical parts of the code and ensuring these areas receive extra attention during reviews.

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

Add the following configuration to the `appsettings.json` file to specify how DevLens should operate:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "RepositorySettings": {
    "UseLocalRepository": "<PLACEHOLDER: Use local repository, true/false> Required",
    "LocalPath": null,
    "RemoteRepositoryUri": null
  },
  "ChangeDataService": {
    "NumberOfMonthsForAverageCalculation": "<PLACEHOLDER: Number of months for calculating the average changes>",
    "MinimumMonthlyChanges": "<PLACEHOLDER: Minimum number of monthly changes to include in analysis>"
  },
  "FeatureToggles": {
    "EnableAppInsightsDependencyAnalysis": "<PLACEHOLDER: Enable or disable dependency analysis, true/false>"
  },
  "EnvBaseAddress": "https://localhost:44377/"
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

- **Common Layer**: Provides shared utilities and extension methods used across all layers, including value objects, enums, and extension classes.

- **Infrastructure Layer**: Handles interactions with external services such as Git repositories. This layer also implements repository patterns for accessing commit data and configuration classes like `RepositorySettings`.

- **Application Layer**: This layer is responsible for orchestrating complex tasks and business processes. It contains components like services, factories, processors, filters, and strategies to manage complex operations. It connects the domain with the infrastructure to fulfill specific use cases.

    - **Services**: Coordinate workflows and apply business logic.
    - **Processors**: Execute detailed operations, such as analyzing data or transforming input.
    - **Factories**: Create complex objects or aggregate instances.
    - **Filters**: Apply specific conditions or rules to data.
    - **Strategies**: Provide various implementations of particular operations to enhance flexibility.  
  
- **Presentation Layer**: Provides interactive Razor Components and server components for developers to view and interact with DevLens insights, making it easier to visualize code health and analyze the metrics provided by other layers.

## Contributing

We welcome contributions! To get started:

1. Fork the repository.
2. Create a new feature branch.
3. Make your changes.
4. Open a pull request.

Please ensure your code follows best practices and is well-documented.

## License

This project is licensed under the MIT License. See the LICENSE file for more details.

## Contact

For questions or feedback, please contact the author at [konrad.jagusiak@gmail.com](mailto\:konrad.jagusiak@gmail.com).

## Author

This project was developed by Konrad Jagusiak. Contributions from the open-source community are welcome under the MIT License.

## Acknowledgments

- Thanks to all contributors for their efforts in building DevLens.
- Built using .NET 8, LibGit2Sharp, Bootstrap, Chart.js, and Blazor for the front-end interface.
