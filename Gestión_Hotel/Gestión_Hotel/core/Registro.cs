﻿using System;
using System.Collections.Generic;

namespace Gestión_Hotel.core;

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

    public int Count => this._elementos.Count;
    public IList<T> Elementos => this._elementos.AsReadOnly(); // OJO, no copia la lista

    public void Insert(int pos, T elem)
    {
        this._elementos.Insert(pos, elem);
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

    private readonly List<T> _elementos;
}