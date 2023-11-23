using System;
using System.Xml;
using Avalonia.Controls;
using Hotel___Gestión_clientes.Core;

namespace Hotel___Gestión_clientes;

public partial class GestionClientes : Window
{
    private Registro<Cliente> _registro;
    private int _posActual;
    private bool _saveWhenClose;
    
    private enum Accion {Add, Delete, Edit, Selected};

    public GestionClientes()
    {
        // Leer el fichero XML desde disco
        try
        {
            this._registro = XmlController<Cliente>.Recuperar(XmlController<Cliente>.TipoRegistro.Clientes);
        }
        catch (XmlException e)
        {
            Console.WriteLine("Error al leer del fichero. Se comienza una sesión limpia." + e.Message);
            this._registro = new Registro<Cliente>();
        }

        // Por defecto, al cerrar la ventana se guarda la información en fichero XML
        this._saveWhenClose = true;

        // Inicio de código de IU Avalonia
        InitializeComponent();

        //Despliegue inicial de datos en la interfaz;
        OnStart();

        //Declaración de listeners de de los botones
        BtInserta.Click += (_, _) => this.OnInsertButtonClick();
        BtElimina.Click += (_, _) => this.OnDeleteButtonClick();
        BtModifica.Click += (_, _) => this.OnModifyButtonClick();

        //Declaración de listeners de de los botones del menú superior
        OpExit.Click += (_, _) => this.OnExitButtonClick();
        OpExitWithoutSave.Click += (_, _) => this.OnExitWithoutSaveButtonClick();
        OpSave.Click += (_, _) => this.OnSaveButtonClick();

        // Establecer listener para modificar el cliente mostrado cada vez que cambia el cliente seleccionado
        ListBoxClientes.SelectionChanged += OnItemSelected;

        //Nos suscribimos al evento Closed para guardar al cerrar la ventana de avalonia, incluso con la interfaz del SO.
        Closed += (_, _) => this.OnExit();

        //Establecer listener en campo DNI para calcular y establecer letra en la interfaz
        TextBoxDni.TextChanged += (_, _) => this.OnTextBoxDniChange();
    }

    /**
     * Método que se ejecuta al iniciar la ventana y se encarga de generar la presentación inicial de datos
     */
    private void OnStart()
    {
        // Inicialmente no hay seleccionado nada por defecto
        this._posActual = -1;
        NotifyDataSetChanged(Accion.Selected, this._posActual);

        // Generamos la lista de clientes
        ListBoxClientes.Items.Clear();
        foreach (Cliente cliente in _registro.Elementos)
        {
            ListBoxClientes.Items.Add(cliente.ToString());
        }
    }
    
    /**
     * Función que se lanza cada vez que se cambia la selección de la lista (selección o desselección)
     */
    private void OnItemSelected(object? sender, SelectionChangedEventArgs args)
    {
        if (sender is ListBox listBox)
        {
            this._posActual = listBox.SelectedIndex;
            MostrarCliente(this._posActual);
        }
    }
    
    /**
     * Método al que se llama cada vez que se realiza una acción que debe modificar la lista de alguna manera
     * sin interación del usuario
     */
    private void NotifyDataSetChanged(Accion accion, int posSeleccionada, int posAlterada = -1)
    {
        switch (accion)
        {
            case Accion.Add:
                ListBoxClientes.Items.Insert(posAlterada, this._registro.Get(posAlterada).ToString());
                break;
            case Accion.Edit:
                ListBoxClientes.Items[posAlterada] = this._registro.Get(posAlterada).ToString();
                break;
            case Accion.Delete:
                ListBoxClientes.Items.RemoveAt(posAlterada);
                break;
            case Accion.Selected:
                break;
            default:
                Console.Error.WriteLine("Error: Not implemented in NotifyDataSetChanged()");
                break;
        }
        ListBoxClientes.SelectedIndex = posSeleccionada;
    }

    private void OnExitButtonClick()
    {
        this.Close();
    }

    private void OnExitWithoutSaveButtonClick()
    {
        this._saveWhenClose = false;
        this.Close();
    }
    
    private void OnExit()
    {
        if (this._saveWhenClose)
        {
            OnSaveButtonClick();
        }
        // OnExitWithoutSaveButtonClick();
    }
    
    private void OnSaveButtonClick()
    {
        XmlController<Cliente>.Guardar(_registro, XmlController<Cliente>.TipoRegistro.Clientes);
    }
    
    /**
     * Se llama a este método al pulsar el botón de insertar en la interfaz
     */
    private void OnInsertButtonClick()
    {
        Cliente cliente = ObtenerCliente();
        if (cliente != null)
        {
            if (this._posActual < 0)
            {
                this._posActual = 0;
            }
            this._registro.Insert(this._posActual, cliente);
            NotifyDataSetChanged(Accion.Add, this._posActual, this._posActual);
        }
    }
    
    /**
     * Se llama a este método al pulsar el botón de eliminar en la interfaz
     */
    private void OnDeleteButtonClick()
    {
        if (this._posActual >= 0)
        {
            int posEliminada = this._posActual;
            this._registro.Elimina(posEliminada);
            // Si hemos borrado el último elemento de la lista, establecemos como posición el último tras el borrado.
            // Y si no quedan elementos, no seleccionamos ninguna posición -> _posActual = -1;
            if (this._posActual >= this._registro.Count)
            {
                this._posActual = this._registro.Count - 1;
            }
            NotifyDataSetChanged(Accion.Delete, this._posActual, posEliminada);
        }
    }
    
    /**
     * Se llama a este método al pulsar el botón de modificar en la interfaz
     */
    private void OnModifyButtonClick()
    {
        Cliente cliente = ObtenerCliente();
        if (cliente != null 
                && _posActual >= 0 
                && _posActual < _registro.Count) {
            this._registro.Modifica(_posActual, cliente);
            NotifyDataSetChanged(Accion.Edit, this._posActual, this._posActual);
        }
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
     * Recuperamos los datos de un cliente del registro y los mostramos en la interfaz si existe.
     * Si no existe el cliente, vaciamos la interfaz
     */
    private void MostrarCliente(int pos)
    {
        if (pos > -1)
        {
            // Mostrar los datos del cliente de la interfaz
            Cliente cliente = this._registro.Get(pos);
            
            TextBoxDni.Text = cliente.Dni.Substring(0, cliente.Dni.Length-1);
            TextBoxLetra.Text = cliente.Dni.Substring(cliente.Dni.Length-1, 1);
            TextBoxNombre.Text = cliente.Nombre;
            TextBoxEmail.Text = cliente.Email;
            TextBoxTelefono.Text = cliente.Telefono.ToString();
            TextBoxDireccionPostal.Text = cliente.DireccionPostal;
            
            LabelTotal.Content = $"Posición: {this._posActual+1} | Clientes: {this._registro.Count}";
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
    
    /**
     * Obtenemos los datos, creamos un objeto Cliente, y lo devolvemos
     */
    private Cliente ObtenerCliente()
    {
        Cliente toRet = null;

        try
        {
            if (TextBoxTelefono.Text != null)
                toRet = new Cliente
                {
                    Dni = TextBoxDni.Text + GestionClientes.CalcularLetraDNI_NIE(int.Parse(TextBoxDni.Text)),
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