using System;
using System.Xml;
using System.Xml.Linq;

namespace Hotel___Gestión_clientes.Core;

public static class  XmlController <T> where T : SerializableXML
{
    
    public static void Guardar(Registro<T> registro)
    {
        XElement raiz = new XElement(XmlClientes);
        foreach (T cliente in registro.Elementos)
        {
            raiz.Add(cliente.ToXElement());
        }
        raiz.Save(XmlClientes);
    }

    public static Registro<Cliente> Recuperar()
    {
        Registro<Cliente> registro = new Registro<Cliente>();
        XElement raiz = XElement.Load(XmlFileName);
        foreach (XElement xElement in raiz.Elements())
        {
            try
            {
                registro.Add(new Cliente(xElement));
            }
            catch (XmlException e)
            {
                Console.Error.WriteLine($"AVISO: Se ha producido un error al leer el elemento: " +
                                        $"({xElement}) y se ha omitido.\n Excepción original: ({e})");
            }
        }
        return registro;
    }

    public static string XmlFileName = "registroClientes.xml";
    private static readonly string XmlClientes = "registroClientes";
}