using System;
using System.Collections.Generic;
using System.Linq;
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
                
                //List<int> recuentoPorMes = ContarReservasPorMes(DataController.reservas.Elementos);
                //this.Chart.Values = recuentoPorMes;
                //this.Chart.Labels = new []{ "En", "Fb", "Ma", "Ab", "My", "Jn", "Jl", "Ag", "Sp", "Oc", "Nv", "Dc" };
            }
    
            void ChangeComodidad()
            {
                this.Chart.LegendY = "Numero de habitaciones";
                this.Chart.LegendX = "Tipos de comodidades";
                var recuentoPorComodidades = ContarHabitacionesConComodidad(DataController.habitaciones);
                List<string> comodidadesPosibles = new List<string>{"Wifi", "Caja fuerte", "Mini-bar", "Baño", "Cocina", "TV"};
                this.Chart.Values = recuentoPorComodidades;
                this.Chart.Labels = comodidadesPosibles;
                this.Chart.Draw();
            }
    
            void ChangeHabitacion()
            {
                this.Chart.LegendY = "Reservas";
                this.Chart.LegendX = "Tipos de Habitacion";
                var recuentoPorHabitacion = ContarReservasPorHabitacion(DataController.reservas.Elementos);
                this.Chart.Values = recuentoPorHabitacion;
                this.Chart.Labels = new []{ "Individual", "Doble", "Matrimonial"};
                this.Chart.Draw();
            }
    
            void ChangeClientes()
            {
                this.Chart.LegendY = "Reservas";
                this.Chart.LegendX = "Clientes";
                var recuentoPorCliente = ContarReservasPorCliente(DataController.reservas.Elementos);
                this.Chart.Values = recuentoPorCliente.reservasPorCliente;
                this.Chart.Labels = recuentoPorCliente.clientes;
                this.Chart.Draw();
            }
    
            void ChangeGeneral()
            {
                this.Chart.LegendY = "Reservas";
                this.Chart.LegendX = "Meses";
                
                var recuentoPorMes = ContarReservasPorMes(DataController.reservas.Elementos);
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
            
            public static List<int> ContarReservasPorHabitacion(IList<Reserva> listaDeReservas)
            {
                List<int> reservasPorHabitacion = new List<int>(new int[3]);

                foreach (var reserva in listaDeReservas) 
                {
                    Habitacion.TipoHabitacion ind = Enum.Parse<Habitacion.TipoHabitacion>(reserva.Tipo);
                    if (ind == Habitacion.TipoHabitacion.Individual)
                    {
                        reservasPorHabitacion[0]++;
                    }
                    if (ind == Habitacion.TipoHabitacion.Doble)
                    {
                        reservasPorHabitacion[1]++;
                    }
                    if(ind == Habitacion.TipoHabitacion.Matrimonial)
                    {
                        reservasPorHabitacion[2]++;
                    }
                }
                return reservasPorHabitacion;
            }
            
            public static List<int> ContarReservasPorMes(IList<Reserva> listaDeReservas)
            {
                List<int> reservasPorMes = new List<int>(new int[12]);

                foreach (var reserva in listaDeReservas) 
                {
                    int mesDeEntrada = reserva.FechaEntrada.Month;
                    reservasPorMes[mesDeEntrada - 1]++;
                }
                return reservasPorMes;
            }
    
            public static (List<String> clientes, List<int> reservasPorCliente) ContarReservasPorCliente(IList<Reserva> listaDeReservas)
            {
                Dictionary<String, int> dict = new Dictionary<String, int>();
                foreach (var reserva in listaDeReservas)
                {
                    if (reserva.Cliente != null)
                    {
                        if (dict.ContainsKey(reserva.Cliente.Nombre))
                        {
                            dict[reserva.Cliente.Nombre]++;
                        }
                        else
                        {
                            dict[reserva.Cliente.Nombre] = 1;
                        }
                    }
                }
        
                List<String> clientes = dict.Keys.ToList();
                List<int> reservasPorCliente = dict.Values.ToList();

                return (clientes, reservasPorCliente);
            }
}