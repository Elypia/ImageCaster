using ImageMagick;

namespace ImageCasterApi.Models.Request
{
    /// <summary>Data object to map formdata from request.</summary>
    public class ResizePreviewModel : FilterPreviewModel
    {
        /// <summary>The filter to use when resizing images.</summary>
        public FilterType Filter { get; set; }
    }
}
