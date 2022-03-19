using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;

namespace Api.Controllers 
{
    [Route("api/featuretoggles")]
    public class FeatureFlagController
    {
        private IFeatureManager _featureManager;
        private ILogger<FeatureFlagController> _logger;
        
        public FeatureFlagController(IFeatureManager featureManager, ILogger<FeatureFlagController> logger)
        {
            _featureManager = featureManager;
            _logger = logger;
        }

        [HttpGet]
        public IAsyncEnumerable<string> GetFeatureToggles()
        {
            return _featureManager.GetFeatureNamesAsync();
        }

        [HttpGet("{name}")]
        public async Task<bool> GetFeatureFlagState(string name)
        {
            return await _featureManager.IsEnabledAsync(name);
        }
    }
}