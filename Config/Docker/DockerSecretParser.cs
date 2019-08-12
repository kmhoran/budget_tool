using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Config.Docker
{
public class DockerSecretParser
    {
        const string DOCKER_SECRET_PATH = "/run/secrets/";
        private readonly IFileProvider _provider = new PhysicalFileProvider(DOCKER_SECRET_PATH);

        
        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public IDictionary<string, string> Parse()
        {

if(Directory.Exists(DOCKER_SECRET_PATH))
{
var contents = _provider.GetDirectoryContents("");
var directoryInfo = contents.GetEnumerator();
Console.WriteLine("SECRET VALUES:");
while(directoryInfo.MoveNext())
{
var info = directoryInfo.Current;
if(info.Exists)
{
    var key = info.Name;
    using (var stream = info.CreateReadStream())
    using(var streamReader = new StreamReader(stream))
    {
      var value = streamReader.ReadToEnd();
      Console.WriteLine($"    {key} : {value}");
    } 
}
}
}

            _data.Clear();
            _data.Add("GoogleConfig:ServiceEmail", "test@e.com");
            _data.Add("GoogleConfig:PrivateKey", "TEST_KEY");
            _data.Add("GoogleConfig:EnvironmentName", "hard_coded_from_Docker_parser");
            return _data;
        }
    }
}