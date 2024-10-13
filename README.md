A Graphview based custom Behaviour Tree Creator for Unity

# Graph Based Behaviour Tree Creator
The Goal of this Behaviour Tree Editor is to allow Designers to create NPC behaviours using simple to understand Trees.
Programmers may add custom Nodes based on their Requirement that will reflect onto the Tree Editor.

### A Custom Graph View based Behavior Tree Creator using Nodes

https://github.com/user-attachments/assets/1baafc99-1694-46dc-ba45-46759aa52894

- The Editor allows you create Custom Behaviour Trees out of Nodes
- These Nodes maybe one of the following 4 types:
  - **Action Node**
  - **Decorator Node**
  - **Control Flow Node**
  - **Coroutine Node**

### Comes with an Inspector View and a Blackboard view 

https://github.com/user-attachments/assets/4f2ba87f-0cda-4b0e-b09e-b76e59117a90

- The Editor comes with an Inspector view to make realtime changes to Variables in each Node
- Every Tree has their own Blackboard that contains shared Variables between all Nodes in a tree
- The Editor also allows for basic functionalities like Delete, Undo and Redo for Nodes as well as Edges

### Realtime Tree Flow with Color Coding

https://github.com/user-attachments/assets/2995934a-7683-4c0d-9bcc-453fb988a63f

- Changes to Nodes and Blackboard can be made realtime through the Inspector View and Blackboard View
- The tree shows the current state of each node, viz — Running, Succeeded or Failed using color codes
- The tree's flow goes Depth First and Left to Right which can be changes realtime by moving the Nodes irrespective of when the Nodes were added to the tree
