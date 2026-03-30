using Minhas_Compras.Models;
using System.Collections.ObjectModel;

namespace Minhas_Compras.View;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        List<Produto> tmp = await App.Db.GetAll();
        tmp.ForEach(i => lista.Add(i));
    }
    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new View.NovoProduto());
        }
        catch (Exception ex)
        {
            DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }
    CancellationTokenSource _cts = new CancellationTokenSource();

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        try
        {
            await Task.Delay(500, _cts.Token);

            string q = txt_search.Text;
            lista.Clear();
            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (TaskCanceledException) { }


    }

    private void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
    {
        double soma = lista.Sum<Produto>(i => i.Total);

        string msg = $"A total é: {soma:C2}";

        DisplayAlertAsync("Total dos produtos", msg, "OK");
    }

    private async void MenuItem_Clicked_Remover(object sender, EventArgs e)
    {
        try {
            MenuItem selecionado = sender as MenuItem;
            Produto p = selecionado.BindingContext as Produto;
            bool confirm = await DisplayAlertAsync("Confirmação", $"Deseja remover o produto {p.Descricao}?", "Sim", "Não");
            if (confirm) { 
            await App.Db.Delete(p.Id);
                lista.Remove(p);
            }

        }
        catch(Exception ex ){
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }
}