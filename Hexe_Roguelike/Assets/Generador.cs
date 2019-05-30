using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generador : MonoBehaviour
{
    public GameObject[] rooms; //Instancias de las salas generadas.
    public GameObject parent; //Objeto padre de las salas, para ordenar simplemente.

    public Material final, inicio;
    private GameObject roomElegida; //Habitación elegida para la nueva instancia.
    public int salasAGenerar; //Valor de la cantidad de salas a generar.
    private int salasGeneradas; //Valor privado que contabiliza las salas generadas.
    private GameObject prevRoom, nextRoom; // GameObject de la sala instanciada, se usará como referencia para la nueva sala.

    public Vector3 posNewRoom; //Posicion donde irá la nueva sala.

    public float tiempoGeneración;
    void Start()
    {
        salasGeneradas = 0;
        posNewRoom = Vector3.zero;
        roomElegida = rooms[Random.Range(1, rooms.Length)];
        nextRoom = Instantiate(roomElegida, posNewRoom, Quaternion.identity, parent.transform);
        nextRoom.gameObject.GetComponent<MeshRenderer>().material = inicio;

        salasGeneradas++;

        StartCoroutine(GenerarSalas());

    }
    IEnumerator GenerarSalas()
    {
        while (salasGeneradas < salasAGenerar)
        {
            float x1 = 0, y1 = 0, x2 = 0, y2 = 0; //Tamaños de las salas anterior y siguiente para decidir cual instanciar.
            RoomParameter prevParameter, nextParameter; //Parametros de las salas anterior y siguiente.

            RoomParameter.Direcciones direccion, direccionSalaNecesaria; //Valor que decide la dirección de la siguiente sala.
            bool salaCorrecta;

            salaCorrecta = false;
            prevRoom = nextRoom;
            prevParameter = prevRoom.GetComponent<RoomParameter>();

            //TAMAÑO X e Y DE LA SALA ANTERIOR.
            x1 = prevParameter.width;
            y1 = prevParameter.lenght;
            direccion = RoomParameter.Direcciones.usada;
            direccionSalaNecesaria = RoomParameter.Direcciones.usada;

            while (direccion == RoomParameter.Direcciones.usada)
            {
                direccion = prevParameter.direccionesPuertas[Random.Range(0, prevParameter.direccionesPuertas.Length)]; //Decide que dirección usar para el camino.
            }

            if (direccion != RoomParameter.Direcciones.usada) switch (direccion)
                {
                    case RoomParameter.Direcciones.Abajo:
                        direccionSalaNecesaria = RoomParameter.Direcciones.Arriba;
                        break;
                    case RoomParameter.Direcciones.Arriba:
                        direccionSalaNecesaria = RoomParameter.Direcciones.Abajo;
                        break;
                    case RoomParameter.Direcciones.Izquierda:
                        direccionSalaNecesaria = RoomParameter.Direcciones.Derecha;
                        break;
                    case RoomParameter.Direcciones.Derecha:
                        direccionSalaNecesaria = RoomParameter.Direcciones.Izquierda;
                        break;
                    default:
                        Debug.Log("Error, la dirección está establecida como 'usada'");
                        break;
                } // Este switch marca la puerta necesaria de la siguiente habitación en base a la anterior.ç

            while (salaCorrecta != true)
            {
                GameObject candidato = rooms[Random.Range(1, rooms.Length)];
                RoomParameter parameterCandidato = candidato.GetComponent<RoomParameter>();

                for (int i = 0; i < parameterCandidato.direccionesPuertas.Length; i++)
                {
                    if (parameterCandidato.direccionesPuertas[i] == direccionSalaNecesaria)
                    {
                        salaCorrecta = true;
                        nextRoom = candidato;
                        //parameterCandidato.direccionesPuertas[i] = RoomParameter.Direcciones.usada;
                    }
                }

            } //Compara candidato a candidato hasta encontrar una sala compatible.

            if (nextRoom != null)
            {
                nextParameter = nextRoom.GetComponent<RoomParameter>();
                x2 = nextParameter.width;
                y2 = nextParameter.lenght;
            }
            else Debug.Log("No se ha seleccionado la siguiente sala correctamente");

            switch (direccion) //Establece la posición donde se instanciará la nueva sala.
            {
                case RoomParameter.Direcciones.Abajo:
                    posNewRoom = new Vector3(posNewRoom.x, posNewRoom.y, posNewRoom.z - (y1 / 2 + y2 / 2));
                    break;
                case RoomParameter.Direcciones.Arriba:
                    posNewRoom = new Vector3(posNewRoom.x, posNewRoom.y, posNewRoom.z + (y1 / 2 + y2 / 2));
                    break;
                case RoomParameter.Direcciones.Izquierda:
                    posNewRoom = new Vector3(posNewRoom.x - (x1 / 2 + x2 / 2), posNewRoom.y, posNewRoom.z);
                    break;
                case RoomParameter.Direcciones.Derecha:
                    posNewRoom = new Vector3(posNewRoom.x + (x1 / 2 + x2 / 2), posNewRoom.y, posNewRoom.z);
                    break;

                default:
                    Debug.Log("Error, la dirección está establecida como 'usada'");
                    break;
            }

            GameObject instanciaRoom = Instantiate(nextRoom, posNewRoom, Quaternion.identity, parent.transform);
            salasGeneradas++;
            if (salasGeneradas == salasAGenerar) instanciaRoom.GetComponent<MeshRenderer>().material = final;
            yield return new WaitForSeconds(tiempoGeneración);
        }
    }
}
