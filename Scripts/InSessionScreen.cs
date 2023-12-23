using System;
using Godot;

namespace FigurePhotosApp.Scripts;

public partial class InSessionScreen : Control
{
    [Export] private TextureRect _imageTextureRect;
    [Export] private Label _timerLabel;
    [Export] private Timer _timer;
    [Export] private ProgressBar _timerProgressBar;
    [Export] private StyleBoxFlat _timerBarStyleBox;
    [Export] private AnimationPlayer _uiAnimationPlayer;
    [Export] private TextureButton _fullScreenButton;
    [Export] private TextureButton _pauseButton;
    [Export] private PackedScene _confirmationOverlayPackedScene;

    private Global _global;
    private int _imageCount = 0;
    private bool _paused;

    private Color _greyBarColor = Color.FromHtml("5f5f5f");

    public override void _Ready()
    {
        // Initialize Global
        _global = GetNode<Global>("/root/Global");
        
        // Set Timer
        _timer.WaitTime = _global.Preset.TimerInterval;
        _timerProgressBar.MaxValue = _global.Preset.TimerInterval;
        
        // Fullscreen
        if (_global.Preset.FullScreen)
        {
            GetViewport().GetWindow().Mode = Window.ModeEnum.Fullscreen;
            _fullScreenButton.ButtonPressed = true;
        }
        
        DisplayRandomImage();
    }
    
    public override void _Process(double delta)
    {
        
        double timerTime = _timer.WaitTime - _timer.TimeLeft;
        _timerProgressBar.Value = timerTime; // Display on timerProgressBar
        
        // Display as 00:00 using timeSpan
        _timerLabel.Text = TimeSpan.FromSeconds(_timer.TimeLeft).ToString(@"mm\:ss");
        
        

        // Change timerProgressBar color to red, then orange when get close to finish
        if (_timerProgressBar.Value == 0)
        {
            _timerBarStyleBox.BgColor = _greyBarColor;
        }
        if (_timerProgressBar.Value == _timer.WaitTime * 0.65)
        {
            // Change bar color to orange
            _uiAnimationPlayer.Play("TimerBarColorSwapToOrange");
        }
        if (_timerProgressBar.Value == _timer.WaitTime * 0.9)
        {
            // Change bar color to red
            _uiAnimationPlayer.Play("TimerBarColorSwapToRed");
        }
    }

    private void DisplayRandomImage()
    {
        Image image = _global.GetRandomImage();
        
        _imageTextureRect.Texture = ImageTexture.CreateFromImage(image);
        _timer.Start();

        _imageCount += 1;
    }

    private void OnTimerTimeOut()
    {
        // If image count has not yet hit total images
        if (_imageCount != _global.Preset.TotalImages)
        {
            // Change Picture
            DisplayRandomImage();
        }
        else
        {
            // Finish Session
            GD.Print("(InSessionScene) Finished Session!");
            _global.GotoScene("res://Scenes/FinishedScreen.tscn");
        }
    }

    
    // Button Actions
    //
    private void OnFullScreenButtonToggled(bool on)
    {
        if (on)
        {
            GetViewport().GetWindow().Mode = Window.ModeEnum.Fullscreen;
        }
        else
        {
            GetViewport().GetWindow().Mode = Window.ModeEnum.Windowed;
        }
    }
    
    private void OnExitButtonPressed()
    {
        _timer.Paused = true;
        _uiAnimationPlayer.Play("UIFadeOut");
        
        ConfirmationOverlay window = _confirmationOverlayPackedScene.Instantiate<ConfirmationOverlay>();
        window.Initialize("cancel current session?", "OK", "no");
        AddChild(window);
        
        // Event handlers
        window.Canceled += () =>
        {
            _uiAnimationPlayer.Play("UIFadeIn");
            RemoveChild(window);
            window.QueueFree();
            
            _timer.Paused = _paused;
        };
        window.Confirmed += () =>
        {
            RemoveChild(window);
            window.QueueFree();
            _global.GotoScene("res://Scenes/SetupScreen.tscn");
        };
    }

    private void OnPausePlayButtonToggled(bool paused)
    {
        _timer.Paused = paused;
        _paused = paused;
    }

    private void OnRestartButtonPressed()
    {
        PauseTimer(true);
        
        ConfirmationOverlay window = _confirmationOverlayPackedScene.Instantiate<ConfirmationOverlay>();
        window.Initialize("restart image?", "OK", "no");
        AddChild(window);
        
        // Event handlers
        window.Confirmed += () =>
        {
            _timer.Stop();
            _timer.Start();
            PauseTimer(false);

        };
        window.Canceled += () =>
        {
            PauseTimer(false);
        };
    }

    private void PauseTimer(bool paused)
    {
        _timer.Paused = paused;
        _pauseButton.ButtonPressed = paused;
    }
}