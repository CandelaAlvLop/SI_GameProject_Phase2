using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePlayer : MonoBehaviour
{
    public Transform player1;  // Referencia al primer jugador
    public Transform player2;  // Referencia al segundo jugador
    public Transform mark1;    // Posici�n de la primera marca
    public Transform mark2;    // Posici�n de la segunda marca
    public float placementRadius = 6f;  // Radio de aceptaci�n para colocaci�n

    private bool isPlayer1Placed = false;
    private bool isPlayer2Placed = false;

    void Update()
    {
        if (!isPlayer1Placed)
        {
            isPlayer1Placed = CheckPlacement(player1, mark1, "Player 1");
        }

        if (!isPlayer2Placed)
        {
            isPlayer2Placed = CheckPlacement(player2, mark2, "Player 2");
        }

        if (isPlayer1Placed && isPlayer2Placed)
        {
            OnPlayersPlaced();
        }
    }

    bool CheckPlacement(Transform player, Transform mark, string playerName)
    {
        float distance = Vector3.Distance(player.position, mark.position);
        Debug.Log($"{playerName} distance to mark: {distance}");
        if (distance <= placementRadius)
        {
            Debug.Log($"{playerName} is correctly placed at {mark.name}");
            return true;
        }
        return false;
    }


    void OnPlayersPlaced()
    {
        Debug.Log("Both players are correctly placed!");
        // Aqu� puedes a�adir la l�gica adicional que necesites cuando ambos jugadores est�n en su lugar
    }
}