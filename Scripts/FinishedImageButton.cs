using Godot;

namespace FigurePhotosApp.Scripts;

public partial class FinishedImageButton : TextureButton
{

    public void Initialize(Image image)
    {
        TextureNormal = ImageTexture.CreateFromImage(image);

        // Re-size Button
        CustomMinimumSize = new Vector2(0, 120);
    }
}