// Classe contendo os dados de cada linha da fatura
public class Faturas
{
    public string NomeCliente { get; set; }
    public string Cep { get; set; }
    public string RuaComComplemento { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public double ValorFatura { get; set; }
    public int NumeroPaginas { get; set; }

    public Faturas(string nomeCliente, string cep, string ruaComComplemento, string bairro, string cidade, string estado, double valorFatura, int numeroPaginas)
    {
        NomeCliente = nomeCliente;
        Cep = cep.Trim();
        RuaComComplemento = ruaComComplemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        ValorFatura = valorFatura;
        NumeroPaginas = numeroPaginas;
    }
}
