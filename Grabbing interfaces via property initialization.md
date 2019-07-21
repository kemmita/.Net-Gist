1. Below is the class and interface
```cs
namespace TestNinja.Mocking
{
    public interface IFileReader
    {
        string Read(string path);
    }
    public class FileReader : IFileReader
    {
        public string Read(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
```
2. And below is how you would inject that into a different class to be utilized.
```cs
namespace TestNinja.Mocking
{
    public class VideoService 
    {
        public IFileReader FileReader { get; set; }

        public VideoService()
        {
            FileReader = new FileReader();
        }
        public string ReadVideoTitle()
        {
            var str = FileReader.Read("video.txt");
            var video = JsonConvert.DeserializeObject<Video>(str);
            if (video == null)
                return "Error parsing the video.";
            return video.Title;
        }
     }
}
```

