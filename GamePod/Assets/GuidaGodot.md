# Godot Docker Image Guide

This brief guide will help you make the most of your container with Godot Engine.

## Container Usage

The container is already perfectly configured to launch the Godot game engine. To activate Godot, run the following command in the container terminal:

```bash
godot
```

This will launch Godot Engine in the opening page where you can select your project or create a new one and open it directly in the container using the graphical application. Its usage is the same as you would have on a Windows operating system.

> **NOTE**: more information on how to use Godot Engine and its command line can be found on the [official website](https://docs.godotengine.org/en/3.3/getting_started/editor/command_line_tutorial.html)
>
> **NOTE**: Remember, any changes you make to the project are synchronized with your project on the host system; therefore, you can use your preferred text editor to modify project files. If you need to create new files or new elements, make sure they are created within the project folder so that they are visible inside the container.
>