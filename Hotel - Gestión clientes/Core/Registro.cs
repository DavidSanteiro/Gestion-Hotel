using System;
using System.Collections.Generic;

namespace Hotel___Gestión_clientes.Core;

public class Registro<T>
{
    public Registro()
    {
        this._elementos = new List<T>();
    }
    
    public Registro(IEnumerable<T> elementos) : this()
    {
        this._elementos.AddRange(elementos);
    }

    public int Count
    {
        get { return this._elementos.Count; }
    }

    public void Add(T elem)
    {
        this._elementos.Add(elem);
    }

    public void Elimina(int i)
    {
        if (i < 0
            || i >= this._elementos.Count)
        {
            throw new ArgumentException($"valor de {nameof(i)}: " + i);
        }

        this._elementos.RemoveAt(i);
    }
    
    public void Modifica(int i, T elem)
    {
        if (i < 0
            || i >= this._elementos.Count)
        {
            throw new ArgumentException($"valor de {nameof(i)}: " + i);
        }

        this._elementos[i] = elem;
    }

    public T Get(int i)
    {
        if (i < 0
            || i >= this._elementos.Count)
        {
            throw new ArgumentException($"valor de {nameof(i)}: " + i);
        }

        return this._elementos[i];
    }
    
    public IList<T> Elementos
    {
        get
        {
            return this._elementos.AsReadOnly(); // OJO, no copia la lista
        }
    }

    private List<T> _elementos;
}