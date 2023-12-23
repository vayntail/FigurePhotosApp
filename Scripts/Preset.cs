using System.ComponentModel;
using Godot;

namespace FigurePhotosApp.Scripts;

public partial class Preset : Resource
{
    [Export] public string FolderPath;
    [Export] public int TimerInterval;
    [Export] public int TotalImages;
    [Export] public int TotalSeconds;
    [Export] public bool FullScreen;

    public void Initialize(string folderPath, int timerInterval, int totalImages, int totalSeconds, bool fullscreen)
    {
        FolderPath = folderPath;
        TimerInterval = timerInterval;
        TotalImages = totalImages;
        TotalSeconds = totalSeconds;
        FullScreen = fullscreen;
        GD.Print("(Preset) Preset values are: ");
        GD.Print("FolderPath: ", FolderPath);
        GD.Print("TimerSeconds: ", TimerInterval);
        GD.Print("TotalImages: ", TotalImages);
        GD.Print("TotalSeconds: ", TotalSeconds);
        GD.Print("FullScreen: ", FullScreen);
    }
}