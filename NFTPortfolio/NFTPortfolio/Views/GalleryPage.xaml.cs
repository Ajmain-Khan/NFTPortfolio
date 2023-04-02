using NFTPortfolio.Models;
using NFTPortfolio.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NFTPortfolio.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GalleryPage : ContentPage
    {
        private const string __INITIAL_ADDRESS_NFT = "addr1qxh7hefj6sza0xlc4ymg4ytxwa0wc3g6ssmcfwcreg00v45q53wakd0u7hslugnfrmqx4wws0stalfz9u9jhhdnxngnsgwt75u";
        private const string __SAMPLE_ASSET_BITS = "1131301ad4b3cb7deaddbc8f03f77189082a5738c0167e177223309743617264616e6f4269747334343433";
        private const string __SAMPLE_IPFS_HASH = "QmS1PqTjMyX8ybEYY3j1zj7C9z1by3Rij6WBYUNUWmog6J";

        private ObservableCollection<AssetModel> Assets { get; set; }
        // Grouping( Dict: <Collection Name, Policy ID> , List: AssetModel's )
        private ObservableCollection<IGrouping<Dictionary<string, string>, List<AssetModel>>> Collections { get; set; }
        private Dictionary<string, List<AssetModel>> tokenDatabase = new Dictionary<string, List<AssetModel>>();

        public GalleryPage()
        {
            InitializeComponent();
            APIService.InitializeClient();
            LoadImages();

            Assets = new ObservableCollection<AssetModel>();

            //Dictionary<string, List<Models.MetadataModel>> assets = GenerateAssetDict(__INITIAL_ADDRESS_NFT).Result;
            //GenerateImages(assets);

        }

        static async Task<Dictionary<string, List<AssetModel>>> GenerateAssetDict(string address)
        {
            string stakeAddress = await APIService.GetStakeAddress(address);

            List<UnitModel> assetList = new List<UnitModel>();
            for (int i = 1; i <= 1000; i++)
            {
                var listSegment = await APIService.GetAssets(stakeAddress, i);
                if (!listSegment.Count.Equals(0)) assetList.AddRange(listSegment); else break;
            }

            List<string> unitHashes = new List<string>();
            foreach (var unit in assetList) unitHashes.Add(unit.Unit);

            List<AssetModel> assets = await APIService.GetAssetMetadata(unitHashes);

            Dictionary<string, List<AssetModel>> tokenDatabase = new Dictionary<string, List<AssetModel>>();
            foreach (var asset in assets)
            {
                if (tokenDatabase.ContainsKey(asset.Policy_ID))
                {
                    tokenDatabase[asset.Policy_ID].Add(asset);
                }
                else
                {
                    tokenDatabase.Add(asset.Policy_ID, new List<AssetModel> { asset });
                }
            }

            return tokenDatabase;
        }

        void GenerateImages(Dictionary<string, List<Models.AssetModel>> tokenDict)
        {
            /*string pattern = @"[^\/]*[\d\w]$";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            foreach (KeyValuePair<string, List<Models.MetadataModel>> keyValuePair in tokenDict)
            {
                foreach (Models.MetadataModel asset in keyValuePair.Value)
                {
                    try
                    {
                        string imageDirtyURI = asset.Onchain_Metadata.Image;
                        Match imageURI = regex.Match(imageDirtyURI);
                        Uri uri = new Uri("https://ipfs.io/ipfs/" + imageURI.Value);

                        Image image = new Image
                        {
                            Source = ImageSource.FromUri(uri)
                        };
                        FlexGallery.Children.Add(image);
                    }
                    catch
                    {
                        FlexGallery.Children.Add(new Label
                        {
                            Text = "Error retreiving image!"
                        });
                    }
                }
            }*/
            Regex regex = new Regex(@"\b(\w{46})\b");
            foreach (KeyValuePair<string, List<AssetModel>> kvp in tokenDatabase)
            {
                foreach (AssetModel asset in kvp.Value)
                {
                    try
                    {
                        if (asset.Onchain_Metadata == null)
                        {
                            FlexGallery.Children.Add(new Label
                            {
                                Text = "Error: No Metadata for asset"
                            });
                            //Console.WriteLine($"No Metadata for asset: {asset.Policy_ID}{asset.Asset_Name}");
                        }
                        else if (!(asset.Onchain_Metadata.Image == null))
                        {
                            var match = regex.Match(asset.Onchain_Metadata.Image);
                            Image image = new Image
                            {
                                Source = ImageSource.FromUri(new Uri("https://ipfs.io/ipfs/" + match.Value))
                            };
                            FlexGallery.Children.Add(image);
                        }
                        else if (!(asset.Onchain_Metadata.Asset == null))
                        {
                            var match = regex.Match(asset.Onchain_Metadata.Asset.Ipfs);
                            Image image = new Image
                            {
                                Source = ImageSource.FromUri(new Uri("https://ipfs.io/ipfs/" + match.Value)),
                                IsAnimationPlaying = true
                            };
                            FlexGallery.Children.Add(image);
                        }
                        else
                        {
                            FlexGallery.Children.Add(new Label
                            {
                                Text = "Error: Cannot retrieve data for token"
                            });
                            //Console.WriteLine($"ERROR - Object Mismatch - Cannot retrieve data for token: {asset.Policy_ID}{asset.Asset_Name}");
                        }
                    }
                    catch (Exception e)
                    {
                        FlexGallery.Children.Add(new Label
                        {
                            Text = $"Error: {e}"
                        });
                    }
                }
            }
        }

        private async Task LoadImages()
        {
            string stakeAddress = await APIService.GetStakeAddress(__INITIAL_ADDRESS_NFT);

            List<UnitModel> assetList = new List<UnitModel>();
            for (int i = 1; i <= 1000; i++)
            {
                var listSegment = await APIService.GetAssets(stakeAddress, i);
                if (!listSegment.Count.Equals(0)) assetList.AddRange(listSegment); else break;
            }

            List<string> unitHashes = new List<string>();
            foreach (var unit in assetList) unitHashes.Add(unit.Unit);

            List<AssetModel> assets = await APIService.GetAssetMetadata(unitHashes);

            foreach (var asset in assets)
            {
                if (tokenDatabase.ContainsKey(asset.Policy_ID))
                {
                    tokenDatabase[asset.Policy_ID].Add(asset);
                }
                else
                {
                    tokenDatabase.Add(asset.Policy_ID, new List<AssetModel> { asset });
                }
            }

            GenerateImages(tokenDatabase);
        }
    }
}