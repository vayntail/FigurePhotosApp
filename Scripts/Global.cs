using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FigurePhotosApp.Scripts;
using Godot;

public partial class Global : Node
{
    private static readonly List<string> ImageExtensions = new List<string> { ".jpg", ".jpeg", ".png" };
    
    private Node CurrentScene { get; set; }
    
    // PUBLIC USE
    public Preset Preset;
    
    public string FolderPath;
    public List<string> ImagePathsList = new();
    private UserData _userData;

    public readonly List<string> UsedImagePathsList = new();
    
    
    private UserDataManager _userDataManager;
    
    //

    public void SetPreset(Preset preset)
    {
        Preset = preset;
        _userDataManager.SetLastSessionPreset(preset);
        _userDataManager.SaveUserData();
    }
    public override void _Ready()
    {
        // Set Current Scene
        Viewport root = GetTree().Root;
        CurrentScene = root.GetChild(root.GetChildCount() - 1);
        
        // Initialize UserDataManager
        _userDataManager = GetNode<UserDataManager>("/root/UserDataManager");


        // Get UserData
        _userData = _userDataManager.GetUserData();
        // Get last session's Preset if it exists.
        Preset = _userDataManager.GetLastSessionPreset();
    }

    public void InitializeFolderPath(string path)
    {
        FolderPath = path;
        SetImagePathsList();
    }

    public void SetImagePathsList()
    {
        // Clear List and restart when a new folder is set
        ImagePathsList.Clear();
        GD.Print("hi");
        foreach (var filePath in Directory.GetFiles(FolderPath))
        {
            if (ImageExtensions.Contains(Path.GetExtension(filePath).ToLowerInvariant()))
            {
                // Add to list if file is image
                ImagePathsList.Add(filePath);
            }
        }
    }

    public Image GetRandomImage()
    {
        Random random = new Random();
        int index = random.Next(ImagePathsList.Count);
        
        UsedImagePathsList.Add(ImagePathsList[index]);
        Image image = Image.LoadFromFile(ImagePathsList[index]);
        
        ImagePathsList.RemoveAt(index);

        return image;
    }
    
    public void GotoScene(string path)
    {
        CallDeferred(MethodName.DeferredGotoScene, path);
    }

    public void DeferredGotoScene(string path)
    {
        // It is now safe to remove the current scene.
        CurrentScene.Free();

        // Load a new scene.
        var nextScene = GD.Load<PackedScene>(path);

        // Instance the new scene.
        CurrentScene = nextScene.Instantiate();

        // Add it to the active scene, as child of root.
        GetTree().Root.AddChild(CurrentScene);

        // Optionally, to make it compatible with the SceneTree.change_scene_to_file() API.
        GetTree().CurrentScene = CurrentScene;
    }
}