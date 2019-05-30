using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomParameter : MonoBehaviour
{
    public int lenght, width; //Largo y ancho de la sala, permite al generador saber donde instanciar la siguiente sala.

    public int numPuertas; //Numero de puertas, de esta forma el generador podrá saber cuantas salas debe generar.

    public enum Direcciones {Izquierda, Derecha, Arriba, Abajo, usada};

    public Direcciones[] direccionesPuertas; //Con este parametro, el generador sabe donde tiene que generar las siguientes salas.
}
