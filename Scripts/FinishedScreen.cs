using Godot;

namespace FigurePhotosApp.Scripts;

public partial class FinishedScreen : Control
{
    [Export] private GridContainer _imagesGridContainer;
    [Export] private PackedScene _finishedImageButtonPackedScene;
    
    private Global _global;
    public override void _Ready()
    {
        GetViewport().GetWindow().Mode = Window.ModeEnum.Windowed;
        
        // Initialize Global
        _global = GetNode<Global>("/root/Global");
        DisplaySessionImages();
    }

    private void DisplaySessionImages()
    {
        GD.Print(_global.UsedImagePathsList.Count);
        
        foreach (var imagePath in _global.UsedImagePathsList)
        {
            Image image = Image.LoadFromFile(imagePath);
            
            FinishedImageButton button = _finishedImageButtonPackedScene.Instantiate<FinishedImageButton>();
            button.Initialize(image);

            _imagesGridContainer.AddChild(button);
        }
    }

    private void OnHomeButtonPressed()
    {
        _global.GotoScene("res://Scenes/SetupScreen.tscn");
    }
}