using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Gestión_Hotel.core.clientes;
using Gestión_Hotel.core.habitaciones;

namespace Gestión_Hotel.core.reservas;

public class Reserva : ISerializableXml<Reserva>
{
        public string IdReserva { get; private set; }
        public string Tipo { get; set; }
        public Cliente Cliente { get; set; }
        
        public Habitacion Habitacion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        public bool UsoGaraje { get; set; }
        public decimal ImportePorDia { get; set; }
        public decimal IvaAplicado { get; set; }

        
        
        public Reserva(string tipo, Cliente cliente,Habitacion habitacion, DateTime fechaEntrada, DateTime fechaSalida, bool usoGaraje, decimal importePorDia, decimal ivaAplicado)
        {
            Tipo = tipo;
            Cliente = cliente;
            Habitacion = habitacion;
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
            UsoGaraje = usoGaraje;
            ImportePorDia = importePorDia;
            IvaAplicado = ivaAplicado;

            // Generar el Id de reserva único
            IdReserva = GenerarIdReserva();
        }

        private string GenerarIdReserva()
        {
            string idAnio = DateTime.Now.Year.ToString();
            string idMes = DateTime.Now.Month.ToString("00");
            string idDia = DateTime.Now.Day.ToString("00");
            string idHabitacion = Habitacion.ID.ToString("000");
            return $"{idAnio}{idMes}{idDia}{idHabitacion}";
        }

        public decimal CalcularImporteTotal()
        {
            TimeSpan duracionReserva = FechaSalida - FechaEntrada;
            int numeroDias = duracionReserva.Days;

            decimal importeTotal = (ImportePorDia * numeroDias) * (1 + (IvaAplicado / 100));
            return importeTotal;
        }

        public string GenerarFactura()
        {
            decimal importeTotal = CalcularImporteTotal();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"FACTURA - Reserva: {IdReserva}").Append("\n");
            stringBuilder.Append($"Cliente: {Cliente.Nombre} tlf: {Cliente.Telefono} email: {Cliente.Email}").Append("\n");
            stringBuilder.Append($"ID Habitacion: {this.Habitacion.ID} Tipo: {this.Habitacion.Tipo}").Append("\n");
            stringBuilder.Append($"Precio por día: {ImportePorDia:C}").Append("\n");
            stringBuilder.Append($"Número de días: {(FechaSalida - FechaEntrada).Days}").Append("\n");
            stringBuilder.Append($"IVA aplicado: {IvaAplicado}%").Append("\n");
            stringBuilder.Append($"Total a pagar: {importeTotal:C}").Append("\n");
            return stringBuilder.ToString();
        }

        public string ToString()
        {
            decimal importeTotal = CalcularImporteTotal();
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"Reserva numero: {IdReserva} ");
            stringBuilder.Append($"Dni cliente: {this.Cliente.Dni} ");
            stringBuilder.Append($"Numero habitacion {this.Habitacion.ID} ");
            stringBuilder.Append($"Precio por día: {ImportePorDia:C} ");
            stringBuilder.Append($"Número de días: {(FechaSalida - FechaEntrada).Days} ");
            stringBuilder.Append($"IVA aplicado: {IvaAplicado}% ");
            stringBuilder.Append($"Total a pagar: {importeTotal:C} ");

            return stringBuilder.ToString();
        }


        public XElement ToXElement()
        {
            return new XElement("Reserva",
                new XElement("IdReserva", IdReserva),
                new XElement("Tipo", Tipo),
                new XElement("Cliente", Cliente.Dni),
                new XElement("Habitacion", Habitacion.ID),
                new XElement("FechaEntrada", FechaEntrada),
                new XElement("FechaSalida", FechaSalida),
                new XElement("UsoGaraje", UsoGaraje),
                new XElement("ImportePorDia", ImportePorDia),
                new XElement("IvaAplicado", IvaAplicado)
            );
        }
        
        public static Reserva FromXElement(XElement xElement)
        {
            if (xElement == null)
            {
                throw new ArgumentNullException(nameof(xElement));
            }

            string idReserva = (string)xElement.Element("IdReserva") ?? throw new ArgumentException("IdReserva is required");
            string tipo = (string)xElement.Element("Tipo") ?? throw new ArgumentException("Tipo is required");
            Cliente cliente = Reserva.FindClienteFromDataController((string)xElement.Element("Cliente"));
            Habitacion habitacion = Reserva.FindHabitacionFromDataController((int)xElement.Element("Habitacion"));
            DateTime fechaEntrada = DateTime.Parse(xElement.Element("FechaEntrada")?.Value ?? throw new ArgumentNullException("FechaEntrada"));
            DateTime fechaSalida = DateTime.Parse(xElement.Element("FechaSalida")?.Value ?? throw new ArgumentNullException("FechaSalida"));
            bool usoGaraje = bool.Parse(xElement.Element("UsoGaraje")?.Value ?? throw new ArgumentNullException("UsoGaraje"));
            decimal importePorDia = decimal.Parse(xElement.Element("ImportePorDia")?.Value ?? throw new ArgumentNullException("ImportePorDia"));
            decimal ivaAplicado = decimal.Parse(xElement.Element("IvaAplicado")?.Value ?? throw new ArgumentNullException("IvaAplicado"));
            
            // Console.WriteLine(cliente.ToString());
            return new Reserva(tipo, cliente, habitacion, fechaEntrada, fechaSalida, usoGaraje, importePorDia, ivaAplicado)
            {
                IdReserva = idReserva
            };
        }
        
        private static Cliente? FindClienteFromDataController(string dni)
        {
            foreach (Cliente cliente in DataController.clientes.Elementos)
            {
                if (cliente.Dni == dni)
                {
                    return cliente;
                }
            }
            return null;
        }

        private static Habitacion? FindHabitacionFromDataController(int idPNn)
        {
            foreach (Habitacion habitacion in DataController.habitaciones.Elementos)
            {
                if (habitacion.ID == idPNn)
                {
                    return habitacion;
                }
            }
            return null;
        }
    }