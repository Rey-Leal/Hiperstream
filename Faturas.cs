
public class Faturas
{
    // Realiza leitura e processamento do Arquivo de Faturas
    public bool ProcessarFaturas(string arquivoDeFaturas)
    {
        try
        {
            string linha;
            bool existeArquivo = File.Exists(arquivoDeFaturas);

            if (existeArquivo)
            {
                using (StreamReader reader = new StreamReader(arquivoDeFaturas))
                {
                    while ((linha = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(linha);
                    }
                }
                // Processou arquivo com sucesso
                return true;
            }
            else
            {
                Console.WriteLine("Arquivo não existe!\nInforme o caminho correto do arquivo de faturas!");
                return false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro na leitura da fatura:\n" + e.Message);
            return false;
        }
    }

    public int Somar(int a, int b)
    {
        try
        {
            return a + b;
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro:\n" + e.Message);
            return 0;
        }
    }
}
