using System;
using System.Collections.Generic;
using Gestión_Hotel.core.habitaciones;
using Gestión_Hotel.core.reservas;

namespace Gestión_Hotel.core.busquedas;

public static class Busqueda
{
    // Búsqueda de reservas pendientes
    public static List<Reserva> ReservasPendientes(IList<Reserva> reservas, int? habitacion = null)
    {
        var actual = DateTime.Now;
        var limite = actual.AddDays(5);

        List<Reserva> pendientes = new List<Reserva>();
        foreach (var reserva in reservas)
        {
            if (habitacion != null)
            {
                if (habitacion == long.Parse(reserva.IdReserva) % 1000 && reserva.FechaEntrada >= actual && reserva.FechaEntrada <= limite)
                {
                    pendientes.Add(reserva);
                }
            }
            else
            {
                if (reserva.FechaEntrada >= actual && reserva.FechaEntrada <= limite)
                {
                    pendientes.Add(reserva);
                }
            }

        }

        return pendientes;
    }

    // Búsqueda de habitaciones vacías
    public static List<Habitacion> HabitacionesVacias(IList<Reserva> reservas, IList<Habitacion> habitaciones, int? piso = null)
    {
        List<Habitacion> habitacionesVacias = new List<Habitacion>();
        var actual = DateTime.Now;

        foreach (var habitacion in habitaciones)
        {
            bool ocupada = false;
            if (piso != null && habitacion.ID / 100 == piso)
            {
                foreach (var reserva in reservas)
                {
                    if (long.Parse(reserva.IdReserva) % 1000 == habitacion.ID 
                        && reserva.FechaEntrada <= actual 
                        && reserva.FechaSalida >= actual)
                    {
                        ocupada = true;
                        break;
                    }
                }
                
                if (!ocupada && !habitacionesVacias.Contains(habitacion))
                {
                    habitacionesVacias.Add(habitacion);
                }
            }
            
            if (piso == null)
            {
                foreach (var reserva in reservas)
                {
                    if (long.Parse(reserva.IdReserva) % 1000 == habitacion.ID 
                        && reserva.FechaEntrada <= actual 
                        && reserva.FechaSalida >= actual)
                    {
                        ocupada = true;
                        break;
                    }
                }
                
                if (!ocupada && !habitacionesVacias.Contains(habitacion))
                {
                    habitacionesVacias.Add(habitacion);
                }
            }
        }

        return habitacionesVacias;
    }

    // Búsqueda de reservas por persona
    public static List<Reserva> ReservasPorPersona(string dni, IList<Reserva> reservas, string tipo)
    {
        var actual = DateTime.Now;
        List<Reserva> reservasTotales = new List<Reserva>();
        
        foreach (var reserva in reservas)
        {
            if (tipo.Equals("Pasadas") && reserva.FechaEntrada < actual && reserva.Cliente.Dni.Equals(dni))
            {
                reservasTotales.Add(reserva);
            }
            
            if (tipo.Equals("Pendientes") && reserva.FechaEntrada > actual && reserva.Cliente.Dni.Equals(dni))
            {
                reservasTotales.Add(reserva);
            }

            if (tipo.Equals("Todas") && reserva.Cliente.Dni == dni)
            {
                reservasTotales.Add(reserva);
            }
        }

        return reservasTotales;
    }

    // Búsqueda de reservas del hotel o por habitación
    public static List<Reserva> ReservasPorHabitacion(IList<Reserva> reservas, string tipo, int numHabitacion)
    {
        List<Reserva> reservasPorHabitacion = new List<Reserva>();
        var actual = DateTime.Now;
        
        foreach (var reserva in reservas)
        {
            if (long.Parse(reserva.IdReserva) % 1000 == numHabitacion)
            {
                if (tipo.Equals("Pasadas") && reserva.FechaEntrada < actual)
                {
                    reservasPorHabitacion.Add(reserva);
                }
            
                if (tipo.Equals("Pendientes") && reserva.FechaEntrada > actual)
                {
                    reservasPorHabitacion.Add(reserva);
                }
            }
        }

        return reservasPorHabitacion;
    }
    
    public static List<Reserva> ReservasHotel(IList<Reserva> reservas, string tipo)
    {
        List<Reserva> reservasHotel = new List<Reserva>();
        var actual = DateTime.Now;

        foreach (var reserva in reservas)
        {
            if (tipo.Equals("Pasadas") && reserva.FechaEntrada < actual)
            {
                reservasHotel.Add(reserva);
            }
            
            if (tipo.Equals("Pendientes") && reserva.FechaEntrada > actual)
            {
                reservasHotel.Add(reserva);
            }
        }

        return reservasHotel;
    }
    
    // Búsqueda de ocupación
    public static List<Habitacion> OcupacionAnho(int anho, IList<Reserva> reservas, IList<Habitacion> habitaciones)
    {
        List<Habitacion> ocupadas = new List<Habitacion>();
        foreach (var reserva in reservas)
        {
            if ((reserva.FechaEntrada.Year == anho || reserva.FechaSalida.Year == anho))
            {
                foreach (var habitacion in habitaciones)
                {
                    if (long.Parse(reserva.IdReserva) % 1000 == habitacion.ID && !ocupadas.Contains(habitacion))
                    {
                        ocupadas.Add(habitacion);
                    }
                }
            }
        }
        
        return ocupadas;
    }
    
    public static List<Habitacion> OcupacionFecha(DateTimeOffset fecha, IList<Reserva> reservas, IList<Habitacion> habitaciones)
    {
        DateTime date = fecha.DateTime;
        List<Habitacion> ocupadas = new List<Habitacion>();
        foreach (var reserva in reservas)
        {
            if (reserva.FechaEntrada.Date <= fecha && reserva.FechaSalida.Date >= fecha)
            { 
                foreach (var habitacion in habitaciones)
                {
                    if (long.Parse(reserva.IdReserva) % 1000 == habitacion.ID && !ocupadas.Contains(habitacion))
                    {
                        ocupadas.Add(habitacion);
                    }
                }   
            }
        }

        return ocupadas;
    }
}