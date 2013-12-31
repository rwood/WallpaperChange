WallpaperChange
===============

Changes the wallpaper based on the time of day.

Download Binaries: http://tamarau.com/WallpaperChange/WallpaperChange_0.4.zip

Install:
Download and unzip the application.  
Run the application.
Right-click on the Tray Icon and select "Start Automatically".

Uninstall:
Right-click on the Tray Icon and de-select "Start Automatically".
Delete the files.


Configuration:
By default it should 1920x1080 resolution desktops. Replace the images with the one for the proper resolution from http://www.reddit.com/r/wallpapers/comments/1tqe9k/update_new_version_of_the_8bit_day_wallpaper_set/
You can change the times and the filenames in the WallpaperChange.exe.config file. The keys must be in the format hh:mm and the values should be the path the wallpaper.
The transition_slices value tells the program how many intermediate pictures to make for transitions. 
The transition_time_ms value tells the program how long the entire transition should take. 
The default is 10 frames over 5 seconds.


v0.4 Added config option to make the wallpaper style Tiled, Centered or Stretched (case-sensitive).
v0.3 fixes issue with crashing after leaving your computer for a while.
