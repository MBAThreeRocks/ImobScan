using System;

namespace ImobScan
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Motores.Zap.ExibirMensagem += ExibirMensagem;
                string retorno = Motores.Zap.ConsultarImoveis();
                ExibirMensagem(retorno);
            }
            catch(Exception ex)
            {
                ExibirMensagem(ex.ToString());
            }
            finally
            {
                Motores.Zap.ExibirMensagem -= ExibirMensagem;
            }
        }

        //Método para exibir um texto no console
        private static void ExibirMensagem(string mensagem)
        {
            Console.WriteLine(mensagem);
        }
        
    }
}
