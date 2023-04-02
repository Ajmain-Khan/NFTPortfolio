using System;

namespace NFTPortfolio.Models
{
    class OnchainMetadataModel
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public OnchainMetadataAssetModel Asset { get; set; }
    }
}
