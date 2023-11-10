using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Hotel___Gestión_clientes.Core;

public static class  XmlController <T> where T : SerializableXML<T>
{
    
    public static void Guardar(Registro<T> registro)
    {
        XElement raiz = new XElement(XmlClientes);
        foreach (T cliente in registro.Elementos)
        {
            raiz.Add(cliente.ToXElement());
        }
        raiz.Save(XmlFileName);
    }

    public static Registro<Cliente> Recuperar()
    {
        FileInfo fileInfo = new FileInfo(XmlFileName);
        if (!fileInfo.Exists) {
            throw new XmlException("No existe el fichero " + XmlFileName);
        }
        
        Registro<Cliente> registro = new Registro<Cliente>();
        XElement raiz = XElement.Load(XmlFileName);
        foreach (XElement xElement in raiz.Elements())
        {
            try
            {
                registro.Add(Cliente.FromXElement(xElement));
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