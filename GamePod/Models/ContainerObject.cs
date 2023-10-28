using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePod.Models;

public class ContainerObject
{
    public string Name { get; set; }
    public string Status { get;set; }

    public ContainerObject(string name, string status)
    {
        Name = name;
        Status = status;
    }

    // get all the containers: GetContainerObjectsAsync

}
