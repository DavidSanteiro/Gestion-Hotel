using System;
using System.Text.RegularExpressions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Gestión_Hotel.core;
using Gestión_Hotel.core.clientes;
using Gestión_Hotel.core.habitaciones;
using Gestión_Hotel.core.reservas;

namespace Gestión_Hotel.ui;

public partial class Reservas : UserControl, IUserControlCruds
{
    private int PosClienteSeleccionado = -1;
    private int PosHabitacionSeleccionado = -1;

    public Reservas()
    {
        InitializeComponent();

        CrudsController<Reserva> crudsController = new(this, ListBoxElementos, DataController.reservas);


        //Declaración de listeners de de los botones
        BtInserta.Click += (_, _) => crudsController.OnInsertButtonClick();
        BtElimina.Click += (_, _) => crudsController.OnDeleteButtonClick();
        BtModifica.Click += (_, _) => crudsController.OnModifyButtonClick();

        // Establecer listener para modificar la habitacion mostrada cada vez que cambia la habitacion seleccionada
        ListBoxElementos.SelectionChanged += crudsController.OnItemSelected;

        string DniClienteSeleccionado = "";
        int posClienteSeleccionado = -1;
        int IdHabitacionSelecionada = -1;
        int posHabitacionSeleccionada = -1;


        // primera llamada para que aparezcan en la primera iteracion en los combobox
        obtenerClientesComboBox();
        obtenerHabitacionesComboBox();

        // añadimos los elementos de clientes para elegir 


        var selClienteComboBox = this.FindControl<ComboBox>("SelCliente");

        selClienteComboBox.DropDownOpened += (sender, args) => { obtenerClientesComboBox(); };


        // determinar cual es el cliente que fue seleccionado
        selClienteComboBox.SelectionChanged += (sender, args) =>
        {
            // Verificar si hay elementos seleccionados
            if (selClienteComboBox.SelectedItem != null)
            {
                // Obtener el ítem seleccionado
                string clienteSeleccionado = (string)selClienteComboBox.SelectedItem;

                // Ahora puedes hacer algo con el cliente seleccionado, como imprimirlo en la consola
                //Console.WriteLine("Cliente seleccionado: " + clienteSeleccionado);

                // cogemos el ID del cliente seleccionado mediante regex, para ver su indice en datacontroller
                string pattern = @"DNI: ([^\s,]+),";
                Match match = Regex.Match(clienteSeleccionado, pattern);
                if (match.Success)
                {
                    // Obtiene el valor correspondiente al grupo capturado
                    string dniSeleccionado = match.Groups[1].Value;

                    // Imprime el resultado
                    //Console.WriteLine("DNI Seleccionado: " + dniSeleccionado);
                    DniClienteSeleccionado = dniSeleccionado;
                }
                else
                {
                    Console.WriteLine("No se encontró un DNI en el formato esperado.");
                }

                //determino la posicion del cliente
                for (int i = 0; i < DataController.clientes.Elementos.Count; i++)
                {
                    if (DataController.clientes.Elementos[i].Dni == DniClienteSeleccionado)
                    {
                        posClienteSeleccionado = i;
                        break; // Rompe el bucle una vez que encuentres el elemento
                    }
                }

                // Console.WriteLine(posClienteSeleccionado);
                this.PosClienteSeleccionado = posClienteSeleccionado;
            }
        };









        // añadimos los elementos de habitaciones para elegir 
        var selHabitacionComboBox = this.FindControl<ComboBox>("SelHabitacion");


        selHabitacionComboBox.DropDownOpened += (sender, args) => { obtenerHabitacionesComboBox(); };



        // determinar cual es la habitacion que fue seleccionada
        selHabitacionComboBox.SelectionChanged += (sender, args) =>
        {
            // Verificar si hay elementos seleccionados
            if (selHabitacionComboBox.SelectedItem != null)
            {
                // Obtener el ítem seleccionado
                string habitacionSeleccionado = (string)selHabitacionComboBox.SelectedItem;

                // Ahora puedes hacer algo con el cliente seleccionado, como imprimirlo en la consola
                //Console.WriteLine("Habitacion seleccionada: " + habitacionSeleccionado);

                // Actualizar la variable 'pos' en DataController.clientes
                // // Suponiendo que pos es la posición del cliente seleccionado en DataController.clientes.Elementos
                // DataController.pos = selClienteComboBox.Items.IndexOf(clienteSeleccionado);
                // cogemos el ID del cliente seleccionado mediante regex, para ver su indice en datacontroller
                string pattern = @"Habitacion (\d+),";
                Match match = Regex.Match(habitacionSeleccionado, pattern);
                if (match.Success)
                {
                    // Obtiene el valor correspondiente al grupo capturado
                    int Idhabitacion = int.Parse(match.Groups[1].Value);

                    // Imprime el resultado
                    //Console.WriteLine("Habitacion Seleccionado: " + Idhabitacion);
                    IdHabitacionSelecionada = Idhabitacion;
                }
                else
                {
                    Console.WriteLine("No se encontró la habitacion en el formato esperado.");
                }

                //determino la posicion del cliente
                for (int i = 0; i < DataController.habitaciones.Elementos.Count; i++)
                {
                    if (DataController.habitaciones.Elementos[i].ID == IdHabitacionSelecionada)
                    {
                        posHabitacionSeleccionada = i;
                        break; // Rompe el bucle una vez que encuentres el elemento
                    }
                }

                // Console.WriteLine(posHabitacionSeleccionada);
                this.PosHabitacionSeleccionado = posHabitacionSeleccionada;
            }
        };



    }

    private void obtenerClientesComboBox()
    {
        var selClienteComboBox = this.FindControl<ComboBox>("SelCliente");


        // Llama a la función para actualizar el ComboBox con la lista de clientes
        if (DataController.clientes != null)
        {
            // Limpia los elementos existentes (si los hay)
            selClienteComboBox.Items.Clear();

            // Agrega los elementos al ComboBox uno por uno
            foreach (var cliente in DataController.clientes.Elementos)
            {
                //Console.WriteLine(cliente.ToString());
                selClienteComboBox.Items.Add(cliente.ToString());
            }
        }

        ;
    }

    private void obtenerHabitacionesComboBox()
    {
        var selHabitacionComboBox = this.FindControl<ComboBox>("SelHabitacion");
        // Llama a la función para actualizar el ComboBox con la lista de habitaciones
        if (DataController.clientes != null)
        {
            // Limpia los elementos existentes (si los hay)
            selHabitacionComboBox.Items.Clear();

            // Agrega los elementos al ComboBox uno por uno
            foreach (var habitacion in DataController.habitaciones.Elementos)
            {
                // Console.WriteLine(habitacion.ToString());
                selHabitacionComboBox.Items.Add(habitacion.ToString());
            }
        }
    }

    public void MostrarElemento(int pos)
    {
        // Console.WriteLine("Mostrando elemento");

        if (pos > -1)
        {

            Reserva res = DataController.reservas.Get(pos);
            Cliente cli = res.Cliente;
            Habitacion hab = res.Habitacion;

            //Poner el cliente 
            var comboBoxCliente = this.FindControl<ComboBox>("SelCliente");
            AsignarComboBoxTest(comboBoxCliente, cli.ToString());

            //Poner el habitacion
            var comboBoxHabitacion = this.FindControl<ComboBox>("SelHabitacion");
            AsignarComboBoxTest(comboBoxHabitacion, hab.ToString());

            // Poner el tipo
            TextBoxTipo.Text = res.Tipo;

            // //Poner la fechaEntrada
            DatePicker fechaEntrada = this.FindControl<DatePicker>("TextBoxFechaEntrada");
            fechaEntrada.SelectedDate = res.FechaEntrada;
            // Console.WriteLine(res.FechaEntrada);


            // // //Poner fecha salida
            DatePicker fechaSalida = this.FindControl<DatePicker>("TextBoxFechaSalida");
            fechaSalida.SelectedDate = res.FechaSalida;

            // Poner garaje
            if (res.UsoGaraje == true)
            {
                CheckBoxUsoGaraje.IsChecked = true;
            }
            else
            {
                CheckBoxUsoGaraje.IsChecked = false;
            }


            //Poner el IVA
            TextBoxIvaAplicado.Text = res.IvaAplicado.ToString();

            //Poner el precio
            TextBoxImporteDia.Text = res.ImportePorDia.ToString();

        }
        else
        {
            // Si pos es -1 (no hay elementos), vaciar la interfaz

            // Vaciar el ComboBox de Cliente
            var comboBoxCliente = this.FindControl<ComboBox>("SelCliente");
            comboBoxCliente.SelectedIndex = -1;

            // Vaciar el ComboBox de Habitación
            var comboBoxHabitacion = this.FindControl<ComboBox>("SelHabitacion");
            comboBoxHabitacion.SelectedIndex = -1;

            // Vaciar el TextBox de Tipo
            TextBoxTipo.Text = string.Empty;

            // Vaciar el DatePicker de Fecha Entrada
            DatePicker fechaEntrada = this.FindControl<DatePicker>("TextBoxFechaEntrada");
            fechaEntrada.SelectedDate = null;

            // Vaciar el DatePicker de Fecha Salida
            DatePicker fechaSalida = this.FindControl<DatePicker>("TextBoxFechaSalida");
            fechaSalida.SelectedDate = null;

            // Desmarcar la CheckBox de Uso Garaje
            CheckBoxUsoGaraje.IsChecked = false;

            // Vaciar el TextBox de IVA
            TextBoxIvaAplicado.Text = string.Empty;

            // Vaciar el TextBox de Importe por Día
            TextBoxImporteDia.Text = string.Empty;
        }

    }

    private void AsignarComboBoxTest(ComboBox comboBox, string contenido)
    {

        foreach (var item in comboBox.Items)
        {
            if (item is string && ((string)item).Equals(contenido))
            {
                comboBox.SelectedItem = item;
                break;
            }
        }
    }


    public object? ObtenerElemento()
    {
        
        Reserva? toRet = null;

        try
        {
            string tipo;
            tipo = this.FindControl<TextBox>("TextBoxTipo").Text;
        
        
            DateTimeOffset fechaEntrada;
            var auxFechaEntrada = this.FindControl<DatePicker>("TextBoxFechaEntrada");
            fechaEntrada = auxFechaEntrada.SelectedDate ?? DateTimeOffset.Now;
            DateTime fechaEntradaDate = fechaEntrada.DateTime;
            if (fechaEntrada == null)
            {
                fechaEntradaDate = new DateTime();
            }    
        
            DateTimeOffset fechaSalida;
            var auxFechaSalida = this.FindControl<DatePicker>("TextBoxFechaSalida");
            fechaSalida = auxFechaSalida.SelectedDate ?? DateTime.Now;
            DateTime fechaSalidaDate = fechaSalida.DateTime;
            if (fechaSalidaDate == null)
            {
                fechaSalidaDate = new DateTime();
            } 
        
            // Verificar si el CheckBox está marcado
            CheckBox checkBoxUsoGaraje = this.FindControl<CheckBox>("CheckBoxUsoGaraje");
            bool usoGaraje = checkBoxUsoGaraje.IsChecked ?? false;
            
            decimal importeDia;
            importeDia = decimal.Parse(this.FindControl<TextBox>("TextBoxImporteDia").Text);
            
            decimal ivaAplicado;
            ivaAplicado = decimal.Parse(this.FindControl<TextBox>("TextBoxIvaAplicado").Text);

            return new Reserva(tipo, DataController.clientes.Elementos[PosClienteSeleccionado],
                DataController.habitaciones.Elementos[PosHabitacionSeleccionado], fechaEntradaDate, fechaSalidaDate,
                usoGaraje, importeDia, ivaAplicado);
        }
        catch
        {
            Console.WriteLine("No se han podido leer los datos. Introducelos correctamente");
        }

        return toRet;
    }
}