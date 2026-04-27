using Minhas_Compras.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Minhas_Compras.View;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    List<string> categorias;
    public ListaProduto()
    {
        InitializeComponent();
        lst_produtos.ItemsSource = lista;
     
    }

    String _categoria = "";
     void atualizaCategorias()
    {
        

        categorias = new List<string>(
                lista.Select(i => i.Categoria).Where(i=>!String.IsNullOrEmpty(i)).Distinct()
                      .OrderBy(i => i)
                );
        categorias.Insert(0, "Todas");
        lst_categoria.ItemsSource = categorias;
        Debug.WriteLine(_categoria);

        if (!string.IsNullOrEmpty(_categoria) && categorias.Contains(_categoria))
        {
            lst_categoria.SelectedItem = _categoria;
        }
        else
        {
            lst_categoria.SelectedIndex = 0;
        }

    }

    protected async override void OnAppearing()
    {
        try
        {
            lista?.Clear();
            List<Produto> tmp = await App.Db.GetAll();
            foreach(var i in tmp)
            {
                lista?.Add(i);
            }
            atualizaCategorias();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");

        }
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

    private async Task executa_busca ( CancellationToken token)
    {

        if (token.IsCancellationRequested) return;

        string produto = txt_search.Text;
        string categoria = lst_categoria.SelectedItem.ToString();
        categoria = categoria == "Todas" ? null : categoria;
        _categoria = categoria;

        lista.Clear();
        List<Produto> tmp = await App.Db.Search(produto, categoria);
        foreach (var i in tmp)
        {
            lista.Add(i);
        }
       
    }
    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        try
        {
            await Task.Delay(500, _cts.Token);
            await executa_busca(_cts.Token);
          
        }
        catch (TaskCanceledException) { return; }


    }
    private async void lst_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        try
        {
            await Task.Delay(500, _cts.Token);

            await executa_busca(_cts.Token);
        }
        catch (TaskCanceledException) { return; }

    }

    private void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
    {
        double soma = lista.Sum<Produto>(i => i.Total);

        string msg = $"A total é: {soma:C2}";

        DisplayAlertAsync("Total dos produtos", msg, "OK");
    }
    private async void ToolbarItem_Clicked_SomarCat(object sender, EventArgs e)
    {
        string msg = "";
        List<Produto> tmp = await App.Db.GetAll();
        double total = 0;
        foreach (var i in tmp)
        {
            total += i.Total;
        }

        lista.GroupBy(i => i.Categoria)
             .Select(g => new { Categoria = g.Key, Total = g.Sum(i => i.Total) })
             .ToList()
             .ForEach(i =>
             {
                 var percentual = 100*(i.Total / total);
                  msg += $"Os gastos com {i.Categoria} - Total: {i.Total:C2} ({percentual:f2}%) \n";
                

             });
        await DisplayAlertAsync("Total", msg, "Ok");
    }
    private async void MenuItem_Clicked_Remover(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;
            Produto p = selecionado.BindingContext as Produto;
            bool confirm = await DisplayAlertAsync("Confirmação", $"Deseja remover o produto {p.Descricao}?", "Sim", "Não");
            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
            
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }

    private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            Navigation.PushAsync(new View.EditarProduto { BindingContext = p });
        }
        catch (Exception ex)
        {
            DisplayAlertAsync("Erro", ex.Message, "OK");

        }
    }

    
}