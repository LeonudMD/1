using System;

namespace HexDumpApp
{
    public class Presenter : IPresenter
    {
        private readonly IView _view;
        private readonly IModel _model;

        public Presenter(IView view, IModel model)
        {
            _view = view;
            _model = model;
        }

        public void Run()
        {
            try
            {
                string filePath = _view.GetFilePath();
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    _view.ShowError("Путь к файлу не может быть пустым.");
                    return;
                }

                string hexDump = _model.GenerateHexDump(filePath);
                _view.ShowHexDump(hexDump);
            }
            catch (Exception ex)
            {
                _view.ShowError(ex.Message);
            }
        }
    }
}
