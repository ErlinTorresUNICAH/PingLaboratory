/*using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

public class PingController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public ActionResult EscanearRed(string rangoIP)
    {
        var dispositivos = EjecutarNmapPing(rangoIP);
        return View("Resultados", dispositivos);
    }

    private List<Dispositivo> EjecutarNmapPing(string rangoIP)
    {
        List<Dispositivo> dispositivos = new List<Dispositivo>();

        // Configura el proceso para ejecutar Nmap
        Process process = new Process();
        process.StartInfo.FileName = @"C:\Program Files (x86)\Nmap\nmap.exe";  // Ruta a Nmap
        process.StartInfo.Arguments = $"-sn {rangoIP}"; // Escaneo tipo ping
        process.StartInfo.RedirectStandardOutput = true;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;

        process.Start();

        string output = process.StandardOutput.ReadToEnd();
        process.WaitForExit();

        // Procesar la salida de Nmap para extraer direcciones IP y estado
        string[] lines = output.Split('\n');
        foreach (var line in lines)
        {
            if (line.Contains("Nmap scan report for"))
            {
                string ip = line.Split(' ')[4];
                dispositivos.Add(new Dispositivo
                {
                    DireccionIP = ip,
                    Estado = "Activo"
                });
            }
        }

        return dispositivos;
    }
}
*/