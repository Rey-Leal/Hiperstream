
using System.Runtime.ConstrainedExecution;
using System.Text;

public class Validacoes
{
    // Realiza leitura e processamento do arquivo de faturas
    public bool ProcessarFaturas(string arquivoDeFaturas)
    {
        try
        {
            bool processouFaturas = false;
            string linhaDaFatura;
            string[] dadosDaFatura;
            string[] caminhoCompleto = arquivoDeFaturas.Split('\\');
            string diretorioDeSaida = Path.Combine(string.Join("\\", caminhoCompleto, 0, caminhoCompleto.Length - 1), "arquivosDeSaida");

            // Verifica existencia do arquivo de faturas
            bool existeArquivo = File.Exists(arquivoDeFaturas);

            if (existeArquivo)
            {
                // Cria arquivos de saida no mesmo diretorio informado da base de dados "base_hi.txt"                
                CriaArquivosDeSaida(diretorioDeSaida);

                // Realiza leitura do arquivo de faturas
                using (StreamReader leitor = new StreamReader(arquivoDeFaturas))
                {
                    while ((linhaDaFatura = leitor.ReadLine()) != null)
                    {
                        dadosDaFatura = linhaDaFatura.Split(';');

                        // Considera apenas linhas com campos numericos nos campos ValorFatura e NumeroPaginas
                        if (Double.TryParse((dadosDaFatura[6].Replace(".", ",")), out _) && Int32.TryParse((dadosDaFatura[7]), out _))
                        {
                            Faturas fatura = new Faturas(dadosDaFatura[0], dadosDaFatura[1], dadosDaFatura[2], dadosDaFatura[3], dadosDaFatura[4], dadosDaFatura[5], Double.Parse(dadosDaFatura[6].Replace(".", ",")), Int32.Parse(dadosDaFatura[7]));

                            // Realiza validacao da fatura
                            if (ValidarFaturas(fatura))
                            {
                                // Prossegue com gravacao em arquivos caso a fatura seja valida 
                                if (GravarFaturas(diretorioDeSaida, fatura))
                                {
                                    processouFaturas = true;
                                }
                            }
                        }
                    }
                    leitor.Close();
                }
            }
            else
            {
                Console.WriteLine("Arquivo inexistente!\nInforme o caminho correto do arquivo de faturas!");
                processouFaturas = false;
            }

            if (processouFaturas)
            {
                Console.WriteLine("\nProcessamento do arquivo de faturas concluido!");
                Console.WriteLine("Arquivos de saída gerados no diretório: '" + diretorioDeSaida + "'");
            }

            return processouFaturas;
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro no processamento do arquivo de faturas:\n" + e.Message);
            return false;
        }
    }

    // Cria aquivos de saida
    public bool CriaArquivosDeSaida(string diretorioDeSaida)
    {
        try
        {
            // Cria diretorio de saida caso nao exista
            if (!Directory.Exists(diretorioDeSaida))
            {
                Directory.CreateDirectory(diretorioDeSaida);
            }

            // Cria arquivos de acordo com enumeracao
            foreach (TipoDeArquivo tipoDeArquivo in Enum.GetValues(typeof(TipoDeArquivo)))
            {
                string caminhoCompleto = Path.Combine(diretorioDeSaida, String.Concat(tipoDeArquivo.ToString(), ".csv"));

                // Apaga arquivos anteriores caso existam
                if (File.Exists(caminhoCompleto))
                {
                    File.Delete(caminhoCompleto);
                }
            }
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine("Erro:\n" + e.Message);
            return false;
        }
    }

    // Valida faturas de acordo com as regras de tratamento estabelecidas
    public bool ValidarFaturas(Faturas fatura)
    {
        try
        {
            // Retira espacos em branco
            fatura.Cep = fatura.Cep;

            // Verifica tamanho do CPF se e numerico e diferente de zero
            if (fatura.Cep.Length >= 7 && fatura.Cep.Length <= 8 && double.TryParse(fatura.Cep, out _) && double.Parse(fatura.Cep) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro:\n" + e.Message);
            return false;
        }
    }

    // Grava faturas em arquivo pertinente
    public bool GravarFaturas(string diretorioDeSaida, Faturas fatura)
    {
        try
        {
            // 0 - Faturas com valores zero
            if (fatura.ValorFatura == 0)
            {
                // Independentemente do numero de paginas
                GravarFaturaNoArquivo(diretorioDeSaida, fatura, TipoDeArquivo.ValorZero);
            }
            // Demais tipos de fatura
            else
            {
                // O número de paginas no arquivo de saída deve ser sempre par
                if (fatura.NumeroPaginas % 2 == 0)
                {
                    // 1 - Ate 6 paginas
                    if (fatura.NumeroPaginas >= 1 && fatura.NumeroPaginas <= 6)
                    {
                        GravarFaturaNoArquivo(diretorioDeSaida, fatura, TipoDeArquivo.Ate06Paginas);
                    }
                    // 2 - Ate 12 paginas
                    if (fatura.NumeroPaginas >= 1 && fatura.NumeroPaginas <= 12)
                    {
                        GravarFaturaNoArquivo(diretorioDeSaida, fatura, TipoDeArquivo.Ate12Paginas);
                    }
                    // 3 - Mais que 12 paginas
                    if (fatura.NumeroPaginas > 12)
                    {
                        GravarFaturaNoArquivo(diretorioDeSaida, fatura, TipoDeArquivo.MaisQue12Paginas);
                    }
                }
            }
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine("Erro:\n" + e.Message);
            return false;
        }
    }

    public bool GravarFaturaNoArquivo(string diretorioDeSaida, Faturas fatura, TipoDeArquivo tipoDeArquivo)
    {
        try
        {
            string caminhoCompleto;
            string enderecoCompleto;
            string linhaFormatada;

            caminhoCompleto = Path.Combine(diretorioDeSaida, String.Concat(tipoDeArquivo.ToString(), ".csv"));
            enderecoCompleto = String.Concat(fatura.RuaComComplemento.Trim(), ", ", fatura.Bairro.Trim(), ", ", fatura.Cidade.Trim(), "-", fatura.Estado.Trim(), ", ", fatura.Cep.Trim());
            linhaFormatada = String.Concat(fatura.NomeCliente, ";", enderecoCompleto, ";", fatura.ValorFatura, ";", fatura.NumeroPaginas);

            // Gravador de dados assincrono para melhor desempenho
            using (StreamWriter gravador = new StreamWriter(caminhoCompleto, true, Encoding.ASCII))
            {
                gravador.WriteLineAsync(linhaFormatada);
                gravador.Close();
            }
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro:\n" + e.Message);
            return true;
        }
    }
}