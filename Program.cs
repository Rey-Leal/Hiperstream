// Teste Hiperstream - Analista de Sistemas Pleno
using System;
using System.IO;

namespace hiperstream;
class Program
{
    static void Main()
    {
        try
        {
            bool repetirExecucao = true;

            while (repetirExecucao)
            {
                Console.WriteLine("Olá seja bem vindo ao Teste Hiperstream!");

                //string arquivoDeFaturas = "E:\\Rey Leal\\Desktop\\Hiperstream\\base_hi.txt";

                Console.WriteLine("Digite o caminho do diretório aonde se encontra o arquivo 'base_hi.txt':");
                string arquivoDeFaturas = Console.ReadLine();

                if (!String.IsNullOrEmpty(arquivoDeFaturas))
                {
                    string extensao = arquivoDeFaturas.Substring(arquivoDeFaturas.Length - 4);
                    if (extensao == ".txt")
                    {
                        Faturas faturas = new Faturas();
                        faturas.ProcessarFaturas(arquivoDeFaturas);
                    }
                    else
                    {
                        Console.WriteLine("O sistema aceita apenas arquivos de faturas em formato .txt!");
                    }
                }
                else
                {
                    Console.WriteLine("Obrigatório informar o caminho do diretório!");
                }

                Console.WriteLine("Para repetir a execução digite 'sim' ou qualquer outra tecla para sair:");
                string entrada = Console.ReadLine();

                repetirExecucao = entrada.ToLower() == "sim" ? true : false;
                Console.Clear();
            }            
        }
        catch (Exception e)
        {
            Console.WriteLine("Erro na leitura da fatura:\n" + e.Message);
        }
    }
}
