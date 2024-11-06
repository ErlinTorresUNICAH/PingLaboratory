using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net;

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
        var dispositivos = EscanearRangoIP(rangoIP);
        return View("Resultados", dispositivos);
    }

    private List<Dispositivo> EscanearRangoIP(string rangoIP)
    {
        List<Dispositivo> dispositivos = new List<Dispositivo>();

        // Obtener IP inicial y final a partir del rango
        var (ipInicio, ipFin) = CalcularRangoIPs(rangoIP);

        for (var ip = ipInicio; CompareIPs(ip, ipFin) <= 0; ip = IncrementarIP(ip))
        {
            string direccionIP = ip.ToString();
            bool activo = HacerPing(direccionIP);

            dispositivos.Add(new Dispositivo
            {
                DireccionIP = direccionIP,
                Estado = activo ? true : false
            });
        }

        return dispositivos;
    }

    private (IPAddress, IPAddress) CalcularRangoIPs(string rangoIP)
    {
        // Suponiendo que rangoIP está en formato CIDR, ejemplo: "192.168.1.0/24"
        var partes = rangoIP.Split('/');
        IPAddress ipInicio = IPAddress.Parse(partes[0]);
        int prefijo = int.Parse(partes[1]);

        uint mascara = ~(uint.MaxValue >> prefijo);
        byte[] bytesInicio = ipInicio.GetAddressBytes();
        byte[] bytesMascara = BitConverter.GetBytes(mascara).Reverse().ToArray();

        byte[] bytesFin = new byte[4];
        for (int i = 0; i < 4; i++)
        {
            bytesFin[i] = (byte)(bytesInicio[i] | ~bytesMascara[i]);
        }

        IPAddress ipFin = new IPAddress(bytesFin);
        return (ipInicio, ipFin);
    }

    private IPAddress IncrementarIP(IPAddress ip)
    {
        byte[] bytes = ip.GetAddressBytes();
        for (int i = bytes.Length - 1; i >= 0; i--)
        {
            if (++bytes[i] != 0)
            {
                break;
            }
        }
        return new IPAddress(bytes);
    }

    private bool HacerPing(string direccionIP)
    {
        try
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send(direccionIP, 1000); // Timeout de 1 segundo
            return reply.Status == IPStatus.Success;
        }
        catch
        {
            return false;
        }
    }

    private int CompareIPs(IPAddress ip1, IPAddress ip2)
    {
        byte[] bytes1 = ip1.GetAddressBytes();
        byte[] bytes2 = ip2.GetAddressBytes();

        for (int i = 0; i < bytes1.Length; i++)
        {
            int diff = bytes1[i].CompareTo(bytes2[i]);
            if (diff != 0)
            {
                return diff;
            }
        }
        return 0;
    }
}
