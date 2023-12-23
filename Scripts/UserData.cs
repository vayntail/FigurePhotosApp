using Godot;
using Godot.Collections;

namespace FigurePhotosApp.Scripts;

public partial class UserData : Resource
{
    [Export] public Preset LastSessionPreset;
    [Export] public Array<Preset> PresetsArray;
    
    public UserData()
    {
        LastSessionPreset = null;
        PresetsArray = new Array<Preset>();
    }
}