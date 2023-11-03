
public class Validacoes
{
    // Realiza leitura e processamento do arquivo de faturas
    public bool ProcessarFaturas(string arquivoDeFaturas)
    {
        try
        {
            string linhaDaFatura;
            string[] dadosDaFatura;

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
                        Fatura fatura = new Fatura(dadosDaFatura[0], dadosDaFatura[1], dadosDaFatura[2], dadosDaFatura[3], dadosDaFatura[4], dadosDaFatura[5], dadosDaFatura[6], dadosDaFatura[7]);

                        // Realiza validacao da linha de fatura
                        if (ValidarFaturas(fatura))
                        {
                            // Caso fatura seja valida prossegue com gravacao em arquivos

                            // TODO:                            
                            // Gravar faturas em arquivos conforme regras                            
                            // Criar funcao que possibilite gravar varios arquivos simultaneamente

                            // 1 Faturas com valores zero (valorZero.csv)
                            //      Independentemente do número de páginas

                            // 2 Ate 6 paginas (ate06Paginas.csv)
                            // 3 De 7 a 12 paginas (ate12Paginas.csv) ?
                            // 4 Mais que 12 paginas (mais12Paginas.csv)
                            // 5    O número de páginas no arquivo de saída deve ser sempre par ???

                            // TESTE DE IMPRESSAO - APAGAR AO FINAL
                            Console.WriteLine(String.Concat(fatura.NomeCliente, "|", fatura.Cep, "|", fatura.RuaComComplemento, "|", fatura.Bairro, "|", fatura.Cidade, "|", fatura.Estado, "|", fatura.ValorFatura, "|", fatura.NumeroPaginas));
                        }
                    }
                }
                // Processou arquivo com sucesso
                Console.WriteLine("Processamento do arquivo de faturas realizada com sucesso!");
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
            Console.WriteLine("Erro no processamento do arquivo de faturas:\n" + e.Message);
            return false;
        }
    }

    // Valida faturas de acordo com as regras de tratamento estabelecidas
    public bool ValidarFaturas(Fatura fatura)
    {
        try
        {
            // Realiza validacoes de CPF            
            if (ValidarCPF(fatura.Cep))
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

    // Valida CPF
    public bool ValidarCPF(string cpf)
    {
        try
        {
            // Retira espacos em branco
            cpf = cpf.Trim();

            // Verifica tamanho do CPF se e numerico e diferente de zero
            if (cpf.Length >= 7 && cpf.Length <= 8 && double.TryParse(cpf, out _) && double.Parse(cpf) != 0)
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

    // Grava faturas em arquivo pertinente caso tenha validado perante as regras estabelecidas
    public bool GravarFaturas(string[] linha)
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