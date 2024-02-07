# Defold Engine Docker Image Guide

This brief guide will help you make the most of your container with Defold Engine.

## Container Usage

The container is already perfectly configured to launch the Defold game engine. To activate Defold, run the following command in the container terminal:

```bash
defold
```

> **Note**: ignore the error messages that appears in the terminal, it is a known bug that does not affect the operation of the engine.

This will launch Defold Engine IDE in the opening page where you can select your project or create a new one and open it directly in the container using the graphical application. Its usage is the same as you would have on a Windows operating system.

> **NOTE**: other information and help guides can be found on the [Defold Engine website](https://defold.com/learn/).

> **NOTE**: Remember, any changes you make to the project are synchronized with your project on the host system; therefore, you can use your preferred text editor to modify project files. If you need to create new files or new elements, make sure they are created within the project folder so that they are visible inside the container.
