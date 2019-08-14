using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;

namespace Config.Docker
{
    public class DockerSecretParser
    {
        const string DOCKER_SECRET_PATH = "/run/secrets/";

        private readonly bool SecretsExists;
        private readonly IFileProvider _provider;
        private readonly IDictionary<string, string> _data = new SortedDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        public DockerSecretParser()
        {
            // if there are no secrets set, let's forget the whole thing
            SecretsExists = Directory.Exists(DOCKER_SECRET_PATH);

            if (SecretsExists)
            {
                _provider = new PhysicalFileProvider(DOCKER_SECRET_PATH);
            }

        }
        public IDictionary<string, string> Parse()
        {
            _data.Clear();
            if (!SecretsExists) return _data;

            var contents = _provider.GetDirectoryContents("");
            var directoryInfo = contents.GetEnumerator();

            while (directoryInfo.MoveNext())
            {
                var info = directoryInfo.Current;
                if (info.Exists)
                {
                    var underscoreKey = info.Name;
                    using (var stream = info.CreateReadStream())
                    using (var streamReader = new StreamReader(stream))
                    {
                        // convert Docker underscore_key to dotnet colon:key
                        var value = streamReader.ReadToEnd();
                        var colonKey = underscoreKey.Replace('_', ':');
                        _data.Add(colonKey, value);
                    }
                }
            }


            return _data;
        }
    }
}