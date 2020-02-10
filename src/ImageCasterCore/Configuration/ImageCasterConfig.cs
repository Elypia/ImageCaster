using System;
using System.Collections.Generic;
using System.IO;
using ImageCasterCore.Configuration.Converters;
using ImageCasterCore.Configuration.NodeDeserializers;
using ImageCasterCore.Extensions;
using NLog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace ImageCasterCore.Configuration
{
    public class ImageCasterConfig
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        
        /// <summary>The default file name for the configuration.</summary>
        private const string DefaultConfigFileName = "imagecaster.yml";

        public Export Export { get; set; }

        public Checks Checks { get; set; }

        public List<PatternConfig> Montages { get; set; }

        public List<Archive> Archives { get; set; }

        /// <summary>Overload of <see cref="Load"/> that loads a file.</summary>
        /// <param name="path">The path to the file which represents the configuration.</param>
        /// <returns>A data object that represents the configuration passed.</returns>
        public static ImageCasterConfig LoadFromFile(string path = DefaultConfigFileName)
        {
            FileInfo fileInfo = new FileInfo(path);

            if (!fileInfo.Exists)
            {
                Logger.Fatal("No configuration file found or specified.");
            }
            
            using (StreamReader reader = new StreamReader(path))
            {
                return Load(reader);
            }
        }
        
        /// <summary>Overload of <see cref="Load"/> that loads a string.</summary>
        /// <param name="config">The literal string to use to load the configuration.</param>
        /// <returns>A data object that represents the configuration passed.</returns>
        public static ImageCasterConfig LoadFromString(string config)
        {
            using (StringReader reader = new StringReader(config))
            {
                return Load(reader);
            }
        }

        /// <summary>Load the configuration from the provided <see cref="TextReader"/>.</summary>
        /// <param name="reader">The text reader to read the string from for the configuration.</param>
        /// <returns>A data object that represents the configuration passed.</returns>
        public static ImageCasterConfig Load(TextReader reader)
        {
            reader.RequireNonNull();
            IDeserializer deserializer = new DeserializerBuilder()
                .WithNamingConvention(HyphenatedNamingConvention.Instance)
                .WithNodeDeserializer((inner) => new ValidationNodeDeseralizer(inner), s => s.InsteadOf<ObjectNodeDeserializer>())
                .WithTypeConverter(new ExifTagConverter())
                .WithTypeConverter(new FileInfoConverter())
                .WithTypeConverter(new MagickGeometryConverter())
                .WithTypeConverter(new ModulateConverter())
                .WithTypeConverter(new RegexConverter())
                .Build();

            return deserializer.Deserialize<ImageCasterConfig>(reader);
        }
    }
}