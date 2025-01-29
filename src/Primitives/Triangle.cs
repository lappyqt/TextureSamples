using OpenTK.Graphics.OpenGL4;
using TextureSamples.Common;

namespace TextureSamples.Primitives;

public class Triangle : IPrimitive
{
    public int TexturesCount { get; private set; } 

    private int _vertexBufferObject;
    private int _vertexArrayObject;
    
    private readonly float[] _vertices =
    [
         0.3f, -0.3f, 0.0f, 1.0f, 0.0f,
        -0.3f, -0.3f, 0.0f, 0.0f, 0.0f,
         0.0f,  0.3f, 0.0f, 0.5f, 1.0f
    ];

    private Shader? _shader;
    private Texture[]? _textures;

    public virtual void Load(int vertexBufferObject, int vertexArrayObject)
    {
        _vertexBufferObject = vertexBufferObject;
        _vertexArrayObject = vertexArrayObject;

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _shader = new Shader("Resources/Shaders/textures.vert", "Resources/Shaders/textures.frag");
        _shader.Use();

        GL.BindVertexArray(_vertexArrayObject);

        int vertexLocation = _shader!.GetAttribLocation("vertexPosition");
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(vertexLocation);

        int textureCoord = _shader!.GetAttribLocation("textureCoordinates");
        GL.VertexAttribPointer(textureCoord, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(textureCoord);

        _textures = Texture.LoadTexturesFromDirectory("Resources/Textures/");
        TexturesCount = _textures.Length;

        _shader.SetUniform("texture0", 0);
        _shader.SetUniform("texture1", 1);
        _shader.SetUniform("alpha", 0.2f);
    }

    public virtual void Draw(int textureId = 0)
    {
        _textures![textureId].Use(TextureUnit.Texture0);
        _textures![(textureId < TexturesCount - 1) ? (++textureId) : (TexturesCount - ++textureId)].Use(TextureUnit.Texture1);
        _shader!.Use();

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    public virtual void DeleteShader()
    {
        _shader!.Dispose();
    }
}