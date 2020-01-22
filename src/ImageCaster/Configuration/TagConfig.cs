using ImageCaster.Extensions;
using YamlDotNet.Serialization;

namespace ImageCaster.Configuration
{
    /// <summary>
    /// An Exif (Exchangable Image Format) key:value pair to set while
    /// exporting images.
    /// </summary>
    public class TagConfig
    {
        /// <summary>The name of the valid Exif tag according to the 2.31 standard.</summary>
        public string Tag { get; set; }
        
        /// <summary>The value to assign to this Exif tag.</summary>
        public string Value { get; set; }

        public TagConfig()
        {
            // Do nothing
        }

        public TagConfig(string tag, string value = null)
        {
            this.Tag = tag.RequireNonNull();
            this.Value = value;
        }
    }
}