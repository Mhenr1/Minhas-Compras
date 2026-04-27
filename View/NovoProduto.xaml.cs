
using Minhas_Compras.Models;

namespace Minhas_Compras.View;

public partial class NovoProduto : ContentPage
{
    public NovoProduto()
    {
        InitializeComponent();
    }

    private async void ToolbarItem_Clicked_Salvar(object sender, EventArgs e)
    {
        try
        {

            string novoProduto = txt_descricao.Text;
            
            Produto p = new Produto
            {
                Descricao = novoProduto,
                Quantidade = Convert.ToDouble(txt_quantidade.Text),
                Preco = Convert.ToDouble(txt_preco.Text),
                Categoria = txt_categoria.Text
            };

            await App.Db.Insert(p);
            await DisplayAlertAsync("Sucesso", $"O produto {novoProduto} foi registrado com sucesso", "Ok");
            LimparFormulario();

        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Erro", ex.Message, "OK");
        }
    }
    void LimparFormulario()
    {
        txt_descricao.Text = string.Empty;
        txt_categoria.Text = string.Empty;
        txt_quantidade.Text = string.Empty;
        txt_preco.Text = string.Empty;

     txt_descricao.Focus();
    }
}