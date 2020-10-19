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
                Motores.Wikipedia.ExibirMensagem += ExibirMensagem;
                //Motores.Wikipedia.GravabaseBairros();
                //Motores.Zap.ClassificarBairro();
                //string retorno = Motores.Zap.ConsultarImoveis();

                Motores.Zap.ConsultarImoveisPorBairros();
                //Motores.Zap.UnificarArquivos();
            }
            catch(Exception ex)
            {
                ExibirMensagem(ex.ToString());
            }
            finally
            {
                Motores.Zap.ExibirMensagem -= ExibirMensagem;
                Motores.Wikipedia.ExibirMensagem -= ExibirMensagem;
            }
        }

        //Método para exibir um texto no console
        private static void ExibirMensagem(string mensagem)
        {
            Console.WriteLine(mensagem);
        }        
    }
}
