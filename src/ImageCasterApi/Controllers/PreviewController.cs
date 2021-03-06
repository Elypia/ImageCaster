﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ImageCasterApi.Models.Data;
using ImageCasterApi.Models.Request;
using ImageMagick;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ImageCasterApi.Controllers
{
    [ApiController]
    [Route("preview")]
    public class PreviewController : ControllerBase
    {       
        private readonly ILogger<PreviewController> _logger;

        public PreviewController(ILogger<PreviewController> logger)
        {
            _logger = logger;
        }
        
        [HttpPost("resize")]
        public ActionResult<FrontendFile> ResizePreview([FromBody] ResizePreviewModel model)
        {
            FilterType filter = model.Filter;
            MagickGeometry geometry = model.Geometry;
            
            _logger.LogTrace("HTTP request received at preview/resize with filter of {0}, and geometery of {1}.", filter, geometry);

            using (IMagickImage magickImage = new MagickImage(model.Image.Data))
            {
                magickImage.FilterType = filter;
                magickImage.Resize(geometry);
                return new FrontendFile(model.Image.ContentType, magickImage.ToByteArray());
            }
        }

        [HttpPost("resize-all-filters")]
        public ActionResult<FrontendFile> ResizeAllFilters([FromBody] FilterPreviewModel model)
        {
            MagickGeometry geometry = model.Geometry;
            
            _logger.LogTrace("HTTP request received at preview/resize with geometery of {0}.", geometry);
            
            MontageSettings montageSettings = new MontageSettings()
            {
                BackgroundColor = MagickColors.None,
                Geometry = new MagickGeometry(2, 2, 0, 0),
                TileGeometry = new MagickGeometry(6, 6)
            };

            using (MagickImage magickImage = new MagickImage(model.Image.Data))
            {
                using (MagickImageCollection collection = new MagickImageCollection())
                {
                    Parallel.ForEach((IEnumerable<FilterType>)typeof(FilterType).GetEnumValues(), (filter) =>
                    {
                        MagickImage magickImageClone = (MagickImage)magickImage.Clone();
                        magickImageClone.FilterType = filter;
                        magickImageClone.Resize(geometry);
                        collection.Add(magickImageClone);
                    });

                    using (IMagickImage montage = collection.Montage(montageSettings))
                    {
                        montage.Format = magickImage.Format;
                        return new FrontendFile(model.Image.ContentType, montage.ToByteArray());
                    }
                }
            }
        }
        
        [HttpPost("recolor")]
        public ActionResult<FrontendFile> ModulatePreview([FromBody] RecolorPreviewModel model)
        {
            using (MagickImage magickImage = new MagickImage(model.Image.Data))
            {
                if (model.Mask != null)
                {
                    using (MagickImage magickImageMask = new MagickImage(model.Mask.Data))
                    {
                        magickImage.SetWriteMask(magickImageMask);
                    }
                }

                magickImage.Modulate(model.Brightness, model.Saturation, model.Hue);
                return new FrontendFile(model.Image.ContentType, magickImage.ToByteArray());
            }
        }
    }
}
