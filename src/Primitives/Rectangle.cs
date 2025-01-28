using OpenTK.Graphics.OpenGL4;
using TextureSamples.Common;

namespace TextureSamples.Primitives;

public class Rectangle : IPrimitive
{
    public int TexturesCount;

    private int _vertexBufferObject;
    private int _elementBufferObject; 
    private int _vertexArrayObject;

    private readonly float[] _vertices;

    private readonly uint[] _indices = 
    [
        0, 1, 2,
        2, 3, 1
    ];

    public Rectangle(float x = 0.5f, float y = 0.5f, float z = 0.0f)
    {
        _vertices =
        [
            -x, y, z,  0.0f, 1.0f,
            x, y, z,   1.0f, 1.0f,
            -x, -y, z, 0.0f, 0.0f,
            x, -y, z,  1.0f, 0.0f
        ];
    }

    private Shader? _shader;
    private Texture[]? _textures;

    public virtual void Load(int VertexBufferObject, int ElementBufferObject, int VertexArrayObject)
    {
        _vertexBufferObject = VertexBufferObject;
        _elementBufferObject = ElementBufferObject;
        _vertexArrayObject = VertexArrayObject;

        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

        _shader = new Shader("Resources/Shaders/textures.vert", "Resources/Shaders/textures.frag");

        GL.BindVertexArray(_vertexArrayObject);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);

        int vertexLocation = _shader!.GetAttribLocation("vertexPosition");
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
        GL.EnableVertexAttribArray(vertexLocation);

        int textureCoord = _shader!.GetAttribLocation("textureCoordinates");
        GL.VertexAttribPointer(textureCoord, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));
        GL.EnableVertexAttribArray(textureCoord);

        _textures = Texture.LoadTexturesFromDirectory("Resources/Textures/");
        TexturesCount = _textures.Length;
    }

    public virtual void Draw(int textureId = 0)
    {
        _shader!.Use();
        _textures![textureId].Use(TextureUnit.Texture0);

        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawElements(BeginMode.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
    }

    public virtual void DeleteShader()
    {
        _shader!.Dispose();
    }
}