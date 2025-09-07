# 🎮 GamePod

![License](https://img.shields.io/badge/license-GPL%20v3-blue.svg)
![.NET](https://img.shields.io/badge/.NET-7.0-purple.svg)
![Platform](https://img.shields.io/badge/platform-Windows%2010%2F11-lightgrey.svg)
![Docker](https://img.shields.io/badge/docker-required-blue.svg)

![Icon](GamePod/GamePod/Assets/../../Assets/StoreLogo.scale-200.png)

**GamePod** is a powerful Windows desktop application that revolutionizes game development by providing containerized development environments. Built on Docker Engine, GamePod creates and manages Linux containers specifically optimized for game development workflows, supporting multiple game engines and development tools.

[![Demo Video](./assets/gamepods.png)](https://www.youtube.com/watch?v=hBs6m7Ih4io)

---

## 📋 Table of Contents

- [🎮 GamePod](#-gamepod)
  - [📋 Table of Contents](#-table-of-contents)
  - [✨ Features](#-features)
  - [🎯 Supported Game Engines](#-supported-game-engines)
  - [📸 Screenshots](#-screenshots)
  - [⚡ Prerequisites](#-prerequisites)
  - [🚀 Installation](#-installation)
  - [💻 Usage](#-usage)
  - [🏗️ Development Setup](#️-development-setup)
  - [🔧 Building from Source](#-building-from-source)
  - [🏛️ Architecture](#️-architecture)
  - [🤝 Contributing](#-contributing)
  - [🐛 Troubleshooting](#-troubleshooting)
  - [📖 Documentation](#-documentation)
  - [🔮 Roadmap](#-roadmap)
  - [💬 Support & Community](#-support--community)
  - [📚 Resources and Credits](#-resources-and-credits)
  - [📄 License](#-license)

---

## ✨ Features

- 🐳 **Containerized Development**: Create isolated, reproducible development environments for game projects
- 🎮 **Multi-Engine Support**: Built-in support for popular game engines including Godot, Unity, Pygame, and Enigma
- 🖥️ **Graphical Applications**: Run GUI applications seamlessly with WSLg integration
- ⚙️ **Easy Management**: Intuitive interface to start, stop, restart, and manage containers
- 📁 **Project Mounting**: Mount local project folders directly into containers for seamless development
- 🔧 **Customizable Settings**: Configure port bindings, volume mounts, and performance settings per container
- 📦 **One-Click Setup**: Automated container creation with pre-configured development environments
- 🔄 **Container Updates**: Check for and apply updates to container images
- 📋 **Project Templates**: Quick setup for new projects with engine-specific configurations

## 🎯 Supported Game Engines

| Engine | Version Support | GUI Support | Status |
|--------|-----------------|-------------|--------|
| 🎯 [Godot Engine](https://godotengine.org/) | Latest | ✅ Full Editor | ✅ Active |
| 🐍 [Pygame](https://www.pygame.org/) | Latest | ✅ Graphics | ✅ Active |
| 🎮 Unity | Hub + Editor | ⚠️ CLI Only | ✅ Active |
| 🎨 [Enigma](https://enigma-dev.org/) | Latest | ✅ Full Editor | ✅ Active |
| 🎯 More engines | - | - | 🔜 Coming Soon |

*GUI applications require WSLg for seamless Windows integration*

## 📸 Screenshots

### Main Dashboard
The main window displays all your created containers with easy management options:

![Main Window](assets/homepage.png)

### Container Creation
Simple workflow to create new development environments:

![Create Page](assets/createpage.png)

## ⚡ Prerequisites

### System Requirements
- **Operating System**: Windows 10 version 1903+ or Windows 11
- **Architecture**: x86, x64, or ARM64
- **Memory**: 8GB RAM minimum (16GB recommended for multiple containers)
- **Storage**: 20GB free space minimum
- **Virtualization**: Hardware virtualization support enabled in BIOS/UEFI

### Required Software
- **Docker Engine**: Latest version with Linux containers support
- **WSL 2**: Windows Subsystem for Linux version 2
- **WSLg**: For graphical application support (included in recent WSL updates)

### Optional Components
- **Visual Studio 2022**: For development and debugging (Community edition or higher)
- **Git**: For version control integration

## 🚀 Installation

### Quick Install

1. **Download** the latest release from the [Releases page](https://github.com/GiorCocc/GamePod/releases)
2. **Run** the installer (`.msix` package)
3. **Follow** the installation wizard

### Prerequisites Setup

If you don't have the required components installed:

#### 1. Enable WSL 2
Run the following commands in PowerShell as Administrator:

```powershell
# Enable WSL feature
dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart

# Enable Virtual Machine Platform
dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart

# Restart your computer, then set WSL 2 as default
wsl --set-default-version 2
```

#### 2. Install Docker Desktop
1. Download [Docker Desktop for Windows](https://www.docker.com/products/docker-desktop)
2. Install with WSL 2 backend enabled
3. Ensure Linux containers are selected (default)

#### 3. Verify Installation
```powershell
# Check WSL installation
wsl --list --verbose

# Check Docker installation
docker --version
docker run hello-world
```

## 💻 Usage

### Getting Started

1. **Launch GamePod** from the Start Menu or Desktop shortcut
2. **Create your first container** by clicking the "Create New Pod" button
3. **Configure your environment**:
   - Enter a unique name for your Pod
   - Select the target game engine
   - Choose your project folder to mount
   - Configure additional settings if needed
4. **Start developing** by opening the container terminal or launching the engine

### Managing Containers

Each container in GamePod can be managed through the main interface:

- **▶️ Start**: Launch the container and make it available for development
- **⏹️ Stop**: Safely stop the running container
- **🔄 Restart**: Restart the container to apply configuration changes
- **🗑️ Delete**: Permanently remove the container and its configuration
- **💻 Terminal**: Open a terminal session inside the container
- **📋 Guide**: View engine-specific usage instructions
- **🔍 Status**: Check container health and update availability

### Container Configuration Options

#### Basic Settings
- **Container Name**: Unique identifier for your development environment
- **Project Path**: Local folder to mount as `/project` in the container
- **Game Engine**: Pre-configured development environment

#### Advanced Settings
- **Port Bindings**: Expose container ports to Windows host
- **Additional Volumes**: Mount extra folders for assets, tools, or libraries  
- **Performance Settings**: CPU and memory limits for the container
- **Environment Variables**: Custom variables for your development workflow

### Working with Game Engines

#### Godot Engine
```bash
# Start Godot Editor
godot

# Run a project
godot --path /project

# Export project
godot --export "Linux/X11" /project/build/game.x86_64
```

#### Pygame Development
```bash
# Navigate to project
cd /project

# Run Python game
python main.py

# Install additional packages
pip install pygame-gui
```

#### Unity (CLI)
```bash
# Open Unity Hub
unity-hub

# Build project (headless)
unity-editor -batchmode -projectPath /project -buildTarget Linux64
```

### Tips and Best Practices

- **Project Organization**: Keep your project files in a dedicated folder for easy mounting
- **Resource Management**: Stop unused containers to free up system resources
- **Regular Updates**: Check for container image updates to get the latest tools and features
- **Backup Projects**: Your mounted project folders remain on Windows, making backup automatic

## 🏗️ Development Setup

### Prerequisites for Development

- **Visual Studio 2022** (Community, Professional, or Enterprise)
  - Workloads: ".NET Desktop Development" and "Universal Windows Platform development"
- **.NET 7.0 SDK** or later
- **Windows 10 SDK** (version 19041 or later)
- **Git** for version control
- **Docker Desktop** for testing containerization features

### Getting the Source Code

```bash
# Clone the repository
git clone https://github.com/GiorCocc/GamePod.git
cd GamePod
```

### Project Structure

```
GamePod/
├── GamePod/                    # Main WinUI 3 application
│   ├── Views/                  # XAML views and pages
│   ├── ViewModels/            # MVVM view models
│   ├── Services/              # Business logic services
│   ├── Models/                # Data models
│   ├── Assets/                # Images, guides, and resources
│   ├── Helpers/               # Utility classes
│   └── Contracts/             # Interface definitions
├── GamePod.Core/              # Core library
│   ├── Services/              # Shared services
│   ├── Contracts/             # Core interfaces
│   └── Helpers/               # Core utilities
├── assets/                    # Documentation assets
└── README.md                  # This file
```

### Setting Up the Development Environment

1. **Open the solution** in Visual Studio 2022:
   ```
   GamePod.sln
   ```

2. **Restore NuGet packages**:
   - Right-click solution → "Restore NuGet Packages"
   - Or use: `dotnet restore`

3. **Set startup project**:
   - Right-click "GamePod" project → "Set as Startup Project"

4. **Configure deployment**:
   - Build for your target architecture (x64, x86, or ARM64)
   - Use "Local Machine" for debugging

### Development Guidelines

#### Code Style
- Follow C# naming conventions and coding standards
- Use MVVM pattern for UI logic separation
- Implement dependency injection for services
- Add XML documentation for public APIs

#### Architecture Patterns
- **MVVM**: Model-View-ViewModel for UI components
- **Dependency Injection**: Using Microsoft.Extensions.Hosting
- **Repository Pattern**: For data access (containers, settings)
- **Command Pattern**: For UI actions and operations

## 🔧 Building from Source

### Build Requirements

- **Windows 10** version 1903+ or **Windows 11**
- **Visual Studio 2022** with required workloads
- **.NET 7.0** or later SDK
- **Windows App SDK** 1.3+ (installed via NuGet)

### Build Instructions

#### Command Line Build
```bash
# Clean solution
dotnet clean

# Restore dependencies
dotnet restore

# Build solution
dotnet build --configuration Release

# Create package
dotnet publish GamePod/GamePod.csproj -c Release -r win-x64
```

#### Visual Studio Build
1. **Open** `GamePod.sln` in Visual Studio 2022
2. **Select configuration**: Release | x64 (or your target platform)
3. **Build** → **Build Solution** (Ctrl+Shift+B)
4. **Deploy** → **Deploy GamePod** (for MSIX package)

### Creating Distribution Package

#### MSIX Package (Recommended)
```bash
# Build MSIX package
msbuild GamePod/GamePod.csproj /p:Configuration=Release /p:Platform=x64 /p:AppxPackageDir=../Output/
```

#### Portable Build
```bash
# Create self-contained deployment
dotnet publish GamePod/GamePod.csproj -c Release -r win-x64 --self-contained
```

### Running Tests

Currently, the project focuses on manual testing of UI components and Docker integration. Automated testing framework is planned for future releases.

```bash
# Manual testing checklist:
# 1. Container creation with different engines
# 2. Container lifecycle management (start, stop, restart)
# 3. File mounting functionality
# 4. Port binding configuration
# 5. WSLg graphical application support
```

## 🏛️ Architecture

### High-Level Architecture

GamePod follows a client-server architecture where the WinUI 3 application acts as the client, communicating with Docker Engine through the Docker API.

```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   WinUI 3 App   │    │   Docker Engine  │    │ Linux Container │
│   (Client)      │◄──►│   (Server)       │◄──►│ (Game Engine)   │
└─────────────────┘    └──────────────────┘    └─────────────────┘
        │                        │                        │
        │                        │                        │
    GamePod UI              Docker API               Development
   Management              Communication              Environment
```

### Technology Stack

#### Frontend (WinUI 3 Application)
- **Framework**: Windows App SDK 1.3+ with WinUI 3
- **Language**: C# 10+ with .NET 7.0
- **Architecture Pattern**: MVVM (Model-View-ViewModel)
- **UI Toolkit**: WinUI 3 controls with Community Toolkit extensions

#### Backend Services
- **Docker Communication**: Docker.DotNet library
- **Configuration Management**: Microsoft.Extensions.Configuration
- **Dependency Injection**: Microsoft.Extensions.Hosting
- **Data Serialization**: Newtonsoft.Json

#### Container Infrastructure
- **Container Platform**: Docker Engine with Linux containers
- **Base Images**: Ubuntu-based images with game engines pre-installed
- **Graphics Support**: WSLg for X11 forwarding
- **File System**: Bind mounts for project folder integration

### Key Components

#### Services Layer
```csharp
// Container management
IContainerService - Docker container lifecycle management
IImageService - Docker image pulling and management
IConfigurationService - Application settings and preferences

// UI Services  
INavigationService - Page navigation and routing
IDialogService - Modal dialogs and user prompts
INotificationService - Toast notifications and alerts
```

#### Data Models
```csharp
// Core entities
Container - Represents a Docker container instance
ContainerConfiguration - Container creation settings
GameEngine - Supported game engine definitions
ProjectSettings - Project-specific configurations
```

#### Docker Integration
The application uses the Docker.DotNet library to communicate with Docker Engine:

- **Container Operations**: Create, start, stop, remove containers
- **Image Management**: Pull, list, and update container images  
- **Volume Mounting**: Bind local directories to container paths
- **Network Configuration**: Port forwarding and network setup

### Security Considerations

- **Privilege Separation**: Containers run with limited privileges
- **Network Isolation**: Containers use isolated Docker networks
- **File System Security**: Project folders mounted with appropriate permissions
- **Docker Security**: Relies on Docker's built-in security features

## 🤝 Contributing

We welcome contributions from the community! GamePod is an open-source project that benefits from diverse perspectives and expertise.

### How to Contribute

#### 🐛 Reporting Bugs
1. **Check existing issues** to avoid duplicates
2. **Use the bug report template** when creating new issues
3. **Provide detailed information**:
   - Windows version and architecture
   - Docker Desktop version
   - Steps to reproduce the issue
   - Expected vs actual behavior
   - Screenshots or error logs

#### 💡 Suggesting Features
1. **Open a feature request** using the provided template
2. **Describe the use case** and benefits
3. **Consider implementation complexity** and maintenance impact

#### 🔧 Code Contributions
1. **Fork the repository** and create a feature branch
2. **Follow coding standards** and architecture patterns
3. **Test your changes** thoroughly
4. **Update documentation** if needed
5. **Submit a pull request** with a clear description

### Development Workflow

```bash
# 1. Fork and clone
git clone https://github.com/yourusername/GamePod.git
cd GamePod

# 2. Create feature branch
git checkout -b feature/your-feature-name

# 3. Make changes and commit
git add .
git commit -m "feat: add support for new game engine"

# 4. Push and create PR
git push origin feature/your-feature-name
```

### Code Style Guidelines

- **C# Conventions**: Follow Microsoft's C# coding conventions
- **XAML Formatting**: Use consistent indentation and naming
- **Comments**: Add XML documentation for public APIs
- **Commit Messages**: Use conventional commit format (feat:, fix:, docs:, etc.)

### Areas for Contribution

- 🎮 **New Game Engine Support**: Add containers for additional engines
- 🐛 **Bug Fixes**: Resolve reported issues and improve stability
- 📚 **Documentation**: Improve guides, tutorials, and API documentation
- 🎨 **UI/UX Improvements**: Enhance user interface and experience
- 🔧 **Performance**: Optimize container management and resource usage
- 🧪 **Testing**: Add automated tests and improve test coverage

## 🐛 Troubleshooting

### Common Issues and Solutions

#### Docker-Related Issues

**Problem**: "Docker is not running" error
```
Solution:
1. Ensure Docker Desktop is installed and running
2. Check if Docker service is started:
   services.msc → Docker Desktop Service → Start
3. Verify Docker is accessible: docker --version
```

**Problem**: Container creation fails
```
Solution:
1. Check available disk space (minimum 5GB free)
2. Verify Docker has permission to access the project folder
3. Try pulling the image manually: docker pull <image-name>
4. Check Docker logs in Docker Desktop → Troubleshoot
```

#### WSL/WSLg Issues

**Problem**: Graphical applications don't display
```
Solution:
1. Ensure WSL 2 is installed and set as default
2. Update WSL: wsl --update
3. Restart WSL: wsl --shutdown, then restart
4. Check WSLg is working: wsl -e xcalc
```

**Problem**: WSL 2 not available
```
Solution:
1. Enable required Windows features:
   - Windows Subsystem for Linux
   - Virtual Machine Platform
2. Restart computer after enabling features
3. Set WSL 2 as default: wsl --set-default-version 2
```

#### Application Issues

**Problem**: GamePod won't start
```
Solution:
1. Check Windows App SDK is installed
2. Try running as administrator
3. Check Windows Event Viewer for error details
4. Reinstall the application
```

**Problem**: Can't mount project folder
```
Solution:
1. Ensure the folder path exists and is accessible
2. Check folder permissions (should be readable/writable)
3. Avoid network drives or cloud-synced folders
4. Use short paths without special characters
```

### Getting Help

If you can't resolve an issue:

1. **Check the [Issues](https://github.com/GiorCocc/GamePod/issues)** for similar problems
2. **Create a new issue** with detailed information
3. **Join our community** for real-time support
4. **Review the documentation** for additional guidance

### Diagnostic Information

When reporting issues, please include:

```powershell
# System information
Get-ComputerInfo | Select-Object WindowsProductName, WindowsVersion, TotalPhysicalMemory

# Docker information  
docker --version
docker info

# WSL information
wsl --list --verbose
wsl --status
```

## 📖 Documentation

### User Guides
- **[Getting Started Guide](docs/getting-started.md)** - Step-by-step setup and first container
- **[Game Engine Guides](GamePod/Assets/)** - Engine-specific documentation:
  - [Godot Engine Guide](GamePod/Assets/GuidaGodot.md)
  - [Pygame Guide](GamePod/Assets/GuidaPyGame.md)  
  - [Unity Hub Guide](GamePod/Assets/GuidaUnityHub.md)
  - [Unity Editor Guide](GamePod/Assets/GuidaUnityEditor.md)
  - [Enigma Guide](GamePod/Assets/GuidaEnigma.md)

### Developer Documentation
- **[API Reference](docs/api-reference.md)** - Code documentation and examples
- **[Architecture Guide](docs/architecture.md)** - Detailed technical architecture
- **[Contributing Guide](CONTRIBUTING.md)** - Development workflow and standards

### Container Documentation
Each supported game engine includes comprehensive guides covering:
- Container setup and configuration
- Development workflow best practices
- Common commands and usage patterns
- Troubleshooting engine-specific issues

## 🔮 Roadmap

### Current Focus (v1.x)
- ✅ Core container management functionality
- ✅ Support for popular game engines (Godot, Pygame, Unity, Enigma)
- ✅ WSLg integration for graphical applications
- 🔄 Improved user interface and experience
- 🔄 Enhanced container configuration options

### Upcoming Features (v2.x)
- 🔜 **Additional Game Engines**: Unreal Engine, Construct 3, GameMaker
- 🔜 **Template System**: Pre-configured project templates
- 🔜 **Cloud Integration**: Sync projects with cloud storage services
- 🔜 **Performance Monitoring**: Resource usage tracking and optimization
- 🔜 **Plugin System**: Extension support for custom tools and workflows

### Long-term Vision (v3.x+)
- 🔮 **Cross-Platform Support**: MacOS and Linux versions
- 🔮 **Collaborative Development**: Multi-user container sharing
- 🔮 **CI/CD Integration**: Automated testing and deployment pipelines
- 🔮 **Remote Development**: Connect to remote Docker hosts
- 🔮 **Mobile Development**: Support for mobile game development tools

### Community Requests
Vote on upcoming features and suggest new ones in our [Discussions](https://github.com/GiorCocc/GamePod/discussions) section!

## 💬 Support & Community

### Getting Help
- 📖 **Documentation**: Check our comprehensive guides and tutorials
- 🐛 **Issues**: Report bugs and request features on GitHub Issues
- 💬 **Discussions**: Join community discussions for questions and ideas
- 📧 **Contact**: Reach out to the maintainers for direct support

### Community Guidelines
- Be respectful and inclusive in all interactions
- Search existing issues before creating new ones
- Provide detailed information when reporting problems
- Help others when you can share your knowledge

### Stay Updated
- ⭐ **Star the repository** to stay informed about updates
- 👁️ **Watch releases** for new version notifications
- 🐦 **Follow updates** on social media and development blogs

## 📚 Resources and Credits

### Technology Stack

| Component | Version | Description |
|-----------|---------|-------------|
| [.NET](https://dotnet.microsoft.com/) | 7.0+ | Core framework and runtime |
| [Windows App SDK](https://docs.microsoft.com/en-us/windows/apps/windows-app-sdk/) | 1.3+ | Modern Windows application platform |
| [WinUI 3](https://docs.microsoft.com/en-us/windows/apps/winui/winui3/) | Latest | Native Windows UI framework |
| [Docker.DotNet](https://github.com/dotnet/Docker.DotNet) | 3.125.15+ | Docker API client for .NET |
| [Docker Engine](https://www.docker.com/) | Latest | Container runtime and management |
| [WSL 2](https://docs.microsoft.com/en-us/windows/wsl/) | Latest | Windows Subsystem for Linux |

### Key Dependencies

```xml
<!-- Core frameworks -->
<PackageReference Include="Microsoft.WindowsAppSDK" Version="1.3.230502000" />
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.1.0" />

<!-- Docker integration -->
<PackageReference Include="Docker.DotNet" Version="3.125.15" />

<!-- UI enhancements -->
<PackageReference Include="CommunityToolkit.WinUI.Controls.SettingsControls" Version="8.0.230907" />
<PackageReference Include="CommunityToolkit.WinUI.UI.Controls.Markdown" Version="7.1.2" />
<PackageReference Include="WinUIEx" Version="2.2" />

<!-- Additional utilities -->
<PackageReference Include="Microsoft.Extensions.Hosting" Version="6.0.1" />
<PackageReference Include="Microsoft.Xaml.Behaviors.WinUI.Managed" Version="2.0.9" />
```

### Development Tools and Resources

- **[Template Studio](https://github.com/microsoft/TemplateStudio)**: Project template generation for Windows App SDK
- **[WinUI 3 Gallery](https://github.com/microsoft/WinUI-Gallery)**: Reference implementation and control showcase
- **[Community Toolkit](https://github.com/CommunityToolkit)**: Additional controls and utilities for WinUI 3
- **[Docker Hub](https://hub.docker.com/)**: Container image registry and documentation

### Game Engine Resources

- **[Godot Engine](https://godotengine.org/)**: Official Godot documentation and resources
- **[Pygame](https://www.pygame.org/)**: Python game development library documentation  
- **[GameCI](https://game.ci/docs/docker/docker-images/)**: Unity Docker images and CI/CD tools
- **[Enigma Development Environment](https://enigma-dev.org/)**: Community-driven game development platform

### Third-Party Container Images

GamePod utilizes community-maintained Docker images for game engines:

- **Unity Images**: Provided by [GameCI](https://game.ci/) - specialized for Unity development and CI/CD
- **Godot Images**: Community-maintained images with full editor support
- **Python/Pygame**: Official Python images with game development packages

> **Note**: GamePod packages and configures these images but does not maintain the underlying engine installations. Please refer to the respective communities for engine-specific support.

### Acknowledgments

Special thanks to:
- The Docker community for excellent containerization tools
- Microsoft for the Windows App SDK and development tools  
- Game engine communities for creating amazing development platforms
- Contributors and users who help improve GamePod

## 📄 License

This project is licensed under the **GNU General Public License v3.0** (GPL-3.0).

### What this means:
- ✅ **Freedom to use**: Use GamePod for any purpose, personal or commercial
- ✅ **Freedom to study**: Access and examine the source code
- ✅ **Freedom to modify**: Create your own versions and improvements
- ✅ **Freedom to distribute**: Share the software and your modifications

### Requirements:
- 📋 **Share alike**: Derivative works must also be licensed under GPL-3.0
- 📋 **Source code**: Must provide source code when distributing
- 📋 **License notice**: Include license and copyright notices
- 📋 **State changes**: Document any modifications made

### Full License Text
See the [LICENSE.txt](LICENSE.txt) file for the complete license terms.

### Third-Party Licenses
This project uses various open-source libraries and tools, each with their own licenses:
- .NET and Windows App SDK: MIT License
- Docker.DotNet: Apache License 2.0
- Community Toolkit: MIT License
- Game engine Docker images: Varies by project

---

<div align="center">

**Made with ❤️ for the game development community**

[⭐ Star this repo](https://github.com/GiorCocc/GamePod/stargazers) • [🐛 Report issues](https://github.com/GiorCocc/GamePod/issues) • [💬 Join discussions](https://github.com/GiorCocc/GamePod/discussions)

</div>
