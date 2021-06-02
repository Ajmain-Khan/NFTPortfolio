using NFTPortfolio.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace NFTPortfolio.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}