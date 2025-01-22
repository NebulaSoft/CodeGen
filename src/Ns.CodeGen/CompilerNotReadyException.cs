namespace Ns.CodeGen
{
    public sealed class CompilerNotReadyException : CodeGenException
    {
        public CompilerNotReadyException() 
            : base($"You must call {nameof(CSharpCompiler)}.{nameof(CSharpCompiler.Begin)} before compile.")
        {
        }
    }
}
