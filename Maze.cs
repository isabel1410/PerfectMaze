using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Maze : MonoBehaviour
{
    [Header("Maze Information")]
    [Tooltip("Node to be instantiated.")] [SerializeField] private Node _node;
    [Tooltip("The dimensions of the maze.")] [SerializeField] private Vector2Int _size;
    [Tooltip("Grid of nodes based on the size")] [SerializeField] private Node[,] _grid;
    [Tooltip("The size of each node (both x and y)")] [SerializeField] private int _nodeSize = 1;
    [Tooltip("The parent the nodes will instantiate on")] [SerializeField] private Transform _parent;
    [Tooltip("The entrance of the maze")] private Node _entranceNode;
    [Tooltip("The exit of the maze")] private Node _exitNode;

    [Header("Lists")]
    [Tooltip("List of all nodes in the maze")] [SerializeField] private List<Node> _nodes = new();
    [Tooltip("Stack of searched paths in maze")] [SerializeField] private Stack<Node> _searchedNodes = new();

    [Header("Settings")]
    [Tooltip("Reference to scriptable object that stores settings")] [SerializeField] private GameSettings _gameSettings;

    ///<summary>Gets called when user clicks on generate button. Instantiates the nodes on start with the dimensions of the maze. After that, the method will call methods to determine the begin and end point of the maze.</summary>
    public void InstantiateNodes()
    {
        // Set the size to what is set in the game settings
        _size = new Vector2Int((int)_gameSettings.Width, (int)_gameSettings.Height);

        // Initialize the grid array
        _grid = new Node[_size.x, _size.y];

        // Loop over each cell in the grid
        for (int row = 0; row < _size.x; row++)
        {
            for (int col = 0; col < _size.y; col++)
            {
                // Calculate the position of the cell in the grid
                Vector3 position = new(row * _nodeSize, col * _nodeSize, 0f);

                // Instantiate the prefab with a transform as parent
                Node newObject = Instantiate(_node, _parent);

                // Assigning localPosition and localRotation
                Quaternion spawnRotation = Quaternion.Euler(-90, 0, 0);
                newObject.transform.localPosition = position;
                newObject.transform.localRotation = spawnRotation;

                // Add the new object to the grid array
                _grid[row, col] = newObject;

                // Set the node to availabe
                newObject.SetColorBasedOfState(Node.State.Available);

                // Lastly, add the object to the list of nodes
                _nodes.Add(newObject);
            }
        }

        PickEntranceNode();
        PickExitNode();

        StartCoroutine(GenerateMaze());
    }

    ///<summary>Starts algorithm to generate the maze until there's no available node left to go to.</summary>
    private IEnumerator GenerateMaze()
    {
        // Loops through nodes until all have been visited
        while (_searchedNodes.Count > 0)
        {
            PickNextNode();
            yield return new WaitForSeconds(0.2f);
        }

        DeleteEntranceAndExitNodeWall();
        SetColorOfEntranceAndExitNode();
    }

    ///<summary>Picks the entrance of the maze. </summary>
    private void PickEntranceNode()
    {
        _entranceNode = _nodes[0];
        _entranceNode.SetColorBasedOfState(Node.State.Current);
        _searchedNodes.Push(_entranceNode);
    }

    ///<summary>Picks the exit of the maze, the last node that was added to the list.</summary>
    private void PickExitNode()
    {
        _exitNode = _nodes[^1];
    }

    ///<summary>Determines the next node to go to. </summary>
    private void PickNextNode()
    {
        // Get last visited node
        Node lastNode = _searchedNodes.Peek();

        // Add new list to keep track of neighbors and get position of last node
        List<Node> neighbors = new();

        Vector2Int lastPosition = GetPositionOfNodeInGrid(lastNode);

        // Check if left node is available
        if (lastPosition.x - _nodeSize >= 0)
        {
            Node neighbor = _grid[lastPosition.x - _nodeSize, lastPosition.y];
            bool isAvailable = neighbor.IsAvailable();

            if (isAvailable) neighbors.Add(neighbor);
        }
        // Check if right node is available
        if (lastPosition.x + _nodeSize < _size.x)
        {
            Node neighbor = _grid[lastPosition.x + _nodeSize, lastPosition.y];
            bool isAvailable = neighbor.IsAvailable();

            if (isAvailable) neighbors.Add(neighbor);
        }
        // Check if below node is available
        if (lastPosition.y - _nodeSize >= 0)
        {
            Node neighbor = _grid[lastPosition.x, lastPosition.y - _nodeSize];
            bool isAvailable = neighbor.IsAvailable();

            if (isAvailable) neighbors.Add(neighbor);
        }
        // Check if up node is available
        if (lastPosition.y + _nodeSize < _size.y)
        {
            Node neighbor = _grid[lastPosition.x, lastPosition.y + _nodeSize];
            bool isAvailable = neighbor.IsAvailable();

            if (isAvailable) neighbors.Add(neighbor);
        }

        // If there's an availabe node to go to...
        if (neighbors.Count > 0)
        {
            //...then choose a random node and add that to the stack
            int randomNode = Random.Range(0, neighbors.Count);
            Node newNode = neighbors[randomNode];
            _searchedNodes.Push(newNode);

            // Switch new node to current and last to visited
            newNode.SetColorBasedOfState(Node.State.Current);
            lastNode.SetColorBasedOfState(Node.State.Visited);

            // Remove the right walls
            RemoveWalls(newNode, lastNode);
        }
        //If not...
        else
        {
            //...go back in the maze by deleting the last node in path
            _searchedNodes.Pop();

            // Set this to current so that it is clear for the user where it's deleting nodes.
            lastNode.SetColorBasedOfState(Node.State.Current);
        }
    }

    ///<summary>Gets the position from a node in the grid</summary>
    /// <param name="node">Node to get position for.</param>
    /// <returns>Returns a position from the node.</returns>
    private Vector2Int GetPositionOfNodeInGrid(Node node)
    {
        // Loop through all the cells in the grid
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                // If the node at this position in the grid array matches the specified node, return its position
                if (_grid[x, y] == node)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        // If the specified node was not found in the grid, return an invalid position
        return new Vector2Int(-1, -1);
    }

    ///<summary>Removes walls based on distance between two nodes.</summary>
    /// <param name="currentNode">Node that the path is currently visiting.</param>
    /// <param name="lastNode">Node that the path had visited before visiting currentNode.</param>
    private void RemoveWalls(Node currentNode, Node lastNode)
    {
        // Get positions of the two nodes
        Vector2Int lastNodePosition = GetPositionOfNodeInGrid(lastNode);
        Vector2Int currentNodePosition = GetPositionOfNodeInGrid(currentNode);

        // Calculate x and y distance
        int distanceX = currentNodePosition.x - lastNodePosition.x;
        int distanceY = currentNodePosition.y - lastNodePosition.y;

        if (distanceX == _nodeSize)
        {
            currentNode.RemoveWall(1);
            lastNode.RemoveWall(0);
        }
        else if (distanceX == -_nodeSize)
        {
            currentNode.RemoveWall(0);
            lastNode.RemoveWall(1);
        }
        if (distanceY == _nodeSize)
        {
            currentNode.RemoveWall(3);
            lastNode.RemoveWall(2);
        }
        else if (distanceY == -_nodeSize)
        {
            currentNode.RemoveWall(2);
            lastNode.RemoveWall(3);
        }
    }

    ///<summary>Removes the wall to create an entrance and an exit.</summary>
    private void DeleteEntranceAndExitNodeWall()
    {
        _entranceNode.RemoveWall(1);
        _exitNode.RemoveWall(0);
    }

    ///<summary>Sets the entrance and exit to have a different color to make it more distinguishable</summary>
    private void SetColorOfEntranceAndExitNode()
    {
        _entranceNode.SetColorBasedOfState(Node.State.Point);
        _exitNode.SetColorBasedOfState(Node.State.Point);
    }

    ///<summary>Gets called when the user presses the stop generating button or the go back button.</summary>
    public void StopGenerating()
    {
        // Clear them so they can be filled with the correct information once the maze gets created again
        _searchedNodes.Clear();
        _nodes.Clear();
        DeleteGrid();
    }

    ///<summary>Deletes the grid one by one.</summary>
    private void DeleteGrid()
    {
        // Loop through all the cells in the grid
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Destroy(_grid[x, y].gameObject);
            }
        }
    }
}
