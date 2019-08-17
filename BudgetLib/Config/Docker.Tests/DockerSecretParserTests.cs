using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using Xunit;

namespace Config.Docker.Tests
{
    public class DockerSecretParserTests
    {
        private readonly string DOCKER_SECRET_PATH = "/run/secrets/";

        [Fact]
        public void Constructor_with_NoDependencies_returns_Controller()
        {
            var result = new DockerSecretParser();

            Assert.NotNull(result);
        }

        [Fact]
        public void Constructor_with_Dependencies_returns_Controller()
        {

            var result = new DockerSecretParser(new FileSystem());

            Assert.NotNull(result);
        }

        [Fact]
        public void Parse_where_NoSecretsExist_returns_EmptyDictionary()
        {
            // where .Exists always returns false 
            var emptyFileSystem = new MockFileSystem();

            var parser = new DockerSecretParser(emptyFileSystem);

            var result = parser.Parse();

            Assert.Empty(result);
        }

        [Fact]
        public void Parse_where_DockerSecretExists_returns_DotNetConfigValues()
        {
            var shallowKey = "ShallowKey";
            var shallowValue = "---shallow-value---";

            var nestedDockerKey = "Nested_Key";
            var nestedDotnetKey = "Nested:Key";
            var nestedValue = "---nested-value---";

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {string.Concat(DOCKER_SECRET_PATH, shallowKey), new MockFileData(shallowValue)},
                {string.Concat(DOCKER_SECRET_PATH, nestedDockerKey), new MockFileData(nestedValue)}
            });

            var parser = new DockerSecretParser(fileSystem);

            var result = parser.Parse();

            Assert.Equal(shallowValue, result[shallowKey]);
            Assert.Equal(nestedValue, result[nestedDotnetKey]);
        }
    }
}
