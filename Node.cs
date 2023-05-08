using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Tooltip("Array of walls around the node")] [SerializeField] private GameObject[] _walls;
    [Tooltip("The floor of the node")] [SerializeField] private MeshRenderer _floor;
    [Tooltip("Enum to keep track of the current state of the node")] [SerializeField] private State _state;

    public enum State
    { 
        Available, 
        Current,
        Visited,
        Point
    }

    ///<summary>Sets the state of the node and changes the color</summary>
    /// <param name="state">State the node needs to change in</param>
    public void SetColorBasedOfState(State state)
    {
        switch (state)
        {
            case State.Available:
                _state = State.Available; _floor.material.color = Color.white; break;
            case State.Current:
                _state = State.Current; _floor.material.color = Color.green; break;
            case State.Visited:
                _state = State.Visited; _floor.material.color = Color.blue; break;
            case State.Point:
                _state = State.Visited; _floor.material.color = Color.red; break;
        }
    }

    ///<summary>Determines wheter this node is available</summary>
    ///<returns>True if available, false if not</returns>
    public bool IsAvailable()
    {
        return _state == State.Available;
    }

    ///<summary>Disables the walls that would otherwise block the generated path</summary>
    /// <param name="wallToRemove">Integer of the wall to remove in _walls</param>
    public void RemoveWall(int wallToRemove)
    {
        _walls[wallToRemove].gameObject.SetActive(false);
    }
}
