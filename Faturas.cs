
public class Faturas
{
    // Realiza leitura e processamento do arquivo de faturas
    public bool ProcessarFaturas(string arquivoDeFaturas)
    {
        try
        {
            List<string[]> listaDaFatura = new List<string[]>();
            string[] dadosDaFatura;
            string linhaDaFatura;

            // Verifica existencia do arquivo de faturas
            bool existeArquivo = File.Exists(arquivoDeFaturas);

            if (existeArquivo)
            {
                // Realiza leitura do arquivo de faturas e armazena dados na lista
                using (StreamReader reader = new StreamReader(arquivoDeFaturas))
                {
                    while ((linhaDaFatura = reader.ReadLine()) != null)
                    {
                        dadosDaFatura = linhaDaFatura.Split(';');
                        listaDaFatura.Add(dadosDaFatura);
                    }

                    // Percorre lista da fatura realizando comparacoes necessarias
                    foreach (var linha in listaDaFatura)
                    {
                        // Teste de impressao da lista
                        Console.WriteLine(String.Concat(linha[0], "|", linha[1], linha[2], "|", linha[3], "|", linha[4], "|", linha[5], "|", linha[6], "|", linha[7]));

                        // TODO
                        // realizar comparacoes
                        // criar classe de gravacao de arquivos                        
                    }
                }
                // Processou arquivo com sucesso
                Console.WriteLine("Leitura do arquivo de faturas realizada com sucesso!");
                return true;
            }
            else
            {
                Console.WriteLine("Arquivo inexistente!\nInforme o caminho correto do arquivo de faturas!");
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro na leitura do arquivo de faturas:\n" + e.Message);
            return false;
        }
    }

    public bool GravarArquivoDeSaida(string arquivo, string linha)
    {
        try
        {
            return true;

        }
        catch (Exception e)
        {
            Console.WriteLine("Erro:\n" + e.Message);
            return false;
        }
    }
}