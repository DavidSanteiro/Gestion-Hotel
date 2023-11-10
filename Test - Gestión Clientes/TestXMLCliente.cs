using Hotel___Gestión_clientes.Core;

namespace Test___Gestión_Clientes;

public class Tests
{
    private Cliente _cliente1;
    [SetUp]
    public void Setup()
    {
        this._cliente1 = new Cliente
        {
            Dni = "13621243E",
            Nombre = "Luis Miguel Alba",
            Telefono = 813739574,
            Email = "bcz5dsrzsi@netscape.net",
            DireccionPostal = "Rúa Doutor Temes Fernández, s/n, bajo"
        };
        Registro<Cliente> registro = new Registro<Cliente>();
        registro.Add(_cliente1);
        XmlController<Cliente>.Guardar(registro);
    }

    [Test]
    public void Test1()
    {
        Registro<Cliente> registro = XmlController<Cliente>.Recuperar();
        Cliente cliente1 = registro.Get(0);
        if (cliente1.Dni == this._cliente1.Dni
            && cliente1.Nombre == this._cliente1.Nombre
            && cliente1.Telefono == this._cliente1.Telefono
            && cliente1.Email == this._cliente1.Email
            && cliente1.DireccionPostal == this._cliente1.DireccionPostal)
        {
            Assert.Pass();
        }
        else
        {
            Assert.Fail();
        }
    }
}