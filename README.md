# PerfectMaze
An app that generates a maze with the *Depth-First* algorithm.

## Improvements
Even though the app is almost finished, I do see some improvements to make already. I'll describe them here down below.

### Maze
- The list of nodes is redundant. This is because the grid already has all the nodes stored.
- It would probably be a better idea to store _nodeSize_ in _GameSettings.cs_ or in _Node.cs_.
- In *PickNextNode*, there is some DRY code (whether the neighbor of the current node is available) that is easily changable by making an extra method.
- In *RemoveWalls*, an integer is given to delete the correct walls. This is not very readable. En enum _Walls_ could be added to _Node_ (for instance _Wall.Up, Wall.Down, Wall.Left_ and _Wall.Right_).
