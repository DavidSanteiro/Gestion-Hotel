using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Gestión_Hotel.core;
using Gestión_Hotel.core.habitaciones;

namespace Gestión_Hotel.ui;

public partial class Habitaciones : UserControl, IUserControlCruds
{
    public Habitaciones()
    {
        // Inicio de código de IU Avalonia
        InitializeComponent();

        
        //Despliegue inicial de datos en la interfaz;
        CrudsController<Habitacion> crudsController = new(this, ListBoxElementos, DataController.habitaciones);

        //Declaración de listeners de de los botones
        BtInserta.Click += (_, _) => crudsController.OnInsertButtonClick();
        BtElimina.Click += (_, _) => crudsController.OnDeleteButtonClick();
        BtModifica.Click += (_, _) => crudsController.OnModifyButtonClick();

        // Establecer listener para modificar la habitacion mostrada cada vez que cambia la habitacion seleccionada
        ListBoxElementos.SelectionChanged += crudsController.OnItemSelected;

        //Establecer listener para consultar por ID
        BtConsulta.Click += (_, _) => this.OnConsulta();

        //Gestionar las comodidades
        comodidadesParaActualizar= new List<string>() { "Wifi","Mini Bar","Caja Fuerte","Baño","Cocina","TV" };

        //gestionar las comodidades
        comodidadesSeleccionadas = new List<string>();

    }

    public int Pos_Hab_ID(int id)
    {
        for (int i = 0; i < DataController.habitaciones.Count; i++)
        {
            if (DataController.habitaciones.Get(i).ID == id)
            {
                return i;
            }
        } 
        return -1;


    }
    void OnConsulta()
    {
        var BoxConsulta=this.FindControl<TextBox>("BoxConsulta");
        if (int.TryParse(BoxConsulta.Text, out var id))
        {
            int nueva_pos = Pos_Hab_ID(id);

            if (nueva_pos >= 0)
            {
               MostrarElemento(nueva_pos);
            }
        }
    }
    private List<string> comodidadesSeleccionadas;
    private List<string> comodidadesParaActualizar;

    public void MostrarElemento(int pos)
    {
        if (pos > -1)
        {
            //Poner el tipo 
            var Tipo = this.FindControl<ComboBox>("Tipo");
            AsignarAComboBox(Tipo, DataController.habitaciones.Get(pos).Tipo.ToString());

            //Poner el piso 
            var Piso = this.FindControl<ComboBox>("Piso");
            AsignarAComboBox(Piso, Convert.ToString(DataController.habitaciones.Get(pos).Piso));

            //Poner la ultimaReserva
            DatePicker ultimaReservaDatePicker = this.FindControl<DatePicker>("UltimaReserva");
            ultimaReservaDatePicker.SelectedDate = DataController.habitaciones.Get(pos).UltimaReserva.DateTime;

            //Poner la ultimaRenovacion
            DatePicker ultimaRenovacionDatePicker = this.FindControl<DatePicker>("UltimaRenovacion");
            ultimaRenovacionDatePicker.SelectedDate = DataController.habitaciones.Get(pos).UltimaRenovacion.DateTime;

            //Poner las comodidades
            // Recorre los CheckBox en el StackPanel y marca los que están en la lista de comodidades
            StackPanel stackPanelComodidades = this.FindControl<StackPanel>("Comodidades");



            stackPanelComodidades.Children.Clear();
        

            // Agrega CheckBox para cada comodidad en la lista
            foreach (var comodidad in comodidadesParaActualizar)
            {
                
                CheckBox checkBox;
                if (DataController.habitaciones.Get(pos).Comodidades.Contains(comodidad))
                {
                    checkBox = new CheckBox
                    {
                        Content = comodidad,
                        IsChecked = true // Puedes establecer el valor según tus necesidades
                    };

                }
                else
                {
                    checkBox = new CheckBox
                    {
                        Content = comodidad,
                        IsChecked = false // Puedes establecer el valor según tus necesidades
                    };
                }
                stackPanelComodidades.Children.Add(checkBox);
            }
            
            LabelTotal.Content = $"Posición: {pos+1} | Habitaciones: {DataController.habitaciones.Count}";
        }
        else
        {
            // Borramos los datos de la habitacion de la interfaz
            //Poner el tipo 
            var Tipo = this.FindControl<ComboBox>("Tipo");
            AsignarAComboBox(Tipo,"Matrimonial");

            //Poner el piso 
            var Piso = this.FindControl<ComboBox>("Piso");
            AsignarAComboBox(Piso, "1");

            //Poner la ultimaReserva
            DatePicker ultimaReservaDatePicker = this.FindControl<DatePicker>("UltimaReserva");
            ultimaReservaDatePicker.SelectedDate = DateTime.Now;

            //Poner la ultimaRenovacion
            DatePicker ultimaRenovacionDatePicker = this.FindControl<DatePicker>("UltimaRenovacion");
            ultimaRenovacionDatePicker.SelectedDate = DateTime.Now;

           
           



          
            
            LabelTotal.Content = "";
        }
        

    }

    public object? ObtenerElemento()
    {
        Habitacion.TipoHabitacion tipo;
        int piso;
        DateTimeOffset ultimaRenovacion;
        DateTimeOffset ultimaReserva;
        
        //Coger el tipo del combobox y asigarlo segun el valor seleccionado
        var Tipo = this.FindControl<ComboBox>("Tipo");
        var TipoSelectedItem = (ComboBoxItem)Tipo.SelectedItem;
        tipo = Enum.Parse<Habitacion.TipoHabitacion>(TipoSelectedItem?.Content.ToString());
       
        //Coger el piso del combobox y asigarlo segun el valor seleccionado
        var Piso = this.FindControl<ComboBox>("Piso");
        var PisoSelectedItem = (ComboBoxItem)Piso.SelectedItem;
        piso = Convert.ToInt32(PisoSelectedItem?.Content.ToString());
        
        //Coger el ultimaReserva  y asigar el valor seleccionado
        var ultimaReservaPicked = this.FindControl<DatePicker>("UltimaReserva");
        ultimaReserva = ultimaReservaPicked.SelectedDate ?? DateTime.Now;
         
        //Coger el ultimaRenovacion  y asigar el valor seleccionado
        var ultimaRenovacionPicked = this.FindControl<DatePicker>("UltimaRenovacion");
        ultimaRenovacion = ultimaRenovacionPicked.SelectedDate ?? DateTime.Now;
      
       
        int id = calcularID(piso);
       
     
        return new Habitacion(tipo, id, ultimaReserva, ultimaRenovacion,GetComodidadesSeleccionadas());
     
    }
    
    int calcularID(int piso)
    {
        
        IEnumerable<int> IdsPiso = DataController.habitaciones?.Elementos
            .Where(x => x.Piso == piso)
            .Select(x => x.ID)
            .OrderBy(x => x) ?? Enumerable.Empty<int>();
        
Console.WriteLine(String.Join(", ", IdsPiso));
        for (int i = 1; i <= 99; i++)
        {
            int nuevoID = (piso * 100) + i;
            
            if (!IdsPiso.Contains(nuevoID))
            {
                return nuevoID;
            }
        }

        return -1;
    }
    
    List<String> GetComodidadesSeleccionadas()
    {
        comodidadesSeleccionadas = new List<string>();
        StackPanel stackPanelComodidades = this.FindControl<StackPanel>("Comodidades");
        
        foreach (var child in stackPanelComodidades.Children)
        {
            if (child is CheckBox checkBox && checkBox.IsChecked == true)
            {
                comodidadesSeleccionadas.Add(checkBox.Content.ToString());
            }
        }

        return comodidadesSeleccionadas;
    }
    
    private void AsignarAComboBox(ComboBox comboBox, string tipo)
    {
        // Encuentra el ComboBoxItem que tiene el contenido igual al tipo
        ComboBoxItem selectedItem = null;
        foreach (var item in comboBox.Items)
        {
            if (item is ComboBoxItem comboBoxItem && comboBoxItem.Content.ToString() == tipo)
            {
                selectedItem = comboBoxItem;
                break;
            }
        }

        
        comboBox.SelectedItem = selectedItem;

        
    }
}