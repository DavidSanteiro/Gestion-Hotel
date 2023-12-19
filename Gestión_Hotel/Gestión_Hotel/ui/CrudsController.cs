using System;
using System.Linq;
using Avalonia.Controls;
using Gestión_Hotel.core;
using Gestión_Hotel.core.habitaciones;

namespace Gestión_Hotel.ui;

public class CrudsController<T> where T : ISerializableXml<T>
{
    private enum Accion
    {
        Add,
        Delete,
        Edit,
        Selected
    };

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

        // Verificar si _listBoxClientes no es null antes de manipularlo
        if (_listBoxClientes is not null)
        {
            _listBoxClientes.Items?.Clear(); // Verifica si Items es null antes de intentar limpiar

            // Verificar si _registro y _registro.Elementos no son null antes de iterar sobre ellos
            if (_registro is not null && _registro.Elementos is not null)
            {
                foreach (T elemento in _registro.Elementos)
                {
                    _listBoxClientes.Items?.Add(elemento
                        .ToString()); // Verifica si Items es null antes de intentar agregar
                }
            }
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
     * Función que selecciona "artificialmente" un elemento de la lista y lo muestra
     */
    public void SelectItem(int pos)
    {
        if (pos < 0 || pos >= this._registro.Count)
        {
            throw new Exception("Cannot invalid exception");
        }

        this._posActual = pos;
        _specificClassInstance.MostrarElemento(this._posActual);
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
        T elemento = (T?)_specificClassInstance?.ObtenerElemento(); // Comprobación para _specificClassInstance

        if (elemento != null)
        {
            if (this._posActual < 0)
            {
                this._posActual = 0;
            }

            // Comprobación para _registro
            if (_registro != null)
            {
                this._registro.Insert(this._posActual, elemento);
                NotifyDataSetChanged(Accion.Add, this._posActual, this._posActual);
            }
            else
            {
                Console.Error.WriteLine("Error: _registro is null in OnInsertButtonClick()");
            }
        }
        else
        {
            Console.Error.WriteLine("Error: ObtenerElemento() returned null in OnInsertButtonClick()");
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
        T elemento = (T?)_specificClassInstance.ObtenerElemento();
        
        if (elemento != null
            && _posActual >= 0
            && _posActual < _registro.Count)
        {
            // la habitacion no debe permitir cambiar el tipo, comporbamos que no sea esa la modificacion y
            // además si no cambió el piso el ID debe ser el mismo
            if (typeof(T) == typeof(Habitacion))
            {
                Habitacion habitacionActual = _registro.Get(_posActual) as Habitacion;
                if (habitacionActual != null && habitacionActual.Tipo != (elemento as Habitacion)?.Tipo)
                {
                }

                if (habitacionActual.Piso == (elemento as Habitacion)?.Piso)
                {
                    (elemento as Habitacion).ID = habitacionActual.ID;
                }
            }

            this._registro.Modifica(_posActual, elemento);
            NotifyDataSetChanged(Accion.Edit, this._posActual, this._posActual);
        }
    }
}