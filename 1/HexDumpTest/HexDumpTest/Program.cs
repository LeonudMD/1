using System;

namespace HexDumpApp
{
    class Program
    {
        static void Main(string[] args)
        {
            IView view = new View();
            IModel model = new Model();
            IPresenter presenter = new Presenter(view, model);
            presenter.Run();

            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
