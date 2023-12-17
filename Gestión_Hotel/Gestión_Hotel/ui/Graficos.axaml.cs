using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Gestión_Hotel.core;
using Gestión_Hotel.core.clientes;
using Gestión_Hotel.core.habitaciones;
using Gestión_Hotel.core.reservas;

namespace Gestión_Hotel.ui;

public partial class Graficos : UserControl
{
            public Graficos()
            {
                InitializeComponent();
    
                this.Chart = this.FindControl<Chart>( "ChGeneral" );
                var rbBars = this.FindControl<RadioButton>( "RbBars" );
                var rbLine = this.FindControl<RadioButton>( "RbLine" );
                var edThickness = this.FindControl<NumericUpDown>( "EdThickness" );
                var cbBold = this.FindControl<CheckBox>( "CbBold" );
                var cbItalic = this.FindControl<CheckBox>( "CbItalic" );
                var opGeneral = this.FindControl<Button>( "opGeneral" );
                var opCliente = this.FindControl<Button>( "opCliente" );
                var opHabitacion = this.FindControl<Button>( "opHabitacion" );
                var opComodidad = this.FindControl<Button>( "opComodidad" );
                
                rbBars.Checked += (_, _) => this.OnChartFormatChanged();
                rbLine.Checked += (_, _) => this.OnChartFormatChanged();
                edThickness.ValueChanged += (_, evt) => this.OnChartThicknessChanged( (double)evt.NewValue );
                cbBold.Click += (_, _) => this.OnFontsStyleChanged();
                cbItalic.Click += (_, _) => this.OnFontsStyleChanged();
                opGeneral.Click += (_, _) => this.ChangeGeneral();
                opCliente.Click += (_, _) => this.ChangeClientes();
                opHabitacion.Click += (_, _) => this.ChangeHabitacion();
                opComodidad.Click += (_, _) => this.ChangeComodidad();
                
                this.Chart.LegendY = "Reservas";
                this.Chart.LegendX = "Meses";
                
                List<int> recuentoPorMes = Reserva.ContarReservasPorMes(listaDeReservas);
                this.Chart.Values = recuentoPorMes;
                this.Chart.Labels = new []{ "En", "Fb", "Ma", "Ab", "My", "Jn", "Jl", "Ag", "Sp", "Oc", "Nv", "Dc" };


                registroHabitaciones = new Registro<Habitacion>(listaDeHabitaciones);
            }
    
            void ChangeComodidad()
            {
                this.Chart.LegendY = "Numero de habitaciones";
                this.Chart.LegendX = "Tipos de comodidades";
                var recuentoPorComodidades = ContarHabitacionesConComodidad(registroHabitaciones);
                List<string> comodidadesPosibles = new List<string>{"Wifi", "Caja fuerte", "Mini-bar", "Baño", "Cocina", "TV"};
                this.Chart.Values = recuentoPorComodidades;
                this.Chart.Labels = comodidadesPosibles;
                this.Chart.Draw();
            }
    
            void ChangeHabitacion()
            {
                this.Chart.LegendY = "Reservas";
                this.Chart.LegendX = "Tipos de Habitacion";
                var recuentoPorHabitacion = Reserva.ContarReservasPorHabitacion(listaDeReservas);
                this.Chart.Values = recuentoPorHabitacion;
                this.Chart.Labels = new []{ "Individual", "Doble", "Matrimonial"};
                this.Chart.Draw();
            }
    
            void ChangeClientes()
            {
                this.Chart.LegendY = "Reservas";
                this.Chart.LegendX = "Clientes";
                var recuentoPorCliente = Reserva.ContarReservasPorCliente(listaDeReservas);
                this.Chart.Values = recuentoPorCliente.reservasPorCliente;
                this.Chart.Labels = recuentoPorCliente.clientes;
                this.Chart.Draw();
            }
    
            void ChangeGeneral()
            {
                this.Chart.LegendY = "Reservas";
                this.Chart.LegendX = "Meses";
                
                var recuentoPorMes = Reserva.ContarReservasPorMes(listaDeReservas);
                this.Chart.Values = recuentoPorMes;
                this.Chart.Labels = new []{ "En", "Fb", "Ma", "Ab", "My", "Jn", "Jl", "Ag", "Sp", "Oc", "Nv", "Dc" };
                this.Chart.Draw();
            }
    
            void OnChartFormatChanged()
            {
                var edThickness = this.FindControl<NumericUpDown>( "EdThickness" );
                
                if ( this.Chart.Type == Chart.ChartType.Bars ) {
                    this.Chart.Type = Chart.ChartType.Lines;
                    this.Chart.DataPen = new Pen( Brushes.Red, 2 * (double)edThickness.Value );
                } else {
                    this.Chart.Type = Chart.ChartType.Bars;
                    this.Chart.DataPen = new Pen( Brushes.Navy, 20 * (double)edThickness.Value );
                }
                
                this.Chart.Draw();
            }
    
            void OnChartThicknessChanged(double thickness)
            {
                if ( this.Chart.Type == Chart.ChartType.Bars ) {
                    this.Chart.DataPen = new Pen( this.Chart.DataPen.Brush, 20 * thickness );
                } else {
                    this.Chart.DataPen = new Pen( this.Chart.DataPen.Brush, 2 * thickness );
                }
                
                this.Chart.AxisPen = new Pen( this.Chart.AxisPen.Brush, 4 * thickness );
                this.Chart.Draw();
            }
    
            void OnFontsStyleChanged()
            {
                var cbBold = this.FindControl<CheckBox>( "CbBold" );
                var cbItalic = this.FindControl<CheckBox>( "CbItalic" );
                bool italic = cbItalic.IsChecked ?? false;
                bool bold = cbBold.IsChecked ?? false;
                FontStyle style = italic ? FontStyle.Italic : FontStyle.Normal;
                FontWeight weight = bold ? FontWeight.Bold : FontWeight.Normal;
    
                this.Chart.DataFont = new Chart.Font( this.Chart.DataFont.Size ) {
                    Family = this.Chart.DataFont.Family,
                    Style = style,
                    Weight = weight
                };
                
                this.Chart.LabelFont = new Chart.Font( this.Chart.LabelFont.Size ) {
                    Family = this.Chart.LabelFont.Family,
                    Style = style,
                    Weight = weight
                };
                
                this.Chart.LegendFont = new Chart.Font( this.Chart.LegendFont.Size ) {
                    Family = this.Chart.LegendFont.Family,
                    Style = style,
                    Weight = weight
                };
                
                this.Chart.Draw();
            }
    
            void InitializeComponent()
            {
                AvaloniaXamlLoader.Load(this);
            }
            
            Chart Chart { get; }
    
            static Cliente cliente1 = new Cliente { Nombre = "Juan Pérez" };
            static Cliente cliente2 = new Cliente { Nombre = "María Rodríguez" };
            static Cliente cliente3 = new Cliente { Nombre = "Carlos López" };
            static Cliente cliente4 = new Cliente { Nombre = "Ana García" };
            static Cliente cliente5 = new Cliente { Nombre = "Pedro Martínez" };
            
             IList<Reserva> listaDeReservas = new List<Reserva> {
                new Reserva { ID = 1, Cliente = cliente1, Tipo = Habitacion.TipoHabitacion.Individual, Entrada = new DateTime(2023, 1, 15), Salida = new DateTime(2023, 3, 20), Garaje = true, Importe = 100, IVA = 21 },
                new Reserva { ID = 2, Cliente = cliente1, Tipo = Habitacion.TipoHabitacion.Doble, Entrada = new DateTime(2023, 5, 10), Salida = new DateTime(2023, 5, 15), Garaje = false, Importe = 120, IVA = 21 },
                new Reserva { ID = 3, Cliente = cliente1, Tipo = Habitacion.TipoHabitacion.Matrimonial, Entrada = new DateTime(2023, 7, 1), Salida = new DateTime(2023, 7, 7), Garaje = true, Importe = 200, IVA = 21 },
                new Reserva { ID = 4, Cliente = cliente2, Tipo = Habitacion.TipoHabitacion.Individual, Entrada = new DateTime(2023, 12, 12), Salida = new DateTime(2023, 8, 20), Garaje = false, Importe = 110, IVA = 21 },
                new Reserva { ID = 5, Cliente = cliente2, Tipo = Habitacion.TipoHabitacion.Doble, Entrada = new DateTime(2023, 12, 5), Salida = new DateTime(2023, 10, 10), Garaje = true, Importe = 130, IVA = 21 },
                new Reserva { ID = 6, Cliente = cliente2, Tipo = Habitacion.TipoHabitacion.Matrimonial, Entrada = new DateTime(2023, 12, 3), Salida = new DateTime(2023, 12, 10), Garaje = false, Importe = 220, IVA = 21 },
                new Reserva { ID = 7, Cliente = cliente3, Tipo = Habitacion.TipoHabitacion.Individual, Entrada = new DateTime(2023, 5, 8), Salida = new DateTime(2023, 2, 15), Garaje = true, Importe = 90, IVA = 21 },
                new Reserva { ID = 8, Cliente = cliente3, Tipo = Habitacion.TipoHabitacion.Doble, Entrada = new DateTime(2023, 9, 18), Salida = new DateTime(2023, 4, 25), Garaje = false, Importe = 110, IVA = 21 },
                new Reserva { ID = 9, Cliente = cliente3, Tipo = Habitacion.TipoHabitacion.Matrimonial, Entrada = new DateTime(2023, 6, 5), Salida = new DateTime(2023, 6, 12), Garaje = true, Importe = 180, IVA = 21 },
                new Reserva { ID = 10, Cliente =cliente4, Tipo = Habitacion.TipoHabitacion.Individual, Entrada = new DateTime(2023, 9, 21), Salida = new DateTime(2023, 9, 30), Garaje = false, Importe = 120, IVA = 21 },
                new Reserva { ID = 11, Cliente = cliente4, Tipo = Habitacion.TipoHabitacion.Doble, Entrada = new DateTime(2023, 1, 10), Salida = new DateTime(2023, 11, 18), Garaje = true, Importe = 140, IVA = 21 },
                new Reserva { ID = 12, Cliente = cliente5, Tipo = Habitacion.TipoHabitacion.Matrimonial, Entrada = new DateTime(2023, 12, 3), Salida = new DateTime(2023, 1, 10), Garaje = false, Importe = 200, IVA = 21 },
            };

            private IList<Habitacion> listaDeHabitaciones = new List<Habitacion>()
            {
                new Habitacion(Habitacion.TipoHabitacion.Matrimonial, 101, DateTimeOffset.Parse("2022-02-15"),
                    DateTime.Parse("2022-01-01"), new List<string> { "Wifi", "TV", "Baño" })

            // new Habitacion
            //     {
            //         ID = 201, Tipo = Habitacion.TipoHabitacion.Doble, UltimaRenovacion = DateTime.Parse("2022-03-01"),
            //         UltimaReserva = DateTime.Parse("2022-04-10"),
            //         Comodidades = new List<string> { "Caja fuerte", "Mini-bar", "TV" }
            //     },
            //     new Habitacion
            //     {
            //         ID = 301, Tipo = Habitacion.TipoHabitacion.Individual,
            //         UltimaRenovacion = DateTime.Parse("2022-05-15"), UltimaReserva = null,
            //         Comodidades = new List<string> { "Wifi", "Cocina" }
            //     },
            //     new Habitacion
            //     {
            //         ID = 401, Tipo = Habitacion.TipoHabitacion.Matrimonial,
            //         UltimaRenovacion = DateTime.Parse("2022-06-20"), UltimaReserva = DateTime.Parse("2022-07-05"),
            //         Comodidades = new List<string> { "Wifi", "Caja fuerte", "TV" }
            //     },
            //     new Habitacion
            //     {
            //         ID = 501, Tipo = Habitacion.TipoHabitacion.Doble, UltimaRenovacion = DateTime.Parse("2022-08-10"),
            //         UltimaReserva = DateTime.Parse("2022-09-15"),
            //         Comodidades = new List<string> { "Mini-bar", "Cocina" }
            //     },
            //     new Habitacion
            //     {
            //         ID = 601, Tipo = Habitacion.TipoHabitacion.Individual,
            //         UltimaRenovacion = DateTime.Parse("2022-10-20"), UltimaReserva = null,
            //         Comodidades = new List<string> { "Baño", "TV" }
            //     },
            //     new Habitacion
            //     {
            //         ID = 701, Tipo = Habitacion.TipoHabitacion.Matrimonial,
            //         UltimaRenovacion = DateTime.Parse("2022-11-30"), UltimaReserva = DateTime.Parse("2022-12-25"),
            //         Comodidades = new List<string> { "Wifi", "Mini-bar", "Cocina" }
            //     },
            //     new Habitacion
            //     {
            //         ID = 801, Tipo = Habitacion.TipoHabitacion.Doble, UltimaRenovacion = DateTime.Parse("2023-01-15"),
            //         UltimaReserva = null, Comodidades = new List<string> { "Caja fuerte", "Baño", "TV" }
            //     },
            //     new Habitacion
            //     {
            //         ID = 901, Tipo = Habitacion.TipoHabitacion.Individual,
            //         UltimaRenovacion = DateTime.Parse("2023-02-28"), UltimaReserva = null,
            //         Comodidades = new List<string> { "Wifi", "Cocina" }
            //     },
            //     new Habitacion
            //     {
            //         ID = 1001, Tipo = Habitacion.TipoHabitacion.Matrimonial,
            //         UltimaRenovacion = DateTime.Parse("2023-03-10"), UltimaReserva = null,
            //         Comodidades = new List<string> { "Caja fuerte", "TV" }
            //     }
            };


            private Registro<Habitacion> registroHabitaciones;
            
            
            public static List<int> ContarHabitacionesConComodidad(Registro<Habitacion> habitaciones)
            {
                List<int> listacomodidades = new List<int>(new int[6]);
                foreach (var habitacion in habitaciones.Elementos)
                {
                    if (habitacion.Comodidades.Contains("Wifi")) {listacomodidades[0]++; }
                    if (habitacion.Comodidades.Contains("Caja fuerte")) {listacomodidades[1]++; }
                    if (habitacion.Comodidades.Contains("Mini-bar")) { listacomodidades[2]++; }
                    if (habitacion.Comodidades.Contains("Baño")) { listacomodidades[3]++; }
                    if (habitacion.Comodidades.Contains("Cocina")) { listacomodidades[4]++; }
                    if (habitacion.Comodidades.Contains("TV")) { listacomodidades[5]++; }
            
                }
                return listacomodidades;
            }
}