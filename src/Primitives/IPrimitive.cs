namespace TextureSamples.Primitives;

public interface IPrimitive
{
    void Draw(int textureId = 0);
    void DeleteShader();
}