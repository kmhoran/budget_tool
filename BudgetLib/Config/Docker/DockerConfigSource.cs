using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Config.Docker
{
    public class DockerConfigSource: IConfigurationSource
    {
        public bool Optional { get; set; }
        public bool ReloadOnChange { get; set; }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DockerConfigProvider(this);
        }
    }
}
