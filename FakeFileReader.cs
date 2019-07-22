using System;
using TestNinja.Mocking;

namespace TestNinja.Tests
{
  
    public class FakeFileReader : IFileReader
    {
        public string Read(string path)
        {
            return "file_path//..";
        }
    }
}
