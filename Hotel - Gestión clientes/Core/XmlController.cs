using System;
using System.Xml;
using System.Xml.Linq;

namespace Hotel___Gestión_clientes.Core;

public static class XmlController
{
    
    public static void Guardar(Registro<Cliente> registro)
    {
        XElement raiz = new XElement(XmlClientes);
        foreach (Cliente cliente in registro.Elementos)
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