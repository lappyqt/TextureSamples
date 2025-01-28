namespace TextureSamples.Common;

public class IndividualShaders(int vertexShader, int fragmentShader) : Tuple<int, int>(vertexShader, fragmentShader)
{
    public int VertexShader => Item1;
    public int FragmentShader => Item2;
}