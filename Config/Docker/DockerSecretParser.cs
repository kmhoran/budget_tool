using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Config.Docker
{
    public class DockerSecretParser
    {

        public DockerSecretParser()
        {
            // if there are no secrets set, let's forget the whole thing
            SecretsExists = File.Exists(DOCKER_SECRET_PATH);

            // TODO: remove shameless console logs
            Console.WriteLine($"SECRET EXISTS: {SecretsExists}");
            if (SecretsExists)
            {
                _provider = new PhysicalFileProvider(DOCKER_SECRET_PATH);
            }

        }
        const string DOCKER_SECRET_PATH = "/run/secrets/";
        private bool SecretsExists { get; }
        private readonly IFileProvider _provider;


        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public IDictionary<string, string> Parse()
        {
            _data.Clear();
            if (!SecretsExists) return _data;

            if (Directory.Exists(DOCKER_SECRET_PATH))
            {
                // TODO: do something real
                var contents = _provider.GetDirectoryContents("");
                var directoryInfo = contents.GetEnumerator();
                Console.WriteLine("SECRET VALUES:");
                while (directoryInfo.MoveNext())
                {
                    var info = directoryInfo.Current;
                    if (info.Exists)
                    {
                        var key = info.Name;
                        using (var stream = info.CreateReadStream())
                        using (var streamReader = new StreamReader(stream))
                        {
                            var value = streamReader.ReadToEnd();
                            Console.WriteLine($"    {key} : {value}");
                        }
                    }
                }
            }

            _data.Clear();

            // TODO: remove this example
            _data.Add("GoogleConfig:ServiceEmail", "test@e.com");
            _data.Add("GoogleConfig:PrivateKey", "TEST_KEY");
            _data.Add("GoogleConfig:EnvironmentName", "hard_coded_from_Docker_parser");
            return _data;
        }
    }
}