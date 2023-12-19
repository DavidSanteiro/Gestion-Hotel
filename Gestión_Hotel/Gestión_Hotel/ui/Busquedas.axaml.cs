using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Gestión_Hotel.core;
using Gestión_Hotel.core.busquedas;
using Gestión_Hotel.core.habitaciones;
using Gestión_Hotel.core.reservas;

namespace Gestión_Hotel.ui;

public partial class Busquedas : UserControl
{
    public Busquedas()
    {
        InitializeComponent();
        
        //Views
        this._vpendientes = this.FindControl<StackPanel>("Vpendientes");
        this._vpersona = this.FindControl<StackPanel>("Vpersona");
        this._vhabitacion = this.FindControl<StackPanel>("Vhabitacion");
        this._vdisponibilidad = this.FindControl<StackPanel>("Vdisponibilidad");
        this._vocupacion = this.FindControl<StackPanel>("Vocupacion");
        
        //Events
        this.Mpendientes.Click += (_, _) => this.ViewPendientes();
        this.Mpersona.Click += (_, _) => this.ViewPersona();
        this.Mhabitacion.Click += (_, _) => this.ViewHabitacion();
        this.Mdisponibilidad.Click += (_, _) => this.ViewDisponibilidad();
        this.Mocupacion.Click += (_, _) => this.ViewOcupacion();
        
        this.BtnPendientes.Click += (_, _) => this.BuscarPendientes();
        this.BtnPersona.Click += (_, _) => this.BuscarPersona();
        this.BtnHabitacion.Click += (_, _) => this.BuscarHabitacion();
        this.BtnDisponibilidad.Click += (_, _) => this.BuscarDisponibilidad();
        this.BtnOcupacion.Click += (_, _) => this.BuscarOcupacion();
        this.BtnOcupacion2.Click += (_, _) => this.BuscarOcupacion2();
    }
    
    // Búsquedas
    private void BuscarPendientes()
    {
        var tbHabitacion = this.FindControl<TextBox>("TbHabitacion");
        List<Reserva> reservas;
        ListBoxElementos.Items.Clear();
        
        if (tbHabitacion?.Text is null or "")
        {
            reservas = Busqueda.ReservasPendientes(DataController.reservas.Elementos);
        }
        else
        {
            reservas= Busqueda.ReservasPendientes(DataController.reservas.Elementos, int.Parse(tbHabitacion.Text));
        }
        
        foreach(Reserva reserva in reservas)
        {
            ListBoxElementos.Items.Add(reserva.ToString());
        }
    }

    private void BuscarPersona()
    {
        var cbReserva = this.FindControl<ComboBox>("CbReserva");
        var tbDni = this.FindControl<TextBox>("TbDNI");
        List<Reserva> reservas;
        ListBoxElementos.Items.Clear();
        
        switch (cbReserva?.SelectedIndex)
        {
            case 0:
                reservas = Busqueda.ReservasPorPersona(tbDni.Text,DataController.reservas.Elementos, "Todas");
                break;
            case 1:
                reservas = Busqueda.ReservasPorPersona(tbDni.Text,DataController.reservas.Elementos, "Pendientes");
                break;
            case 2:
                reservas = Busqueda.ReservasPorPersona(tbDni.Text,DataController.reservas.Elementos, "Pasadas");
                break;
            default:
                reservas = null;
                break;
        }

        if (reservas != null)
            foreach (Reserva reserva in reservas)
            {
                ListBoxElementos.Items.Add(reserva.ToString());
            }
    }

    private void BuscarHabitacion()
    {
        var cbReserva = this.FindControl<ComboBox>("CbReserva2");
        var tbHabitacion = this.FindControl<TextBox>("TbHabitacion2");
        string tipo = "";
        List<Reserva> reservas;
        ListBoxElementos.Items.Clear();
        
        switch (cbReserva?.SelectedIndex)
        {
            case 0:
                tipo = "Pendientes";
                break;
            case 1:
                tipo = "Pasadas";
                break;
            default:
                break;
        }
        
        if (tbHabitacion?.Text is null or "")
        {
            reservas = Busqueda.ReservasHotel(DataController.reservas.Elementos, tipo);
        }
        else
        {
            reservas = Busqueda.ReservasPorHabitacion(DataController.reservas.Elementos, tipo,int.Parse(tbHabitacion.Text));
        }
        
        foreach(Reserva reserva in reservas)
        {
            ListBoxElementos.Items.Add(reserva.ToString());
        }
    }

    private void BuscarDisponibilidad()
    {
        var tbDisponibilidad = this.FindControl<TextBox>("TbDisponibilidad");
        List<Habitacion> habitaciones;
        ListBoxElementos.Items.Clear();
        
        if (tbDisponibilidad?.Text is null or "")
        {
            habitaciones = Busqueda.HabitacionesVacias(DataController.reservas.Elementos, DataController.habitaciones.Elementos);
        }
        else
        {
            habitaciones = Busqueda.HabitacionesVacias(DataController.reservas.Elementos, DataController.habitaciones.Elementos,
                                                        int.Parse(tbDisponibilidad.Text));
        }
        
        foreach(Habitacion habitacion in habitaciones)
        {
            ListBoxElementos.Items.Add(habitacion.ToString());
        }
    }

    private void BuscarOcupacion()
    {
        var dtPicker = this.FindControl<DatePicker>("dtPicker");
        List<Habitacion> habitaciones = null;
        ListBoxElementos.Items.Clear();
        
        if (dtPicker != null && dtPicker.SelectedDate != null)
        {
            habitaciones = Busqueda.OcupacionFecha(dtPicker.SelectedDate.Value, DataController.reservas.Elementos, DataController.habitaciones.Elementos);
        }

        if (habitaciones != null)
            foreach (Habitacion habitacion in habitaciones)
            {
                ListBoxElementos.Items.Add(habitacion.ToString());
            }
    }
    
    private void BuscarOcupacion2()
    {
        var dtPicker = this.FindControl<DatePicker>("dtPicker");
        List<Habitacion> habitaciones = null;
        ListBoxElementos.Items.Clear();
        
        if (dtPicker != null && dtPicker.SelectedDate != null)
        {
            DateTimeOffset fecha = dtPicker.SelectedDate.Value;
            habitaciones = Busqueda.OcupacionAnho(fecha.Year, DataController.reservas.Elementos, DataController.habitaciones.Elementos);
        }

        if (habitaciones != null)
            foreach (Habitacion habitacion in habitaciones)
            {
                ListBoxElementos.Items.Add(habitacion.ToString());
            }
    }

    // CAMBIO DE VISTAS
    private void ViewPendientes()
    {
        this._vpendientes.IsVisible = true;
        this._vpersona.IsVisible = false;
        this._vhabitacion.IsVisible = false;
        this._vdisponibilidad.IsVisible = false;
        this._vocupacion.IsVisible = false;
        ClearContents();
    }
    
    private void ViewPersona()
    {
        this._vpendientes.IsVisible = false;
        this._vpersona.IsVisible = true;
        this._vhabitacion.IsVisible = false;
        this._vdisponibilidad.IsVisible = false;
        this._vocupacion.IsVisible = false;
        ClearContents();
    }
    
    private void ViewHabitacion()
    {
        this._vpendientes.IsVisible = false;
        this._vpersona.IsVisible = false;
        this._vhabitacion.IsVisible = true;
        this._vdisponibilidad.IsVisible = false;
        this._vocupacion.IsVisible = false;
        ClearContents();
    }
    
    private void ViewDisponibilidad()
    {
        this._vpendientes.IsVisible = false;
        this._vpersona.IsVisible = false;
        this._vhabitacion.IsVisible = false;
        this._vdisponibilidad.IsVisible = true;
        this._vocupacion.IsVisible = false;
        ClearContents();
    }
    
    private void ViewOcupacion()
    {
        this._vpendientes.IsVisible = false;
        this._vpersona.IsVisible = false;
        this._vhabitacion.IsVisible = false;
        this._vdisponibilidad.IsVisible = false;
        this._vocupacion.IsVisible = true;
        ClearContents();
    }

    private void ClearContents()
    {
        var tbHabitacion = this.FindControl<TextBox>("TbHabitacion");
        var tbDisponibilidad = this.FindControl<TextBox>("TbDisponibilidad");
        var tbHabitacion2 = this.FindControl<TextBox>("TbHabitacion2");
        var tbDNI = this.FindControl<TextBox>("TbDNI");
        var dtPicker = this.FindControl<DatePicker>("dtPicker");
        ListBoxElementos.Items.Clear();
        tbHabitacion.Text = "";
        tbDisponibilidad.Text = "";
        tbDNI.Text = "";
        tbHabitacion2.Text = "";
        dtPicker.SelectedDate = null;
    }
    
    // views
    private StackPanel? _vpendientes;
    private StackPanel? _vpersona;
    private StackPanel? _vhabitacion;
    private StackPanel? _vdisponibilidad;
    private StackPanel? _vocupacion;
}