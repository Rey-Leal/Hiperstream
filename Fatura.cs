// Classe contendo os dados de cada linha da fatura
public class Fatura
{
    public string NomeCliente { get; set; }
    public string Cep { get; set; }
    public string RuaComComplemento { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
    public string ValorFatura { get; set; }
    public string NumeroPaginas { get; set; }

    public Fatura(string nomeCliente, string cep, string ruaComComplemento, string bairro, string cidade, string estado, string valorFatura, string numeroPaginas)
    {
        NomeCliente = nomeCliente;
        Cep = cep;
        RuaComComplemento = ruaComComplemento;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        ValorFatura = valorFatura;
        NumeroPaginas = numeroPaginas;
    }
}
