namespace Gestión_Hotel.core.habitaciones;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Xml.Linq;



public class Habitacion: ISerializableXml<Habitacion>

{
        public Habitacion( TipoHabitacion tipo, int id, DateTimeOffset ultimaReserva, DateTimeOffset ultimaRenovacion, List<String> comodidades )
        {
            Tipo = tipo;
            ID = id;
            UltimaReserva = ultimaReserva;
            UltimaRenovacion = ultimaRenovacion;
            Comodidades = comodidades;
        }
        public TipoHabitacion Tipo { get; init; }

        public int Piso
        {
            get
            {
                return ID / 100;
            }
           
        }

        public XElement ToXElement()
        {
            return new XElement("Habitacion",
                new XAttribute("Tipo", Tipo),
                new XAttribute("ID", ID),
                new XAttribute("UltimaReserva", UltimaReserva.ToString("yyyy-MM-ddTHH:mm:ss")),
                new XAttribute("UltimaRenovacion", UltimaRenovacion.ToString("yyyy-MM-ddTHH:mm:ss")),
                new XElement("Comodidades", 
                    Comodidades.ConvertAll(comodidad => new XElement("Comodidad", comodidad))
                )
            );
        }

        public static Habitacion FromXElement(XElement xElement)
        {
            return new Habitacion(
                tipo: Enum.Parse<TipoHabitacion>(xElement.Attribute("Tipo")?.Value ?? throw new ArgumentNullException("Tipo")),
                id: int.Parse(xElement.Attribute("ID")?.Value ?? throw new ArgumentNullException("ID")),
                ultimaReserva: DateTimeOffset.Parse(xElement.Attribute("UltimaReserva")?.Value ?? throw new ArgumentNullException("UltimaReserva")),
                ultimaRenovacion: DateTimeOffset.Parse(xElement.Attribute("UltimaRenovacion")?.Value ?? throw new ArgumentNullException("UltimaRenovacion")),
                comodidades: xElement.Element("Comodidades")?.Elements("Comodidad")
                    .Select(comodidadElement => comodidadElement.Value).ToList() ?? new List<string>()
            );
        }

        public override String ToString()
        {
            return $"Habitacion {ID}, {Tipo}, ultima reserva: {UltimaReserva.ToString()}," +
                   $" Comodidades: {String.Join(" ,",Comodidades)}, ultima renovacion {UltimaRenovacion.ToString()}";
        }

        public int ID { get; set; }
        public DateTimeOffset UltimaRenovacion { get; init; }
        public DateTimeOffset UltimaReserva { get; init; }
        public List<String> Comodidades { get; }
        
        public enum TipoHabitacion
        {
            Matrimonial,
            Doble,
            Individual
        }       
}