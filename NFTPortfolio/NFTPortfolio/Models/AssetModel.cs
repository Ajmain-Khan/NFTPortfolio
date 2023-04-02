using System;

namespace NFTPortfolio.Models
{
    class AssetModel
    {
        public string Policy_ID { get; set; }
        public string Asset_Name { get; set; }
        public string Fingerprint { get; set; }
        public ulong Quantity { get; set; }
        public string Initial_Mint_TX_Hash { get; set; }
        public OnchainMetadataModel Onchain_Metadata { get; set; }
        //public string Metadata { get; set; }
    }
}
