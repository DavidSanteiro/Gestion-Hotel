using System;
using System.Xml;
using Gestión_Hotel.core.clientes;
using Gestión_Hotel.core.habitaciones;
using Gestión_Hotel.core.reservas;

namespace Gestión_Hotel.core;

/**
 * Clase estática (no se instancia) que sirve para guardar los datos de la app y acceder a ellos de forma global
 */
public static class DataController
{
    public static void InitializeDataController()
    {
        //Leer los ficheros XML desde disco
        try
        {
            clientes = XmlController<Cliente>.Recuperar(XmlController<Cliente>.TipoRegistro.Clientes);
            habitaciones = XmlController<Habitacion>.Recuperar(XmlController<Habitacion>.TipoRegistro.Habitaciones);
            reservas = XmlController<Reserva>.Recuperar(XmlController<Reserva>.TipoRegistro.Reservas);
        }
        catch (XmlException e)
        {
            Console.WriteLine("Error al leer clientes del fichero. Se comienza una sesión limpia." + e.Message);
            clientes = new Registro<Cliente>();
            habitaciones = new Registro<Habitacion>();
            reservas = new Registro<Reserva>();
        }
    }
    
    public static void SaveData()
    {
        XmlController<Cliente>.Guardar(clientes, XmlController<Cliente>.TipoRegistro.Clientes);
        XmlController<Habitacion>.Guardar(habitaciones, XmlController<Habitacion>.TipoRegistro.Habitaciones);
        XmlController<Reserva>.Guardar(reservas, XmlController<Reserva>.TipoRegistro.Reservas);
    }

    public static Registro<Cliente> clientes;
    public static Registro<Habitacion> habitaciones;
    public static Registro<Reserva> reservas;
}