namespace SkyStruct.FileGenerator;

public interface IFileGenerator;

public class FileGenerator : IFileGenerator
{
    
    public FileGenerator(){}

    public async void GenerateFile(string fileData, string fileName)
    {
        using var outputFile = new StreamWriter(fileName);
        await outputFile.WriteAsync(fileData);
    }
    
}
