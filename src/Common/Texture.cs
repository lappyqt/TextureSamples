using StbImageSharp;
using OpenTK.Graphics.OpenGL4;

namespace TextureSamples.Common;

public class Texture(int handle)
{
    private int Handle = handle;

    public void Use(TextureUnit unit = TextureUnit.Texture0)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, Handle);
    }

    public static Texture LoadFromFile(string texturePath)
    {
        int handle = GL.GenTexture();

        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);

        StbImage.stbi_set_flip_vertically_on_load(1);

        ImageResult image = ImageResult.FromStream(File.OpenRead(texturePath), ColorComponents.RedGreenBlueAlpha);
        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
 
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int) TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int) TextureMagFilter.Nearest);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        return new Texture(handle);
    }

    public static Texture[] LoadTexturesFromDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            throw new Exception($"No such directory: {directoryPath}");
        }

        var texturePaths = Directory.GetFiles(directoryPath);
        var textures = new Texture[texturePaths.Length];

        for (int i = 0; i < texturePaths.Length; i++) 
        {
            textures[i] = LoadFromFile(texturePaths[i]);
        }

        return textures;
    }
}