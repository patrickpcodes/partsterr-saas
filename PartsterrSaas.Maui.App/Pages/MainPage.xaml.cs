using PartsterrSaas.Maui.App.Models;
using PartsterrSaas.Maui.App.PageModels;

namespace PartsterrSaas.Maui.App.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(MainPageModel model)
	{
		InitializeComponent();
		BindingContext = model;
	}
}