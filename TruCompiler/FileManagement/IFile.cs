namespace TruCompiler.FileManagement
{
    public interface IFile
    {
        public abstract void Write(string OutputPath, string content);
        public abstract string Read(string ReadingPath);
    }
}