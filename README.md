# Highscore service test game client
This is a part of one of my C# practice projects, it is not designed for production use. 
The code is shared mostly for demonstration purposes.

The project consists of two solutions:
- Highscore Service (uses gRPC for communication, EFCore SQLite for data storage) - Code stored here (https://github.com/Ivan-267/gRPCHighscoreService)
- Highscore Client (a minigame built with Godot game engine using C# as a simple test client for the service) - Code stored in this repository

After getting and starting the HighScore Service, the test game can be launched from the Godot Editor (tested with Godot_v3.4.4-stable_mono_win64). 
The game can also be launched without the service, but the Highscore functionality will not work.