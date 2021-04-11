# LUDUM DARE 48 MYSTERY PROJECT
This repository will contain our Ludum Dare 48 project, built using .net.

This is an example change.

## Requirements
- [.net 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- An IDE with support for .net, such as [JetBrains Rider] (https://www.jetbrains.com/rider/).
- [MGCB Editor] (https://docs.monogame.net/articles/tools/mgcb_editor.html) in order to add new assets.

## Supported systems
For now we're targeting Desktop OpenGL, which should work on Windows, Linux and macOS.

## Starting the game
In the root of the repository:
```
dotnet build
```
will restore all projects.
```
dotnet run --project .\LD48\LD48.csproj
```
will run the main project. It's also possible to just write "dotnet run" while in the LD48 folder.

## Contributing
Pull requests are required, no pushes directly in main. The code style is open to discussion.
