using Godot;

namespace FigurePhotosApp.Scripts;

public partial class InSessionScreenUI : Control
{
    [Export] private CanvasLayer _UIButtonsLayer;
    [Export] private TextureButton _exitButton;
    [Export] private TextureButton _fullScreenButton;
    [Export] private TextureButton _pausePlayButton;
    [Export] private TextureButton _restartButton;
    [Export] private AnimationPlayer _uiAnimationPlayer;
    
    
    private bool _buttonHovered = false;
    
    private void PlayUIAnimation(bool turnOn)
    {
        if (turnOn)
        {
            _uiAnimationPlayer.Play("UIFadeIn");
        }
        else
        {
            _uiAnimationPlayer.Play("UIFadeOut");
        }
    }

    
    
    // App Mouse entered/exited
    private void OnUIButtonsControlMouseEntered()
    {
        // If mouse is coming from hovering button, don't play animation and just set to visible.
        if (_buttonHovered)
        {
            _uiAnimationPlayer.Stop();
            _UIButtonsLayer.Visible = true;
        }
        else
        {
            // Else if mouse is coming from outside the app, play animation
            PlayUIAnimation(true);
        }
    }
    
    private void OnUIButtonsControlMouseExited()
    {
        PlayUIAnimation(false);
        _buttonHovered = false;
    }


    
    // Exit Button
    private void OnExitButtonMouseEntered()
    {
        _uiAnimationPlayer.Stop();
        _UIButtonsLayer.Visible = true;
        _exitButton.MouseDefaultCursorShape = CursorShape.PointingHand;
    }
    private void OnExitButtonMouseExited()
    {
        PlayUIAnimation(false);
        _buttonHovered = true;
    }

    
    // FullScreen Button
    private void OnFullScreenButtonMouseEntered()
    {
        _uiAnimationPlayer.Stop();
        _UIButtonsLayer.Visible = true;
        _fullScreenButton.MouseDefaultCursorShape = CursorShape.PointingHand;
    }
    
    private void OnFullScreenButtonMouseExited()
    {
        PlayUIAnimation(false);
        _buttonHovered = true;
    }

    
    // PausePlay Button
    private void OnPausePlayButtonMouseEntered()
    {
        _uiAnimationPlayer.Stop();
        _UIButtonsLayer.Visible = true;
        _pausePlayButton.MouseDefaultCursorShape = CursorShape.PointingHand;
    }

    private void OnPausePlayButtonMouseExited()
    {
        PlayUIAnimation(false);
        _buttonHovered = true;
    }

    
    // Restart Button
    private void OnRestartButtonMouseEntered()
    {
        _uiAnimationPlayer.Stop();
        _UIButtonsLayer.Visible = true;
        _restartButton.MouseDefaultCursorShape = CursorShape.PointingHand;
    }

    private void OnRestartButtonMouseExited()
    {
        PlayUIAnimation(false);
        _buttonHovered = true;
    }
}
