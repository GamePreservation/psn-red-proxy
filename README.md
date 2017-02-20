PSN Redirecting Proxy
=====================

PSN Redirecting Proxy allows faster downloads from a PlayStation 3, 4 or Vita device by redirecting downloads to files stored on your local PC.

It is based on [PSX Download Helper](https://github.com/KOPElan/PSX-Download-Helper) with a few differences:

- Simplified feature set

- Command line interface

- Files in local folder can be organized in subfolders


Usage
-----

### Determine the IP addres of your PC

You will need to determine the IP address of your computer. This can be done using `ipconfig.exe`. Look for the IPv4 Address entry. In this example, it's `192.168.0.176`.

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

You can organize this folder in any way you want. The proxy will recursively look for the files under the root folder.

For example:

```
C:\Packages\Sony - PlayStation 3\Old Demo\asdlkjzhiuwhefn1890xcY998usfdjkll87.pkg
C:\Packages\Sony - PlayStation 4\Super Cool Game\UP0000-CUSA00000_00-AAAAAAAAAAAAAAAA_0.pkg
C:\Packages\Sony - PlayStation 4\Super Cool Game\UP0000-CUSA00000_00-AAAAAAAAAAAAAAAA_1.pkg
C:\Packages\Sony - PlayStation 4\Super Cool Game\UP0000-CUSA00000_00-AAAAAAAAAAAAAAAA_3.pkg
```


### Start the proxy and download

On your PC, open a command prompt window in the PSN Redirecting Proxy folder.

Launch the executable, specifying the IP address, port and the path to the local folder.

```
PSNRedProxy.exe -i 192.168.0.176 -p 8080 -l "C:\Packages"
```

The first time, you may see a prompt from the Windows firewall. You will need to confirm that you want your PC to accept incoming connections via PSN Redirecting Proxy.

On your PlayStation device, go to your download list or game library and start a download.

When a request comes in for a PSN package, it will be displayed in the console. The URL is color coded:

- Blue: The file is being downloaded from your local disk
- Purple: The file is being downloaded from the internet


Known Issues
------------

- Crash on startup if the port is not available.

- Currently the only way to stop the proxy is to close the Command Prompt window, or end the process via Task Manager.

- The proxy doesn't provide the necessary functionality for check for updates to work, or online play. As a workaround, you can installer another proxy which enables this functionality, but doesn't enable redirecting of downloads to local disk. An example is [Privoxy](https://www.privoxy.org/). You can setup Privoxy on the same IP Address/Port, and start/stop Privoxy and PSN Redirecting Proxy as you need. This way, you do not need to change the settings on your PlayStation device as you go between downloading and playing online games.


Credits
-------

PSN Redirecting Proxy is based on on the source of [PSX Download Helper](https://github.com/KOPElan/PSX-Download-Helper).
