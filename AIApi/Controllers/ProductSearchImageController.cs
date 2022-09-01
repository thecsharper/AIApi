﻿using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AIApi.Events;
using AIApi.Models;
using AIApi.Classifier;
using AIApi.Infrastructure;

using Microsoft.Extensions.Logging;

namespace AIApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductSearchImageController : ProductSearchBase
    {
        private readonly ITensorFlowPredictionStrategy _tensorFlowPredictionStrategy;

        public ProductSearchImageController(ILogger<ProductSearchBase> logger, 
                                            IEventBus eventBus,
                                            ITensorFlowPredictionStrategy tensorFlowPredictionStrategy) :base(logger, eventBus)
        {
            _tensorFlowPredictionStrategy = tensorFlowPredictionStrategy;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ImageClassifierResponse), 200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(415)]
        public async Task<IActionResult> ClassifyImage(IFormFile files)
        {
            if (files.Length == 0)
            {
                return NoContent();
            }

            IEnumerable<string> tags = null;
            using (var image = new MemoryStream())
            {
                await files.CopyToAsync(image);
                var imageData = image.ToArray();
                if (!imageData.IsValidImage())
                {
                    return StatusCode(StatusCodes.Status415UnsupportedMediaType);
                }

                tags = await _tensorFlowPredictionStrategy.ClassifyImageAsync(imageData);
            }

            var integrationEvent = new IntegrationEvent();

            QueueMessageToEventBus(integrationEvent, integrationEvent.Id);

            return Ok(tags);
        }
    }
}
