using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace GalEngine
{
    public class LogicGuiSystem : BehaviorSystem
    {
        private GuiComponentEventProperty mComponentEventProperty;

        public LogicGuiSystem() : base("LogicGuiSystem")
        {
            //add requirement component
            //gui logic system is used for solving the logic problem like mouse move, click event
            //we also need the visual gui component
            RequireComponents.AddRequireComponentType<LogicGuiComponent>();
            RequireComponents.AddRequireComponentType<VisualGuiComponent>();
            RequireComponents.AddRequireComponentType<TransformGuiComponent>();

            //init component event property
            mComponentEventProperty.CaptureControl = new GuiControl[3];
        }

        protected internal override void Excute(List<GameObject> passedGameObjectList)
        {
            //solve key board event, we need process all objects
            //it may trigger the input event for gui control
            void keyBoardSolver(List<GameObject> gameObjects, KeyBoardEvent eventArg, ref GuiComponentEventProperty eventProperty)
            {
                //only focus control can run input event
                //if there are no control get focus, we do not process the event
                if (eventProperty.FocusControl == null) return;

                //get logic component
                var logicComponent = eventProperty.FocusControl.GetComponent<LogicGuiComponent>();

                //if we remove the focus event part from logic component, we need reset the focus control in the event property
                if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.Focus))
                {
                    //if we add the input event part to logic component and the key code is down, we invoke event part
                    if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.Input) == true && eventArg.IsDown == true)
                        logicComponent.EventParts.Get(GuiComponentSupportEvent.Input).Invoke(eventProperty.FocusControl, 
                            new GuiComponentInputEvent(eventArg.Time, eventArg.KeyCode));
                }
                else eventProperty.FocusControl = null;
            }

            //solve mouse click event, we need process all objects
            //it may trigger the drag, focus, click event for gui control
            void mouseClickSolver(List<GameObject> gameObjects, MouseClickEvent eventArg, ref GuiComponentEventProperty eventProperty)
            {
                //when mouse button up, we need release the capture control and invoke click event
                if (eventArg.IsDown == false)
                {
                    var button = (uint)eventArg.Button;

                    //we have the control
                    if (eventProperty.CaptureControl[button] != null)
                    {
                        var logicComponent = eventProperty.CaptureControl[button].GetComponent<LogicGuiComponent>();

                        //invoke event
                        if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.MouseClick))
                            logicComponent.EventParts.Get(GuiComponentSupportEvent.MouseClick).
                                Invoke(eventProperty.CaptureControl[button],
                                new GuiComponentMouseClickEvent(eventArg.Time, eventArg.Position, eventArg.Button, eventArg.IsDown));

                        eventProperty.CaptureControl[button] = null;
                    }
                }

                //stack to maintain the path of game object's tree from node to root
                //matrix is the invert transform from root to node
                Stack<Tuple<GameObject, Matrix4x4>> ancestors = new Stack<Tuple<GameObject, Matrix4x4>>();

                //add virtual root to stack
                ancestors.Push(new Tuple<GameObject, Matrix4x4>(null, Matrix4x4.Identity));

                GuiControl newFocusControl = null;
                GuiControl newDragControl = null;
                GuiControl newCaptureControl = null;

                foreach (var gameObject in gameObjects)
                {
                    //maintain the elments in the stack are ancestors from root to node(with deep order, first element is root)
                    //because the list of game objects is the dfs order of tree
                    while (ancestors.Count != 1 && ancestors.Peek().Item1 != gameObject.Parent) ancestors.Pop();

                    //get the component of gui control
                    var logicComponent = gameObject.GetComponent<LogicGuiComponent>();
                    var visualComponent = gameObject.GetComponent<VisualGuiComponent>();
                    var transformComponent = gameObject.GetComponent<TransformGuiComponent>();

                    var invertTransform = Matrix4x4.Identity;

                    //invert the transform of game object
                    Matrix4x4.Invert(transformComponent.Transform, out invertTransform);

                    //get the invert transform from root to node
                    invertTransform = ancestors.Peek().Item2 * invertTransform;

                    //compute the mouse position in the local space of gui control
                    var mousePosition = Vector2.Transform(new Vector2(eventArg.Position.X, eventArg.Position.Y), invertTransform);
                    //test if the mouse position is in the gui control
                    var isContained = visualComponent.Shape.Contain(new Position<float>(mousePosition.X, mousePosition.Y));
                    
                    //for solving focus event, we need to discuss the left button status
                    if (eventArg.Button == MouseButton.Left)
                    {
                        //if left button is down and the control contains the mouse cursor
                        //we find the new focus control and new drag control
                        if (eventArg.IsDown == true && isContained == true)
                        {
                            newFocusControl = gameObject as GuiControl;

                            //if we are not dragging control, we can capture new drag control
                            //when left button is down and control contains the mouse cursor
                            if (eventProperty.DragControl == null)
                            {
                                newDragControl = gameObject as GuiControl;
                            }
                        }

                        //when we click the empty place, the focus will be lost
                        if (eventArg.IsDown == true && isContained == false && eventProperty.FocusControl == gameObject)
                        {
                            eventProperty.FocusControl = null;

                            //update the component focus status
                            if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.Focus))
                            {
                                logicComponent.EventParts.Get(GuiComponentSupportEvent.Focus).
                                    Invoke(gameObject as GuiControl, new GuiComponentFocusEvent(eventArg.Time, false));
                            }
                        }

                        //when we finish drag control
                        if (eventArg.IsDown == false && isContained == true && eventProperty.DragControl == gameObject)
                        {
                            eventProperty.DragControl = null;

                            //if game object has the drag event part
                            if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.Drag))
                            {
                                //update the component drag status
                                logicComponent.EventParts.Get(GuiComponentSupportEvent.Drag).
                                    Invoke(gameObject as GuiControl, new GuiComponentDragEvent(eventArg.Time, false));
                            }
                        }
                    }

                    //mouse down and it is in the control, record the capture control
                    if (eventArg.IsDown && isContained)
                    {
                        newCaptureControl = gameObject as GuiControl;
                    }

                    //update the ancestors stack
                    ancestors.Push(new Tuple<GameObject, Matrix4x4>(gameObject, invertTransform));
                }

                //update the new focus control status and invoke event
                //disable the old focus control status and invoke event
                if (newFocusControl != null)
                {
                    var oldFocusLogicComponent = eventProperty.FocusControl?.GetComponent<LogicGuiComponent>();

                    //disable old control
                    if (oldFocusLogicComponent != null && oldFocusLogicComponent.EventParts.Contain(GuiComponentSupportEvent.Focus))
                    {
                        //get focus event part and invoke the event
                        oldFocusLogicComponent.EventParts.Get(GuiComponentSupportEvent.Focus).
                            Invoke(eventProperty.FocusControl, new GuiComponentFocusEvent(eventArg.Time, false));
                    }

                    //update new control, if the new contorl does not have focus event part, we will miss focus
                    //else we invoke the event for focus event part
                    var logicComponent = newFocusControl.GetComponent<LogicGuiComponent>();

                    if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.Focus))
                    {
                        logicComponent.EventParts.Get(GuiComponentSupportEvent.Focus)
                            .Invoke(newFocusControl, new GuiComponentFocusEvent(eventArg.Time, true));

                        eventProperty.FocusControl = newFocusControl;
                    }
                    else eventProperty.FocusControl = null;
                }

                //update the new drag control status and invoke event
                if (newDragControl != null)
                {
                    var logicComponent = newDragControl.GetComponent<LogicGuiComponent>();

                    if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.Drag))
                    {
                        logicComponent.EventParts.Get(GuiComponentSupportEvent.Drag).
                            Invoke(newDragControl, new GuiComponentDragEvent(eventArg.Time, true));

                        eventProperty.DragControl = newDragControl;
                    }
                    else eventProperty.DragControl = null;
                }

                //update the new capture control status and invoke event
                if (newCaptureControl != null)
                {
                    var button = (uint)eventArg.Button;
                    var logicComponent = newCaptureControl.GetComponent<LogicGuiComponent>();

                    //if the old capture control is not null, means we do not release the button before we press
                    Utility.Assert(eventProperty.CaptureControl[button] == null);

                    eventProperty.CaptureControl[button] = newCaptureControl;

                    //invoke the event 
                    if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.MouseClick))
                        logicComponent.EventParts.Get(GuiComponentSupportEvent.MouseClick).
                            Invoke(newCaptureControl, new GuiComponentMouseClickEvent(
                                eventArg.Time, eventArg.Position, eventArg.Button, eventArg.IsDown));
                }
            }

            //solve mouse wheel event, we need process all objects
            //it may trigger the wheel event
            void mouseWheelSolver(List<GameObject> gameObjects, MouseWheelEvent eventArg, ref GuiComponentEventProperty eventProperty)
            {
                //stack to maintain the path of game object's tree from node to root
                //matrix is the invert transform from root to node
                Stack<Tuple<GameObject, Matrix4x4>> ancestors = new Stack<Tuple<GameObject, Matrix4x4>>();

                //add virtual root to stack
                ancestors.Push(new Tuple<GameObject, Matrix4x4>(null, Matrix4x4.Identity));

                foreach (var gameObject in gameObjects)
                {
                    //maintain the elments in the stack are ancestors from root to node(with deep order, first element is root)
                    //because the list of game objects is the dfs order of tree
                    while (ancestors.Count != 1 && ancestors.Peek().Item1 != gameObject.Parent) ancestors.Pop();

                    //get the component of gui control
                    var logicComponent = gameObject.GetComponent<LogicGuiComponent>();
                    var visualComponent = gameObject.GetComponent<VisualGuiComponent>();
                    var transformComponent = gameObject.GetComponent<TransformGuiComponent>();

                    var invertTransform = Matrix4x4.Identity;

                    //invert the transform of game object
                    Matrix4x4.Invert(transformComponent.Transform, out invertTransform);

                    //get the invert transform from root to node
                    invertTransform = ancestors.Peek().Item2 * invertTransform;

                    //compute the mouse position in the local space of gui control
                    var mousePosition = Vector2.Transform(new Vector2(eventArg.Position.X, eventArg.Position.Y), invertTransform);

                    //if the game object has mouse wheel 
                    //invoke the wheel event for game object mouse position is contained
                    if (visualComponent.Shape.Contain(new Position<float>(mousePosition.X, mousePosition.Y)) &&
                        logicComponent.EventParts.Contain(GuiComponentSupportEvent.MouseWheel))
                        logicComponent.EventParts.Get(GuiComponentSupportEvent.MouseWheel).Invoke(gameObject as GuiControl,
                            new GuiComponentMouseWheelEvent(eventArg.Time, eventArg.Position, eventArg.Offset));

                    //update the ancestors stack
                    ancestors.Push(new Tuple<GameObject, Matrix4x4>(gameObject, invertTransform));
                }
            }

            //solve mouse move event, we need process all objects
            //it may trigger the drag, move, hover
            void mouseMoveSolver(List<GameObject> gameObjects, MouseMoveEvent eventArg, ref GuiComponentEventProperty eventProperty)
            {
                //when we move the mouse, the control are dragging should be moving
                if (eventProperty.DragControl != null)
                {
                    var logicComponent = eventProperty.DragControl.GetComponent<LogicGuiComponent>();

                    //if the control has drag event part, we move it
                    if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.Drag))
                    {
                        var transformComponent = eventProperty.DragControl.GetComponent<TransformGuiComponent>();

                        transformComponent.Position = new Position<float>(
                            transformComponent.Position.X + eventArg.Position.X - eventProperty.MousePosition.X,
                            transformComponent.Position.Y + eventArg.Position.Y - eventProperty.MousePosition.Y);
                    }
                    else eventProperty.DragControl = null;
                }

                //stack to maintain the path of game object's tree from node to root
                //matrix is the invert transform from root to node
                //bool is the visable status sum(operator &) from root to node
                Stack<Tuple<GameObject, Matrix4x4>> ancestors = new Stack<Tuple<GameObject, Matrix4x4>>();

                //add virtual root to stack
                ancestors.Push(new Tuple<GameObject, Matrix4x4>(null, Matrix4x4.Identity));

                foreach (var gameObject in gameObjects)
                {
                    //maintain the elments in the stack are ancestors from root to node(with deep order, first element is root)
                    //because the list of game objects is the dfs order of tree
                    while (ancestors.Count != 1 && ancestors.Peek().Item1 != gameObject.Parent) ancestors.Pop();

                    //get the component of gui control
                    var logicComponent = gameObject.GetComponent<LogicGuiComponent>();
                    var visualComponent = gameObject.GetComponent<VisualGuiComponent>();
                    var transformComponent = gameObject.GetComponent<TransformGuiComponent>();

                    var invertTransform = Matrix4x4.Identity;

                    //invert the transform of game object
                    Matrix4x4.Invert(transformComponent.Transform, out invertTransform);

                    //get the invert transform from root to node
                    invertTransform = ancestors.Peek().Item2 * invertTransform;

                    //compute the mouse position in the local space of gui control
                    var mousePosition = Vector2.Transform(new Vector2(eventArg.Position.X, eventArg.Position.Y), invertTransform);
                    //test if the mouse position is in the gui control
                    var isContained = visualComponent.Shape.Contain(new Position<float>(mousePosition.X, mousePosition.Y));

                    //if we has mouse move event part
                    //invoke the move event for game object when mouse position is contained
                    if (isContained == true && logicComponent.EventParts.Contain(GuiComponentSupportEvent.MouseMove))
                        logicComponent.EventParts.Get(GuiComponentSupportEvent.MouseMove).Invoke(gameObject as GuiControl,
                            new GuiComponentMouseMoveEvent(eventArg.Time, eventArg.Position));

                    //if we have mouse hover event part
                    //solve the hover event, we only sender hover event when the hover is changed
                    if (logicComponent.EventParts.Contain(GuiComponentSupportEvent.Hover))
                    {
                        //get mouse hover event part
                        var hoverPart = logicComponent.EventParts.Get(GuiComponentSupportEvent.Hover) as GuiComponentHoverEventPart;

                        //hover is true and isContained is false(leave = false)
                        //hover is false and isContained is true(enter = true)
                        if (hoverPart.Hover != isContained)
                        {
                            //invoke hover event, the hover status is equal the "isContained"
                            hoverPart.Invoke(gameObject as GuiControl, new GuiComponentHoverEvent(eventArg.Time, isContained));
                        }
                    }
                    
                    //update the ancestors stack
                    ancestors.Push(new Tuple<GameObject, Matrix4x4>(gameObject, invertTransform));
                }

                //update event property
                eventProperty.MousePosition = eventArg.Position;
            }

            //logic component solver
            while (EventCount != 0)
            {
                switch (GetEvent(true))
                {
                    case KeyBoardEvent keyBoard: keyBoardSolver(passedGameObjectList, keyBoard, ref mComponentEventProperty); break;
                    case MouseClickEvent mouseClick: mouseClickSolver(passedGameObjectList, mouseClick, ref mComponentEventProperty); break;
                    case MouseWheelEvent mouseWheel: mouseWheelSolver(passedGameObjectList, mouseWheel, ref mComponentEventProperty); break;
                    case MouseMoveEvent mouseMove: mouseMoveSolver(passedGameObjectList, mouseMove, ref mComponentEventProperty); break;
                    default: break;
                }
            }
        }
    }
}
