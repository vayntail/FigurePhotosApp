using System.Collections.Generic;
using Godot;

namespace FigurePhotosApp.Scripts;

public partial class UserDataManager : Node
{
    private const string UserDataPath = "user://UserData.tres";
    private UserData _userData;
    
    public UserData GetUserData()
    {
        // Load UserFolders.tres
        if (ResourceLoader.Exists(UserDataPath))
        {
            GD.Print("(Global) UserData.tres exists!");
            _userData = ResourceLoader.Load<UserData>(UserDataPath);
            return _userData;
        }
        else
        {
            GD.Print("(Global) UserData.tres does exist.");
            _userData = new UserData();
            
            SaveUserData();

            return _userData;
        }
    }
    
    public void SaveUserData()
    {
        ResourceSaver.Save(_userData, UserDataPath);
        if (ResourceLoader.Exists(UserDataPath))
        {
            GD.Print("(Global) UserFolders.tres saved!");
        }
    }

    public void SetLastSessionPreset(Preset preset)
    {
        _userData.LastSessionPreset = preset;
    }

    public Preset GetLastSessionPreset()
    {
        return _userData.LastSessionPreset;
    }
}