using System;
using Avalonia.Controls;
using Gestión_Hotel.core;

namespace Gestión_Hotel.ui;

public class CrudsController<T> where T : ISerializableXml<T>
{
    private enum Accion {Add, Delete, Edit, Selected};

    private ListBox _listBoxClientes;
    private Registro<T> _registro;
    private int _posActual;

    private readonly IUserControlCruds _specificClassInstance;

    public CrudsController(IUserControlCruds specificClassInstance, ListBox listBoxClientes, Registro<T> registro)
    {
        _listBoxClientes = listBoxClientes;
        _registro = registro;
        _posActual = 0;
        _specificClassInstance = specificClassInstance;
        
        OnStart();
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
        _listBoxClientes.Items.Clear();
        foreach (T elemento in _registro.Elementos)
        {
            _listBoxClientes.Items.Add(elemento.ToString());
        }
    }
    
    /**
     * Función que se lanza cada vez que se cambia la selección de la lista (selección o deselección)
     */
    public void OnItemSelected(object? sender, SelectionChangedEventArgs args)
    {
        if (sender is ListBox listBox)
        {
            this._posActual = listBox.SelectedIndex;
            _specificClassInstance.MostrarElemento(this._posActual);
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
                _listBoxClientes.Items.Insert(posAlterada, this._registro.Get(posAlterada).ToString());
                break;
            case Accion.Edit:
                _listBoxClientes.Items[posAlterada] = this._registro.Get(posAlterada).ToString();
                break;
            case Accion.Delete:
                _listBoxClientes.Items.RemoveAt(posAlterada);
                break;
            case Accion.Selected:
                break;
            default:
                Console.Error.WriteLine("Error: Not implemented in NotifyDataSetChanged()");
                break;
        }
        _listBoxClientes.SelectedIndex = posSeleccionada;
    }
    
    /**
     * Se llama a este método al pulsar el botón de insertar en la interfaz
     */
    public void OnInsertButtonClick()
    {
        T elemento = (T?) _specificClassInstance.ObtenerElemento();
        
        if (elemento != null)
        {
            if (this._posActual < 0)
            {
                this._posActual = 0;
            }
            this._registro.Insert(this._posActual, elemento);
            NotifyDataSetChanged(Accion.Add, this._posActual, this._posActual);
        }
    }
    
    /**
     * Se llama a este método al pulsar el botón de eliminar en la interfaz
     */
    public void OnDeleteButtonClick()
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
    public void OnModifyButtonClick()
    {
        T elemento = (T?) _specificClassInstance.ObtenerElemento();
        if (elemento != null 
                && _posActual >= 0 
                && _posActual < _registro.Count) {
            this._registro.Modifica(_posActual, elemento);
            NotifyDataSetChanged(Accion.Edit, this._posActual, this._posActual);
        }
    }
}