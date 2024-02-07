# Blender Docker Image Guide

This brief guide will help you make the most of your container with Blender.

## Container Usage

The container is already perfectly configured to launch the Blender editor. To activate Blender, run the following command in the container terminal:

```bash
blender
```

> **Note**: ignore the error messages that appears in the terminal, it is a known bug that does not affect the operation of the engine.

This will launch Blender in the opening page where you can select your project or create a new one and open it directly in the container using the graphical application. Its usage is the same as you would have on a Windows operating system.

> **NOTE**: Please, keep in mind that the rendering performance of the container is not the same as that of a native installation of Blender on your host system. Use this container only for testing purposes and not for production.

> **NOTE**: Remember, any changes you make to the project are synchronized with your project on the host system; therefore, you can use your preferred text editor to modify project files. If you need to create new files or new elements, make sure they are created within the project folder so that they are visible inside the container.
