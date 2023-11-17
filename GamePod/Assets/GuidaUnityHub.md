# Unity Hub Docker Image Guide

This short guide will help you make the most of your Unity Hub container.

## License Activation

Upon container startup, Unity Hub is not yet authenticated with your Unity account. Follow these steps to activate the license:

1. In a terminal opened in the container (you can open one with the `execute` button), you need to create a manual activation file. To do this, type the following command:

    ```bash
    unity-hub -createManualActivationFile
    ```

    The file is created in the root of the container's file system, so it's not visible on the host system. Therefore, copy the file to the project folder, which is accessible from the container. To do this, type the following command:

    ```bash
    cp *.alf /project
    ```

    > NOTE: Alternatively, you can run the `unity-hub -createManualActivationFile` command directly in your project folder.

2. Open a web browser on your host machine and go to [https://license.unity3d.com/manual](https://license.unity3d.com/manual) where you need to perform the following steps:

    1. Enter the `.alf` file created inside the container into the page's box and click the NEXT button.
    2. On the next page, enter the Unity license key (only if you have the Pro or Plus plan) and click the NEXT button.

        If you don't have these types of licenses, open the page inspector (right-click -> inspect or `F12`) and look for the element

        ```html
        <div class="option option-personal clear" style="display: none;">
        ```

        and delete the content in the `style` attribute so that it becomes

        ```html
        <div class="option option-personal style="">
        ```

        Then go back to the page and click on Personal and then NEXT.

    3. On the next page, download the activation file and save it in the project folder.

3. Return to the container terminal and type the following command to activate the license:

    ```bash
    unity-hub -manualLicenseFile <activation file path>
    ```

## Container Usage

The container with only Unity Hub is useful for managing Unity projects but does not allow game execution. For this reason, a container with Unity Editor has been created.

To install a specific version of Unity Editor, use the following command:

```bash
unity-hub install --version <version code> --changeset <change set> --module <additional modules>
```

To run the game, use the following command:

```bash
unity-editor -projectPath <project path> -executeMethod <method name to execute>
```

> **NOTE: To run the game, you must have the correct version of Unity Editor installed.**

### Example of Unity Editor Installation

```bash
unity-hub install --version 2021.1.21f1 --changeset 9fdda2f0c0f1 --module linux-il2cpp
```

> **NOTE**: More information on available commands and usage is available in the [official documentation](https://docs.unity3d.com/2023.3/Documentation/Manual/EditorCommandLineArguments.html#:~:text=On%20Windows%2C%20type%20the%20following%20into%20the%20Command,test%20suites%2C%20automated%20builds%20and%20other%20production%20tasks).
>
> **NOTE**: Due to limitations imposed by Unity and the license used, simultaneous opening of the same project inside the container and on the host system cannot be executed; therefore, it is recommended to use this image only at the end of the project or as a debugging tool.
>
> **NOTE**: Container image created and distributed by [GameCI](https://game.ci/docs/docker/docker-images)
>
> Information on the Docker image of the container available at [unityci/hub](https://hub.docker.com/r/unityci/hub)
>