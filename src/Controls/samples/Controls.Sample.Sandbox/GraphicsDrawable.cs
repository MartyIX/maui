namespace Maui.Controls.Sample;

public class GraphicsDrawable(Microsoft.Maui.Graphics.IImage[] images) : IDrawable
{
	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		// 3x3 matrix composed of 100x100 images. The number of images is always 9. 
		for (int i = 0; i < images.Length; i++)
		{
			int row = i / 3;
			int column = i % 3;

			canvas.DrawImage(images[i], column * 100, row * 100, 100, 100);
		}		
	}
}