using CosmaSimonaLab7.Models;

namespace CosmaSimonaLab7;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList) this.BindingContext)
        {
            BindingContext = new Product()
        });
    }
    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        var product = listView.SelectedItem as Product;
        if (product != null)
        {
            var shopl = (ShopList)this.BindingContext;
            await App.Database.DeleteListProductAsync(shopl.ID, product.ID);
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
        }
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;
        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
}