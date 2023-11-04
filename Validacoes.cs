
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
                using (StreamReader reader = new StreamReader(arquivoDeFaturas))
                {
                    while ((linhaDaFatura = reader.ReadLine()) != null)
                    {
                        // Cria nova classe da fatura
                        dadosDaFatura = linhaDaFatura.Split(';');

                        // Considera apenas linhas com campos numericos nos campos ValorFatura e NumeroPaginas
                        if (Double.TryParse((dadosDaFatura[6]), out _) && Int32.TryParse((dadosDaFatura[7]), out _))
                        {
                            Faturas fatura = new Faturas(dadosDaFatura[0], dadosDaFatura[1], dadosDaFatura[2], dadosDaFatura[3], dadosDaFatura[4], dadosDaFatura[5], Double.Parse(dadosDaFatura[6]), Int32.Parse(dadosDaFatura[7]));
                            // Realiza validacao da fatura
                            if (ValidarFaturas(fatura))
                            {
                                // Prossegue com gravacao em arquivos caso a fatura seja valida 
                                if (GravarFaturas(diretorioDeSaida, fatura))
                                {
                                    processouFaturas = true;
                                }

                                // TESTE DE IMPRESSAO - APAGAR AO FINAL !!!
                                //Console.WriteLine(String.Concat(fatura.NomeCliente, "|", fatura.Cep, "|", fatura.RuaComComplemento, "|", fatura.Bairro, "|", fatura.Cidade, "|", fatura.Estado, "|", fatura.ValorFatura, "|", fatura.NumeroPaginas));
                            }
                        }
                    }
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
            string[] arquivosDeSaida = { "valorZero.csv", "ate06Paginas.csv", "ate12Paginas.csv", "maisQue12Paginas.csv" };

            // Cria diretorio de saida caso nao exista
            if (!Directory.Exists(diretorioDeSaida))
            {
                Directory.CreateDirectory(diretorioDeSaida);
            }

            foreach (string arquivo in arquivosDeSaida)
            {
                string caminhoCompleto = Path.Combine(diretorioDeSaida, arquivo);

                // Apaga arquivos anteriores caso existam
                if (File.Exists(caminhoCompleto))
                {
                    File.Delete(caminhoCompleto);
                }
                // Cria novos arquivos                                
                File.Create(caminhoCompleto);
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

    // TODO:                            
    // Gravar faturas em arquivos conforme regras                            
    // Criar funcao que possibilite gravar varios arquivos simultaneamente

    // Faturas com valores zero (valorZero.csv)
    //      Independentemente do número de páginas

    // Ate 6 paginas (ate06Paginas.csv)
    // Ate 12 paginas (ate12Paginas.csv) ?
    // Mais que 12 paginas (maisQue12Paginas.csv)
    //      O número de páginas no arquivo de saída deve ser sempre par ???

    // Grava faturas em arquivo pertinente
    public bool GravarFaturas(string diretorioDeSaida, Faturas fatura)
    {
        try
        {
            // 0 - Faturas com valores zero - valorZero.csv
            if (fatura.ValorFatura == 0)
            {
                GravarFaturaNoArquivo(diretorioDeSaida, fatura, 0);
            }
            // Demais tipos de fatura
            else
            {
                // 1 - Ate 6 paginas - ate06Paginas.csv
                if (fatura.NumeroPaginas >= 1 && fatura.NumeroPaginas <= 6)
                {
                    GravarFaturaNoArquivo(diretorioDeSaida, fatura, 1);
                }
                // 2 - Ate 12 paginas - ate12Paginas.csv
                if (fatura.NumeroPaginas >= 1 && fatura.NumeroPaginas <= 12)
                {
                    GravarFaturaNoArquivo(diretorioDeSaida, fatura, 2);
                }
                // 3 - Mais que 12 paginas - maisQue12Paginas.csv
                if (fatura.NumeroPaginas > 12)
                {
                    GravarFaturaNoArquivo(diretorioDeSaida, fatura, 3);
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

    public bool GravarFaturaNoArquivo(string diretorioDeSaida, Faturas fatura, int tipoDeArquivo)
    {
        try
        {
            string[] arquivosDeSaida = { "valorZero.csv", "ate06Paginas.csv", "ate12Paginas.csv", "maisQue12Paginas.csv" };
            string caminhoCompleto = Path.Combine(diretorioDeSaida, arquivosDeSaida[tipoDeArquivo]);

            StreamWriter stream = new StreamWriter(caminhoCompleto, true, Encoding.ASCII);
            stream.WriteLine(fatura.NomeCliente + ";" + fatura.ValorFatura + ";" + fatura.NumeroPaginas);
            stream.Close();

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro:\n" + e.Message);
            return false;
        }
    }
}