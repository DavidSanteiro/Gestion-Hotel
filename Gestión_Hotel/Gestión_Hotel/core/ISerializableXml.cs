using System.Xml.Linq;

namespace Gestión_Hotel.core;

// Sería buena idea renombrarlo a CommonInterface o algo por el estilo. Se espera que contenga más que interfaz para XML
public interface ISerializableXml<T>
{
    /**
     * Crea un nodo XElement que contenga toda la información del elemento.
     * Se debe de poder "deserializar" con el método FromXElement.
     */
    public XElement ToXElement();
    /**
    * Despliega toda la clase del elemento a partir de un nodo "raíz" XElement
    * creado con el método ToXElement()
    */
    public static abstract T FromXElement(XElement xElement);
    /**
     * Muestra información útil sobre el elemento. La idea es mostrar la información más importante primero
     * (ids, nombres, ...) y mostrar después información menos relevante. Por ejemplo, en el caso de cliente,
     * despliega un string con la siguiente información: "DNI: 13621243E, Nombre: Luis Miguel Alba, Teléfono: 813739574,
     * Email: bcz5dsrzsi@netscape.net, Dirección postal: Rúa do Doutor Temes Fernández, s/n, bajo"
     */
    public string ToString();
}