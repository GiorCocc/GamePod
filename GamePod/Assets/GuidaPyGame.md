# Pygame Docker Image Guide

This brief guide will help you make the most of your Pygame container.

## Container Usage

The Pygame container is useful for managing Pygame projects and running the game. To open the project, type the following command:

```bash
cd /project
```

then, run the project with the Python interpreter:

```bash
python <project file>.py
```

## Container Usage (Advanced)

If you need to install additional packages, you can use the `pip` command. For example, to install the extra Pygame package, type the following command:

```bash
pip install pygame
```

In the same way, you can install any other package you need, as long as it is compatible with the Python version installed in the container.

> NOTE: The container is based on the `python:latest` image, so you can find more information about the Python version [here](https://hub.docker.com/_/python).