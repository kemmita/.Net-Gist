1. TO pass an iunterface in via a class cvonstructor, we can use a privater field along with the class consturcotr. 
```cs
namespace TestNinja.Mocking
{
    public class VideoService
    {
        private IFileReader _fileReader;
        public VideoService(IFileReader fileReader = null)
        {
            _fileReader = fileReader ?? new FileReader();
        }
        public string ReadVideoTitle()
        {
            var str = _fileReader.Read("C:/ConsoleAppTest/Program.cs");
            var video = JsonConvert.DeserializeObject<Video>(str);
            if (video == null)
                return "Error parsing the video.";
            return video.Title;
        }
    }
}
```
