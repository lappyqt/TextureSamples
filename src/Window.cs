using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using TextureSamples.Primitives;

namespace TextureSamples;

public class Window : GameWindow
{
    private int _vertexBufferObject;
    private int _elementBufferObject;
    private int _vertexArrayObject;
    private int _textureId = 0;

    private readonly Rectangle _rectangle;

    public Window(int width, int height, string title): base(new GameWindowSettings(), new NativeWindowSettings { ClientSize = (width, height), Title = title })
    {
        _rectangle = new Rectangle(0.22f, 0.4f);
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        GL.ClearColor(Color4.Black);

        _vertexBufferObject = GL.GenBuffer();
        _elementBufferObject = GL.GenBuffer();
        _vertexArrayObject = GL.GenVertexArray();

        _rectangle.Load(_vertexBufferObject, _elementBufferObject, _vertexArrayObject);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit);

        _rectangle.Draw(_textureId);

        SwapBuffers();
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key >= Keys.D1 && e.Key <= Keys.D9 - (9 - _rectangle.TexturesCount)) {
            _textureId = e.Key - Keys.D1;
        }
    }

    protected override void OnFramebufferResize(FramebufferResizeEventArgs e)
    {
        base.OnFramebufferResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }

    protected override void OnUnload()
    {
        base.OnUnload();

        _rectangle.DeleteShader();

        GL.DeleteBuffer(_vertexBufferObject);
        GL.DeleteBuffer(_elementBufferObject);
        GL.DeleteVertexArray(_vertexArrayObject);
    }
}