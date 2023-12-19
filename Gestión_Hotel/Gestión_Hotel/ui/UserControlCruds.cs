
namespace Gestión_Hotel.ui;

public interface IUserControlCruds
{
    /**
     * Despliega el elemento en la interfaz, mediante los inputs, textBox, radios, checkbos, ... habilitados para ello.
     *
     * Nota: Si la posición no es válida, se debe vaciar los inputs, radios, checkbox, ... El programa está diseñado
     * para devolver -1 cuando se requiera vaciar la interfaz
     */
    public void MostrarElemento(int pos);
    /**
     * Genera el elemento en la interfaz, mediante los inputs, textBox, radios, checkbos, ... habilitados para ello.
     * Se devuelve el cliente/habitación/reserva como object porque en C# una clase no puede ser genérica y heredar
     * de otra clase al mismo tiempo (si hacemos la clase genérica, no podemos heredar de UserControl)
     *
     * Nota: En caso de que el elemento no se pueda generar porque los datos no son correctos, se debe vaciar la interfaz 
     */
    public object? ObtenerElemento();
    
}