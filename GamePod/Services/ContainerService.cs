using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 *  ContainerService.cs
 *  @Author: Giorgio Coccapani
 *  @Date: 25/09/2023
 *  
 *  This class contains the service that will be used to create the container.
 *  This class receives the command to create the container and runs it in the terminal.
 *  
 */

namespace GamePod.Services;
internal class ContainerService
{
    // Properties
    public string Command { get; private set; } = string.Empty;

    // Constructor
    public ContainerService(string command)
    {
        Command = command;
    }

    // Run the command in the terminal
    public void RunCommand()
    {
        // open the terminal and paste the command
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = "powershell.exe";
        startInfo.Arguments = "/C " + Command;
        Process.Start(startInfo);
    }
}
