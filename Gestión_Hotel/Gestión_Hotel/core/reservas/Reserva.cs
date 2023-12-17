using System;
using System.Collections.Generic;
using System.Linq;
using Gestión_Hotel.core.clientes;
using Gestión_Hotel.core.habitaciones;

namespace Gestión_Hotel.core.reservas;

public class Reserva
{
    public int ID { get; init; }
    public Cliente? Cliente {get; init;}
    public Habitacion.TipoHabitacion Tipo { get; init; }
    public DateTime Entrada  { get; init; }
    public DateTime Salida { get; init; }
    public bool Garaje { get; init; }
    public int Importe { get; init; }
    public int IVA { get; init; }
    
    public static List<int> ContarReservasPorMes(IList<Reserva> listaDeReservas)
    {
        List<int> reservasPorMes = new List<int>(new int[12]);

        foreach (var reserva in listaDeReservas) 
        {
            int mesDeEntrada = reserva.Entrada.Month;
            reservasPorMes[mesDeEntrada - 1]++;
        }
        return reservasPorMes;
    }
    
    public static (List<String> clientes, List<int> reservasPorCliente) ContarReservasPorCliente(IList<Reserva> listaDeReservas)
    {
        Dictionary<String, int> dict = new Dictionary<String, int>();
        foreach (var reserva in listaDeReservas)
        {
            if (reserva.Cliente != null)
            {
                if (dict.ContainsKey(reserva.Cliente.Nombre))
                {
                    dict[reserva.Cliente.Nombre]++;
                }
                else
                {
                    dict[reserva.Cliente.Nombre] = 1;
                }
            }
        }
        
        List<String> clientes = dict.Keys.ToList();
        List<int> reservasPorCliente = dict.Values.ToList();

        return (clientes, reservasPorCliente);
    }
    
    public static List<int> ContarReservasPorHabitacion(IList<Reserva> listaDeReservas)
    {
        List<int> reservasPorHabitacion = new List<int>(new int[3]);

        foreach (var reserva in listaDeReservas) 
        {
            Habitacion.TipoHabitacion ind = reserva.Tipo;
            if (ind == Habitacion.TipoHabitacion.Individual)
            {
                reservasPorHabitacion[0]++;
            }
            if (ind == Habitacion.TipoHabitacion.Doble)
            {
                reservasPorHabitacion[1]++;
            }
            if(ind == Habitacion.TipoHabitacion.Matrimonial)
            {
                reservasPorHabitacion[2]++;
            }
        }
        return reservasPorHabitacion;
    }
}