namespace GamePod.Models;

/*
 *  LinuxDistributions.cs
 *  @Author: Giorgio Coccapani
 *  @Date: 25/09/2023
 *  
 *  This class contains all the Linux distributions that are supported by the GamePods.
 *  For every linux distro there is a name, the code for the container and che command for the package manager.
 */

internal class LinuxDistribution
{
    // Linux distributions
    public static readonly LinuxDistribution Ubuntu   = new("Ubuntu", "ubuntu", "apt-get");
    public static readonly LinuxDistribution Debian   = new("Debian", "debian", "apt-get");
    public static readonly LinuxDistribution Fedora   = new("Fedora", "fedora", "dnf");
    public static readonly LinuxDistribution Arch     = new("Arch", "archlinux", "pacman");
    public static readonly LinuxDistribution Manjaro  = new("Manjaro", "manjaro", "pacman");
    public static readonly LinuxDistribution OpenSuse = new("OpenSuse", "opensuse", "zypper");
    public static readonly LinuxDistribution CentOS   = new("CentOS", "centos", "yum");
    public static readonly LinuxDistribution Gentoo   = new("Gentoo", "gentoo", "emerge");
    public static readonly LinuxDistribution Void     = new("Void", "void", "xbps-install");
    public static readonly LinuxDistribution Alpine   = new("Alpine", "alpine", "apk");

    // Properties
    public string Name
    {
        get; private set;
    }
    public string ContainerCode
    {
        get; private set;
    }
    public string PackageManager
    {
        get; private set;
    }

    // Constructor
    private LinuxDistribution(string name, string containerCode, string packageManager)
    {
        Name = name;
        ContainerCode = containerCode;
        PackageManager = packageManager;
    }

    // Get the LinuxDistro object from the name
    public static LinuxDistribution GetLinuxDistro(string name)
    {
        switch (name)
        {
            case "Ubuntu":
                return Ubuntu;
            case "Debian":
                return Debian;
            case "Fedora":
                return Fedora;
            case "Arch":
                return Arch;
            case "Manjaro":
                return Manjaro;
            case "OpenSuse":
                return OpenSuse;
            case "CentOS":
                return CentOS;
            case "Gentoo":
                return Gentoo;
            case "Void":
                return Void;
            case "Alpine":
                return Alpine;
            default:
                return null;
        }
    }

    // Get a list of all the Names of the LinuxDistro objects
    public static List<string> GetLinuxDistroNamesList()
    {
        List<string> names = new List<string>();

        names.Add(Ubuntu.Name);
        names.Add(Debian.Name);
        names.Add(Fedora.Name);
        names.Add(Arch.Name);
        names.Add(Manjaro.Name);
        names.Add(OpenSuse.Name);
        names.Add(CentOS.Name);
        names.Add(Gentoo.Name);
        names.Add(Void.Name);
        names.Add(Alpine.Name);

        return names;
    }

}
