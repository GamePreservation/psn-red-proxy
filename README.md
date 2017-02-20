PSN Redirecting Proxy
=====================

PSN Redirecting Proxy allows faster downloads from a PlayStation 3, 4 or Vita device by redirecting downloads to files stored on your local PC.

It is based on [PSX Download Helper](https://github.com/KOPElan/PSX-Download-Helper) with a few differences:

- Simplified feature set

- Command line interface

- Files in local folder can be organized in subfolders


Requirements
------------

Built on .NET Framework 4.5.2. Tested on Windows 10. Other configurations are untested.


Usage
-----

### Determine the IP address of your PC

You will need to determine the IP address of your computer. This can be done using `ipconfig.exe`. Look for the `IPv4 Address` entry. In this example, it's `192.168.0.176`.

```
C:\>ipconfig.exe

Windows IP Configuration

   IPv4 Address. . . . . . . . . . . : 192.168.0.176
   Subnet Mask . . . . . . . . . . . : 255.255.255.0
   Default Gateway . . . . . . . . . : 192.168.0.1

```


### Setup your PlayStation device network settings

On your PlayStation device, go to the network settings and find the proxy settings. This varies depending on which device you have.

Select to use a proxy, and it will prompt you for an IP address and a port number.

Enter the IPv4 Address as listed in the Windows IP Configuration, and pick a port number that is available on your PC. In this example, we'll use `8080`.


### Setup PC local folder

On your PC, create a folder where you will store the package files. In this example, we'll use `C:\Packages`.

You can organize this folder in any way you want. The proxy will recursively look for the files under the local folder.

For example:

```
C:\Packages\Sony - PlayStation 3\Old Demo\asdlkjzhiuwhefn1890xcY998usfdjkll87.pkg
C:\Packages\Sony - PlayStation 4\Super Cool Game\UP0000-CUSA00000_00-AAAAAAAAAAAAAAAA_0.pkg
C:\Packages\Sony - PlayStation 4\Super Cool Game\UP0000-CUSA00000_00-AAAAAAAAAAAAAAAA_1.pkg
C:\Packages\Sony - PlayStation 4\Super Cool Game\UP0000-CUSA00000_00-AAAAAAAAAAAAAAAA_2.pkg
```


### Start the proxy and download

On your PC, open a command prompt window in the PSN Redirecting Proxy folder.

Launch the executable, specifying the IP address, port and the path to the local folder.

```
PSNRedProxy.exe -i 192.168.0.176 -p 8080 -l "C:\Packages"
```

The first time, you may see a prompt from the Windows firewall. You will need to confirm that you want your PC to accept incoming connections for PSN Redirecting Proxy.

On your PlayStation device, go to your download list or game library and start a download.

When a request comes in for a PSN package, it will be displayed in the console. The URL is color coded:

- Blue: The file is being downloaded from your local disk
- Purple: The file is being downloaded from the internet (it was not found on your local disk)


Known Issues
------------

- Crash on startup if the port is not available.

- Currently the only way to stop the proxy is to close the Command Prompt window, or end the process via Task Manager.

- Check for updates or online play does not work. If you want to avoid having to constantly change the network settings on your PlayStation device as you install and play online games, you may want to consider switching proxies on your PC instead. [Privoxy](https://www.privoxy.org/) is a proxy that works for most online games. You can setup Privoxy on the same IP Address/Port as PSN Redirecting Proxy, and start/stop Privoxy and PSN Redirecting Proxy as you need.


Credits
-------

PSN Redirecting Proxy is based on the source of [PSX Download Helper](https://github.com/KOPElan/PSX-Download-Helper).
