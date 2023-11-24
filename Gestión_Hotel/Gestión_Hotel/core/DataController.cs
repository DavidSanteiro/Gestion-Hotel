using System;
using System.Xml;

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
            // reservas = XmlController<Reserva>.Recuperar(XmlController<Reserva>.TipoRegistro.Reservas);
            // habitaciones = XmlController<Habitacion>.Recuperar(XmlController<Habitacion>.TipoRegistro.Habitaciones);
            // clientes = XmlController<Cliente>.Recuperar(XmlController<Cliente>.TipoRegistro.Clientes);
        }
        catch (XmlException e)
        {
            Console.WriteLine("Error al leer clientes del fichero. Se comienza una sesión limpia." + e.Message);
            // reservas = new Registro<Reserva>();
            // habitaciones = new Registro<Habitacion>();
            // clientes = new Registro<Cliente>();
        }
    }
    
    public static void SaveData()
    {
        // XmlController<Reserva>.Guardar(reservas, XmlController<A>.TipoRegistro.Reservas);
        // XmlController<Habitacion>.Guardar(habitaciones, XmlController<A>.TipoRegistro.Habitaciones);
        // XmlController<Cliente>.Guardar(clientes, XmlController<A>.TipoRegistro.Clientes);
    }
    
    // public static Registro<Reserva> reservas;
    // public static Registro<Habitacion> habitaciones;
    // public static Registro<Cliente> clientes;
}