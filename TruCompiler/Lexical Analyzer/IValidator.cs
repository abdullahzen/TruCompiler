namespace TruCompiler.Lexical_Analyzer
{
    public interface IValidator<T>
    {
        public T Validate(string value, int i, string type);
    }
}