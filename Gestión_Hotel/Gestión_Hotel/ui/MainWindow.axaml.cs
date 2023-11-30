using Avalonia.Controls;
using Gestión_Hotel.core;

namespace Gestión_Hotel;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        // Leer el fichero XML desde disco
        DataController.InitializeDataController();
        
        // Por defecto, al cerrar la ventana se guarda la información en fichero XML
        this._saveWhenClose = true;
        
        // Inicio de código de IU Avalonia
        InitializeComponent();
        
        //Nos suscribimos al evento Closed para guardar al cerrar la ventana de avalonia, incluso con la interfaz del SO.
        Closed += (_, _) => this.OnExit();
        
        //Declaración de listeners de de los botones del menú superior
        OpExit.Click += (_, _) => this.OnExitButtonClick();
        OpExitWithoutSave.Click += (_, _) => this.OnExitWithoutSaveButtonClick();
        OpSave.Click += (_, _) => this.OnSaveButtonClick();
        
        // //Declaración de listeners de las pestañas
        // TabItemReservas.Tapped += (_, _) => this.OnTapTabItemReservas();
        // TabItemHabitaciones.Tapped += (_, _) => this.OnTapTabItemHabitaciones();
        // TabItemClientes.Tapped += (_, _) => this.OnTapTabItemClientes();
        // TabItemGraficos.Tapped += (_, _) => this.OnTapTabItemGraficos();
        // TabItemBusquedas.Tapped += (_, _) => this.OnTapTabItemBusquedas();
        // TabItemAjustes.Tapped += (_, _) => this.OnTapTabItemAjustes();
    }

    // Acciones al cerrar la app
    private void OnExit()
    {
        if (this._saveWhenClose)
        {
            DataController.SaveData();
        }
    }
    
    // Acciones de los menús
    private void OnExitButtonClick()
    {
        this.Close();
    }
    
    private void OnExitWithoutSaveButtonClick()
    {
        this._saveWhenClose = false;
        this.Close();
    }
    
    private void OnSaveButtonClick()
    {
        DataController.SaveData();
    }
    
    private bool _saveWhenClose;
    
    // Acciones para inicializar el código de los distintos módulos
    // private void OnTapTabItemAjustes()
    // {
    //     // throw new System.NotImplementedException();
    // }
    //
    // private void OnTapTabItemBusquedas()
    // {
    //     // throw new System.NotImplementedException();
    // }
    //
    // private void OnTapTabItemGraficos()
    // {
    //     // throw new System.NotImplementedException();
    // }
    //
    // private void OnTapTabItemClientes()
    // {
    //     // throw new System.NotImplementedException();
    // }
    //
    // private void OnTapTabItemHabitaciones()
    // {
    //     // throw new System.NotImplementedException();
    // }
    //
    // private void OnTapTabItemReservas()
    // {
    //     // throw new System.NotImplementedException();
    // }
}