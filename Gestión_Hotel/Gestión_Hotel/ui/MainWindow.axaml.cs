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
        BtExit.Click += (_, _) => this.OnExitButtonClick();
        BtExitWithoutSave.Click += (_, _) => this.OnExitWithoutSaveButtonClick();
        BtSave.Click += (_, _) => this.OnSaveButtonClick();
        
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
    
}