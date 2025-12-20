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
        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        slist.ShopID = selectedShop.ID;
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
        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = (System.Collections.IList)items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");
        var shopl = (ShopList)BindingContext;
        if (shopl.ShopID != 0 && items != null)
        {
            ShopPicker.SelectedItem = items.FirstOrDefault(x => x.ID == shopl.ShopID);
        }
        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
}