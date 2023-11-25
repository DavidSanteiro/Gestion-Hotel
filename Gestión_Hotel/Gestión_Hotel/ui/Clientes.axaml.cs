using System;
using Avalonia.Controls;
using Gestión_Hotel.core;
using Gestión_Hotel.core.clientes;

namespace Gestión_Hotel.ui;

public partial class Clientes : UserControl, IUserControlCruds
{
    public Clientes()
    {
        // Inicio de código de IU Avalonia
        InitializeComponent();

        //Despliegue inicial de datos en la interfaz;
        CrudsController<Cliente> crudsController = new(this, ListBoxElementos, DataController.clientes);

        //Declaración de listeners de de los botones
        BtInserta.Click += (_, _) => crudsController.OnInsertButtonClick();
        BtElimina.Click += (_, _) => crudsController.OnDeleteButtonClick();
        BtModifica.Click += (_, _) => crudsController.OnModifyButtonClick();

        // Establecer listener para modificar el cliente mostrado cada vez que cambia el cliente seleccionado
        ListBoxElementos.SelectionChanged += crudsController.OnItemSelected;

        //Establecer listener en campo DNI para calcular y establecer letra en la interfaz
        TextBoxDni.TextChanged += (_, _) => this.OnTextBoxDniChange();
    }

    public void MostrarElemento(int pos)
    {
        if (pos > -1)
        {
            // Mostrar los datos del cliente de la interfaz
            Cliente cliente = DataController.clientes.Get(pos);
            
            TextBoxDni.Text = cliente.Dni.Substring(0, cliente.Dni.Length-1);
            TextBoxLetra.Text = cliente.Dni.Substring(cliente.Dni.Length-1, 1);
            TextBoxNombre.Text = cliente.Nombre;
            TextBoxEmail.Text = cliente.Email;
            TextBoxTelefono.Text = cliente.Telefono.ToString();
            TextBoxDireccionPostal.Text = cliente.DireccionPostal;
            
            LabelTotal.Content = $"Posición: {pos+1} | Clientes: {DataController.clientes.Count}";
        }
        else
        {
            // Borramos los datos del cliente de la interfaz
            TextBoxDni.Text = "";
            TextBoxLetra.Text = "";
            TextBoxNombre.Text = "";
            TextBoxEmail.Text = "";
            TextBoxTelefono.Text = "";
            TextBoxDireccionPostal.Text = "";
            
            LabelTotal.Content = "";
        }
    }

    public object? ObtenerElemento()
    {
        Cliente? toRet = null;

        try
        {
            if (TextBoxTelefono.Text != null)
                toRet = new Cliente
                {
                    Dni = TextBoxDni.Text + Clientes.CalcularLetraDNI_NIE(int.Parse(TextBoxDni.Text)),
                    Nombre = TextBoxNombre.Text,
                    Telefono = int.Parse(TextBoxTelefono.Text),
                    Email = TextBoxEmail.Text,
                    DireccionPostal = TextBoxDireccionPostal.Text
                };
        }
        catch
        {
            Console.WriteLine("No se han podido leer los datos. Motivo: excepción al parsear los datos");
        }

        return toRet;
    }
    
     /**
     * Método que es invocado cada vez que cambia el textBox DNI. Sirve para actualizar la letra
     */
    private void OnTextBoxDniChange()
    {
        string? dni = TextBoxDni.Text;
        if (!string.IsNullOrEmpty(dni))
        {
            if (!int.TryParse(dni, out int numeroDni))
            {
                TextBoxLetra.Text = "no válido";
            }
            else
            {
                TextBoxLetra.Text = CalcularLetraDNI_NIE(numeroDni).ToString();
            }
        }
    }
    
    /**
     * Método estático para generar una letra dado un DNI determinado.
     */
    private static char CalcularLetraDNI_NIE(int numeroDni)
    {
        const string caracteresLetraDni = "TRWAGMYFPDXBNJZSQVHLCKE";

        // Calcular el índice correspondiente
        int indice = numeroDni % caracteresLetraDni.Length;

        // Obtener la letra correspondiente
        char letra = caracteresLetraDni[indice];

        return letra;
    }
}