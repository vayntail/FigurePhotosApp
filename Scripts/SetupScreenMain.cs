using System;
using Godot;

namespace FigurePhotosApp.Scripts;

public partial class SetupScreenMain : Control
{
    [Export] private Button _imagesFolderPathButton;
    [Export] private OptionButton _timerIntervalOptionButton;
    [Export] private HSlider _totalSlider;
    [Export] private Label _totalImagesLabel;
    [Export] private Label _totalTimeLabel;
    [Export] private CheckBox _fullscreenCheckBox;

    [Export] private FileDialog _folderDialog;
    [Export] private PackedScene _confirmationOverlayPackedScene;

    private Global _global;
    private bool _folderPathInitialized = false;
    
    public override void _Ready()
    {
        GetViewport().GetWindow().Mode = Window.ModeEnum.Windowed;
        Setup();
    }

    private void Setup()
    {
        // Initialize Global
        _global = GetNode<Global>("/root/Global");

        // If preset already exists
        if (_global.Preset != null)
        {
            SetupDisplayFromPreset(_global.Preset);
        }
        else
        {
            DisableSelections(true);
        }
    }

    private void UpdateLastSessionPreset()
    {
        
    }
    
    
    private void SetupDisplayFromPreset(Preset preset)
    {
        InitializeFolderPath(preset.FolderPath);

        _timerIntervalOptionButton.Selected = _timerIntervalOptionButton.GetItemIndex(preset.TimerInterval);
        _totalSlider.Value = preset.TotalImages;
        
        SetTotalLabels();

        _fullscreenCheckBox.ButtonPressed = preset.FullScreen;
    }
    
    private void OnImagesFolderPathButtonPressed()
    {
        _folderDialog.Visible = true;
    }
    
    private void OnFolderDialogPathSelected(string path)
    {
        InitializeFolderPath(path);
    }


    
    private void InitializeFolderPath(string path)
    {
        // Initialize the first folder path by dialog selection or via preset
        _folderPathInitialized = true;
        _imagesFolderPathButton.Text = path;
        
        _global.InitializeFolderPath(path);

        var allImagesCount = _global.ImagePathsList.Count;
        _totalSlider.MaxValue = allImagesCount;
        _totalSlider.Value = _totalSlider.MaxValue;
        
        DisableSelections(false);
    }

    private void DisableSelections(bool disabled)
    {
        _timerIntervalOptionButton.Disabled = disabled;
        _fullscreenCheckBox.Disabled = disabled;
        _totalSlider.Editable = !disabled;
        _totalSlider.Scrollable = !disabled;
        if (disabled)
        {
            _totalSlider.MouseFilter = MouseFilterEnum.Ignore;
            _timerIntervalOptionButton.MouseFilter = MouseFilterEnum.Ignore;
            _fullscreenCheckBox.MouseFilter = MouseFilterEnum.Ignore;
        }
        else
        {
            _totalSlider.MouseFilter = MouseFilterEnum.Stop;
            _timerIntervalOptionButton.MouseFilter = MouseFilterEnum.Stop;
            _fullscreenCheckBox.MouseFilter = MouseFilterEnum.Stop;
        }
    }
    
    private void SetTotalLabels()
    {
        // Set TotalImagesLabel
        _totalImagesLabel.Text = _totalSlider.Value + " images";

        // Set TotalTimeLabel
        var totalSeconds = GetTotalSeconds();
        var totalTimeSpan = TimeSpan.FromSeconds(totalSeconds);

        if (totalTimeSpan.Hours >= 1)
        {
            _totalTimeLabel.Text = TimeSpan.FromSeconds(totalSeconds).ToString("h'h 'm'm 's's'");
        }
        else if (totalTimeSpan.Hours == 0)
        {
            if (totalTimeSpan.Minutes >= 1)
            {
                _totalTimeLabel.Text = TimeSpan.FromSeconds(totalSeconds).ToString("m'm 's's'");
            }
            else if (totalTimeSpan.Minutes == 0)
            {
                _totalTimeLabel.Text = TimeSpan.FromSeconds(totalSeconds).ToString("s's'");
            }
        }
    }

    private int GetTotalSeconds()
    {
        var timerInterval = GetTimerInterval();
        return (int)_totalSlider.Value * timerInterval;
    }

    private int GetTimerInterval()
    {
        // get item ID, which is the interval seconds
        return _timerIntervalOptionButton.GetItemId(_timerIntervalOptionButton.Selected);
    }

    private void OnStartButtonPressed()
    {
        // If no folder path is selected or no images exist in list, send a warning overlay
        if (!_folderPathInitialized | _global.ImagePathsList.Count==0 )
        {
            ConfirmationOverlay window = _confirmationOverlayPackedScene.Instantiate<ConfirmationOverlay>();
            window.Initialize("you must select a folder that contains at least 1 image!", "OK", null);
            AddChild(window);
        }
        else
        {
            Preset preset = new Preset();
            preset.Initialize(
                folderPath: _imagesFolderPathButton.Text,
                timerInterval: GetTimerInterval(),
                totalImages: (int)_totalSlider.Value,
                totalSeconds: GetTotalSeconds(),
                fullscreen: _fullscreenCheckBox.ButtonPressed
            );
            _global.SetPreset(preset);
            
            _global.GotoScene("res://Scenes/InSessionScreen.tscn");
        }
    }
    
    
        

    private void OnTotalSliderValueChanged(float value)
    {
        SetTotalLabels();
    }

    private void OnTimerOptionButtonItemSelected(int index)
    {
        SetTotalLabels();
    }

}