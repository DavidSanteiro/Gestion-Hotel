using System.Xml;
using System.Xml.Linq;

namespace Gestión_Hotel.core.clientes;

public class Cliente : ISerializableXml<Cliente>
{
    
    public string Dni { get; init; }
    public string? Nombre { get; set; }
    public int Telefono { get; set; }
    public string? Email { get; set; }
    public string? DireccionPostal { get; set; }

    public override string ToString()
    {
        return $"DNI: {this.Dni}, Nombre: {this.Nombre}, Teléfono: {this.Telefono}, Email: {this.Email}, Dirección postal: {this.DireccionPostal}";
    }

    public XElement ToXElement()
    {
        return new XElement(XmlClient,
            new XElement(XmlDni, this.Dni),
            new XElement(XmlNombre, this.Nombre),
            new XElement(XmlTelefono, this.Telefono),
            new XElement(XmlEmail, this.Email),
            new XElement(XmlDireccionPostal, this.DireccionPostal));
    }

    public static Cliente FromXElement(XElement xElement)
    {
        if (xElement == null)
        {
            throw new XmlException("ERROR: Null value can't be converted to cliente.");
        }

        string dni = (string?) xElement.Element(XmlDni) ?? "";
        string nombre = (string?) xElement.Element(XmlNombre) ?? "";
        int telefono = (int?) xElement.Element(XmlTelefono) ?? -1;
        string email = (string?) xElement.Element(XmlEmail) ?? "";
        string direccionPostal = (string?) xElement.Element(XmlDireccionPostal) ?? "";

        if (string.IsNullOrEmpty(dni)
            || string.IsNullOrEmpty(nombre)
            || telefono == -1
            || string.IsNullOrEmpty(email)
            || string.IsNullOrEmpty(direccionPostal))
        {
            throw new XmlException("ERROR when parsing cliente.");
        }

        return new Cliente
        {
            Dni = dni,
            Nombre = nombre,
            Telefono = telefono,
            Email = email,
            DireccionPostal = direccionPostal
        };
    }

    private static readonly string XmlClient = "client";
    private static readonly string XmlDni = "dni";
    private static readonly string XmlNombre = "nombre";
    private static readonly string XmlTelefono = "telefono";
    private static readonly string XmlEmail = "email";
    private static readonly string XmlDireccionPostal = "direccionPostal";

}