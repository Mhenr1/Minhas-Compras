using Minhas_Compras.Models;
using System.Collections.Specialized;

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
            string descricao = txt_descricao.Text;
           if( String.IsNullOrWhiteSpace(descricao) )
            { 
                await DisplayAlertAsync("Erro", "Por favor, informe a descrição do produto", "OK");

                return;


            }


            Produto produto_contexto = BindingContext as Produto;
            Produto p = new Produto()
            {
                

                Descricao = descricao,
                Preco = Convert.ToDouble(txt_preco.Text),
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Id = produto_contexto.Id,
                Categoria = txt_categoria.Text
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