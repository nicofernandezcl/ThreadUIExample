using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;

namespace ThreadUIExample.Pages
{
    public class IndexModel : PageModel
    {

        public string Message { get; set; }
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;

            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

            List<int> initialData = new List<int> { 1, 1 };
            worker.RunWorkerAsync(initialData);
            // versus
            //CalculateFibo(int.MaxValue / 2, initialData);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            List<int>? parameters = e.Argument as List<int>;

            CalculateFibo(int.MaxValue / 2, parameters);

            e.Result = parameters;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // aca actualiza tu bd o mandar correos o algo

            List<int>? result = e.Result as List<int>;
            Console.WriteLine($"terminé la ejecución");
            Console.WriteLine($"calculé {result.Count} numeros de fibonacci");
            Console.WriteLine($"los ultimos 3 valores que calcule son: ");
            Console.WriteLine(result[^1]);
            Console.WriteLine(result[^2]);
            Console.WriteLine(result[^3]);

            // caga el calculo pq no alcanza en un int32, pero se entiende la idea
        }

        private void CalculateFibo(int max, List<int>? parameters)
        {
            int i = 0;
            while (i < max - 1)
            {
                parameters.Add(parameters[i] + parameters[i + 1]);
                i++;
                //worker.ReportProgress(i / max);
            }
        }
    }
}