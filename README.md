**igbtool** is a project for utilities to handle .IGB (Intrinsic Graphics binary) files used for *Crash Nitro Kart* (PS2, XBOX, GCN). It may work on other games' files, but this is not tested.

There are actually two programs in this repo:

## igbtool

The titular **igbtool**, which is a command-line utility to extract IGB file information and save it to a text file. It also supports ripping images from IGB files, but this does not actually work.

### Building

The program is built to a `bin` folder inside the `build` folder on the project directory. There are intermediate steps which have different names depending on the build configuration used.

#### Linux

The project is just a cmake with a few files. No fancy steps should be needed to build the program, but I do not use Linux so I wouldn't know. Feel free to PR whatever it takes to make it work!

#### Windows

To build the program using Visual Studio, first you need to open it as a CMake Project and then open the `CMakeLists.txt` file:

![image](https://user-images.githubusercontent.com/7569514/135014088-11f29024-7b20-4193-b16d-a13524f6b952.png)

Then you can press Ctrl+Shift+B to build the entire solution (this consists of the `igbtool` project + a few extra libraries).

To launch the program through Visual Studio, click the arrow to the right of this button in the toolbar to bring up a drop down of run configurations to choose from:

![image](https://user-images.githubusercontent.com/7569514/135014546-ab410291-92ea-489f-8f5f-ea4acb99676f.png)

You will only see the default ones, but you can add more by editing the `launch.vs.json` file in your `.vs` folder. The file looks like this for example:
```json
{
  "version": "0.2.1",
  "defaults": {},
  "configurations": [
    {
      "type": "default",
      "project": "CMakeLists.txt",
      "projectTarget": "igbtool.exe (bin\\igbtool.exe)",
      "name": "Run igbtool (XPAL barinboss)",
      "args": [ "..\\..\\CNK\\XPAL\\Backup\\assets\\xbox\\gfx\\chars\\barinboss.igb" ]
    }
  ]
}
```
You should change parameter like `name` and `args` as you see fit.

## igbgui

A GUI for inspecting IGB files. This is built using .NET Core 5.0 WinForms, so it is Windows-only. A Linux version would be nice, though.

To build it simply run the Visual Studio solution and build it with Ctrl+Shift+B. The resulting files are in `igbgui/bin/<configuration>/net5.0-windows/`.
