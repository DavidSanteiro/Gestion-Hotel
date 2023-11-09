﻿using System.Xml.Linq;

namespace Hotel___Gestión_clientes.Core;

public interface SerializableXML
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
    public void FromXElement(XElement xElement);
}