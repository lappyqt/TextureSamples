using OpenTK.Graphics.OpenGL4;

namespace TextureSamples.Common;

public class Shader : IDisposable
{
    private bool _disposed;
    public readonly int Handle;

    public Shader(string vertexShaderPath, string fragmentShaderPath)
    {
        Handle = GL.CreateProgram();

        var shaders = Initialize(vertexShaderPath, fragmentShaderPath);
        CompileShaders(shaders);
    }

    ~Shader()
    {
        if (_disposed == false)
        {
            Console.WriteLine("GPU Resource Leak. Possible missing Dispose() call.");
        }
    }

    public void Use() => GL.UseProgram(Handle);

    private IndividualShaders Initialize(string vertexShaderPath, string fragmentShaderPath)
    {
        string vertexShaderSource = File.ReadAllText(vertexShaderPath);
        string fragmentShaderSource = File.ReadAllText(fragmentShaderPath);

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);

        GL.ShaderSource(vertexShader, vertexShaderSource);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);

        return new (vertexShader, fragmentShader);
    }

    private void CompileShaders(IndividualShaders shaders)
    {
        CompileShader(shaders.VertexShader);
        CompileShader(shaders.FragmentShader);

        GL.AttachShader(Handle, shaders.VertexShader);
        GL.AttachShader(Handle, shaders.FragmentShader);

        GL.LinkProgram(Handle);
        GL.GetProgram(Handle, GetProgramParameterName.LinkStatus, out int status);

        if (status == 0) 
        {
            throw new Exception(GL.GetProgramInfoLog(Handle));
        }

        GL.DetachShader(Handle, shaders.VertexShader);
        GL.DetachShader(Handle, shaders.FragmentShader);

        GL.DeleteShader(shaders.VertexShader);
        GL.DeleteShader(shaders.FragmentShader);
    }

    private void CompileShader(int shader)
    {
        GL.CompileShader(shader);
        GL.GetShader(shader, ShaderParameter.CompileStatus, out int status);

        if (status == 0) 
        {
            throw new Exception(GL.GetShaderInfoLog(shader));
        }
    }

    public void SetUniform(string name, int value)
    {
        int location = GL.GetUniformLocation(Handle, name);
        GL.Uniform1(location, value);
    }

    public void SetUniform(string name, float value)
    {
        int location = GL.GetUniformLocation(Handle, name);
        GL.Uniform1(location, value);
    }

    public int GetAttribLocation(string attribName)
    {
        return GL.GetAttribLocation(Handle, attribName);
    }

    public void Dispose()
    {
        GL.DeleteProgram(Handle);
        _disposed = true;
        
        GC.SuppressFinalize(this);
    }
}