using Minhas_Compras.Models;

namespace Minhas_Compras.View;

public partial class EditarProduto : ContentPage
{
    public EditarProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked_Editar(object sender, EventArgs e)
    {
        try
        {

            Produto produto_contexto = BindingContext as Produto;
            Produto p = new Produto()
            {
                
                Descricao = txt_descricao.Text,
                Preco = Convert.ToDouble(txt_preco.Text),
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Id = produto_contexto.Id,
            };

            await App.Db.Update(p);
            await DisplayAlertAsync("Sucesso", $"Produto {txt_descricao.Text} alterado com sucesso!", "OK");
            Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", "A alteração do produto falhou", "OK");

        }
    }
}