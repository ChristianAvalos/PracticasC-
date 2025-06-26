using Microsoft.Extensions.Options;

namespace BivliotecaAPI
{
       public class PagosProcesamiento
        {
            private tarifaOpciones _tarifaOpciones;

            public PagosProcesamiento(IOptionsMonitor<tarifaOpciones> optionsMonitor) 
            {
                    _tarifaOpciones = optionsMonitor.CurrentValue;
                    optionsMonitor.OnChange((nuevaTarifa) =>
                    {
                        Console.WriteLine("Tarifa actualizada");
                        _tarifaOpciones = nuevaTarifa;
                    });
            }

            public void ProcesarPago()
            {
            //aqui usamos las tarifas
            }
            public tarifaOpciones ObtenerTarifa()
            {
                return _tarifaOpciones;
            }
    }
}
