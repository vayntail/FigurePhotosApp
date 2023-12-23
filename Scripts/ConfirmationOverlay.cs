using Godot;

namespace FigurePhotosApp.Scripts;

public partial class ConfirmationOverlay : Control
{
    [Export] private AnimationPlayer _uiAnimationPlayer;
    [Export] private Timer _fadeOutTimer;
    [Export] private Label _messageLabel;
    [Export] private Button _button1;
    [Export] private Button _button2;
    [Export] private TextureButton _xButton;

    [Signal] public delegate void ConfirmedEventHandler();
    [Signal] public delegate void CanceledEventHandler();

    private bool _confirmed;

    public override void _Ready()
    {
        _uiAnimationPlayer.Play("FadeIn");
    }

    public void Initialize(string message, string button1, string button2)
    {
        _messageLabel.Text = message;

        _button1.Text = button1;

        
        if (button2 == null)
        {
            _button2.Visible = false;
        }
        else
        {
            _button2.Text = button2;
        }
    }

    private void OnXButtonPressed()
    {
        _uiAnimationPlayer.Play("FadeOut");
        _fadeOutTimer.Start();
        _confirmed = false;
    }

    private void OnButton1Pressed()
    {
        _uiAnimationPlayer.Play("FadeOut");
        _fadeOutTimer.Start();
        _confirmed = true;
    }

    private void OnButton2Pressed()
    {
        _uiAnimationPlayer.Play("FadeOut");
        _fadeOutTimer.Start();
        _confirmed = false;
    }

    private void OnFadeOutTimerTimeOut()
    {
        if (_confirmed)
        {
            EmitSignal(SignalName.Confirmed); 
        }
        else
        {
            EmitSignal(SignalName.Canceled); 
        }
        QueueFree();
    }
    
    // UI stuff
    // 
    private void OnXButtonMouseEntered()
    {
        _xButton.MouseDefaultCursorShape = CursorShape.PointingHand;
    }
}