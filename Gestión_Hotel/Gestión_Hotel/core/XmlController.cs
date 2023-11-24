using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Gestión_Hotel.core;

// "XmlController <T> where T : ISerializableXml<T>" indica que la clase es genérica pero con la restricción de que el
// tipo genérico "T" debe de implementar la interfaz ISerializableXml
public static class XmlController<T> where T : ISerializableXml<T>
{
    /**
     * Método que almacena los elementos en el fichero XML
     */
    public static void Guardar(Registro<T> registro, TipoRegistro tipoRegistro)
    {
        XElement raiz = new XElement(NombreElementoRaizXml);
        foreach (T elemento in registro.Elementos)
        {
            raiz.Add(elemento.ToXElement());
        }

        raiz.Save(GetNombreFichero(tipoRegistro));
    }

    /**
     * Método que recuera los elementos del fichero XML
     */
    public static Registro<T> Recuperar(TipoRegistro tipoRegistro)
    {
        FileInfo fileInfo = new FileInfo(GetNombreFichero(tipoRegistro));
        if (!fileInfo.Exists)
        {
            throw new XmlException("No existe el fichero " + GetNombreFichero(tipoRegistro));
        }

        Registro<T> registro = new Registro<T>();
        XElement raiz = XElement.Load(GetNombreFichero(tipoRegistro));
        foreach (XElement xElement in raiz.Elements())
        {
            try
            {
                registro.Add(T.FromXElement(xElement));
            }
            catch (XmlException e)
            {
                Console.Error.WriteLine($"AVISO: Se ha producido un error al leer el elemento: " +
                                        $"({xElement}) y se ha omitido.\n Excepción original: ({e})");
            }
        }

        return registro;
    }

    private static string GetNombreFichero(TipoRegistro tipo)
    {
        string toRet;
        switch (tipo)
        {
            case TipoRegistro.Clientes:
                toRet = NombreFicheroClientes;
                break;
            case TipoRegistro.Habitaciones:
                toRet = NombreFicheroHabitaciones;
                break;
            case TipoRegistro.Reservas:
                toRet = NombreFicheroReservas;
                break;
            default:
                throw new Exception("NO IMPLEMENTADO");
        }

        return toRet;
    }
    
    public enum TipoRegistro {
        Clientes,
        Habitaciones,
        Reservas
    };

    public static string NombreFicheroClientes = "xmlClientes.xml";
    public static string NombreFicheroHabitaciones = "xmlHabitaciones.xml";
    public static string NombreFicheroReservas = "xmlReservas.xml";

    private const string NombreElementoRaizXml = "registro";
}