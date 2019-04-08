using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

using GalEngine.GameResource;
using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    
    public class GuiDebugProperty
    {
        public ShapeDebugProperty ShapeProperty { get; set; }
    }

    public class GuiSystem : BehaviorSystem
    {
        private Image mCanvas;
        private GuiRender mRender;

        private GuiComponentEventProperty mComponentEventProperty;

        public Rectangle<int> Area { get; set; }
        public GuiDebugProperty GuiDebugProperty { get; set; }
        
        public GuiSystem(GpuDevice device, Rectangle<int> area) : base("GuiSystem")
        {
            RequireComponents.AddRequireComponentType<TransformGuiComponent>();
            RequireComponents.AddRequireComponentType<VisualGuiComponent>();
            RequireComponents.AddRequireComponentType<LogicGuiComponent>();
            
            Area = area;

            mRender = new GuiRender(device);

            mCanvas = new Image(
                new Size<int>(Area.Right - Area.Left, Area.Bottom - Area.Top),
                PixelFormat.RedBlueGreenAlpha8bit,
                mRender.Device);
        }

        protected internal override void Update()
        {
            //update the render area, we need to update the canvas and render target
            //if the area's size is not equal the canvas's size
            if (mCanvas.Size.Width != Area.Right - Area.Left ||
                mCanvas.Size.Height != Area.Bottom - Area.Top)
            {
                Utility.Dispose(ref mCanvas);

                mCanvas = new Image(
                    new Size<int>(Area.Right - Area.Left, Area.Bottom - Area.Top),
                    PixelFormat.RedBlueGreenAlpha8bit,
                    mRender.Device);
            }
        }

        protected internal override void Present(PresentRender render)
        {
            render.Mask(mCanvas, Area, 1.0f);
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

                //because there is most one control get focus, so we only run one event solver
                eventProperty.FocusControl.GetComponent<LogicGuiComponent>().GetEventSolver(GuiComponentStatusProperty.Input)?
                    .Invoke(eventProperty.FocusControl, new GuiComponentInputEvent(eventArg.Time, eventArg.KeyCode));
            }

            //solve mouse click event, we need process all objects
            //it may trigger the drag, focus, click event for gui control
            void mouseClickSolver(List<GameObject> gameObjects, MouseClickEvent eventArg, ref GuiComponentEventProperty eventProperty)
            {
                //stack to maintain the path of game object's tree from node to root
                //matrix is the invert transform from root to node
                //bool is the visable status sum(operator &) from root to node
                Stack<Tuple<GameObject, Matrix4x4, bool>> ancestors = new Stack<Tuple<GameObject, Matrix4x4, bool>>();

                //add virtual root to stack
                ancestors.Push(new Tuple<GameObject, Matrix4x4, bool>(null, Matrix4x4.Identity, true));

                GuiControl newDragControl = null;
                GuiControl newFocusControl = null;

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
                    //get the show status(sum of & from root to node)
                    var isShow = logicComponent.GetStatus(GuiComponentStatusProperty.Show) & ancestors.Peek().Item3;

                    //for solving drag and focus event
                    if (eventArg.Button == MouseButton.Left)
                    {
                        //solve the drag event, we need enable drag event and the mouse position in the control
                        if (logicComponent.GetEventStatus(GuiComponentStatusProperty.Drag) == true && isContained == true && isShow == true)
                        {
                            //mouse down, means we start to drag control
                            //if we want to drag new control, we must release last control
                            if (eventArg.IsDown == true && eventProperty.DragControl == null)
                            {
                                //disable old control who are draging, because only one control should be draging at same time
                                //and set the new control who are draging
                                newDragControl = gameObject as GuiControl;
                            } 

                            //mouse up, means we end to drag control
                            if (eventArg.IsDown == false && eventProperty.DragControl == gameObject)
                            {
                                //end to drag
                                eventProperty.DragControl = null;

                                //update the component drag status and invoke the event
                                logicComponent.SetStatus(GuiComponentStatusProperty.Drag, false);
                                logicComponent.GetEventSolver(GuiComponentStatusProperty.Drag)?.
                                    Invoke(control: gameObject as GuiControl, new GuiComponentDragEvent(eventArg.Time, false));
                            }
                        }

                        //solve the focus event, we need enable focus event and the mouse position in the control
                        if (logicComponent.GetEventStatus(GuiComponentStatusProperty.Focus) == true && isContained == true && isShow == true)
                        {
                            //only mouse button is down we need to update the game obejct who get focus
                            if (eventArg.IsDown == true)
                            {
                                //only one control can get focus
                                newFocusControl = gameObject as GuiControl;
                            }
                        }

                        //when we click the empty place, the focus will be lost
                        if (gameObject == eventProperty.FocusControl && isContained == false)
                        {
                            eventProperty.FocusControl = null;

                            //update the component focus status and invoke the event of get focus
                            logicComponent.SetStatus(GuiComponentStatusProperty.Focus, false);
                            logicComponent.GetEventSolver(GuiComponentStatusProperty.Focus)?
                                .Invoke(gameObject as GuiControl, new GuiComponentFocusEvent(eventArg.Time, false));
                        }
                    }

                    //invoke the click event for game object when event is enabled and mouse position is contained
                    if (logicComponent.GetEventStatus(GuiComponentStatusProperty.Click) == true && isContained == true && isShow == true)
                        logicComponent.GetEventSolver(GuiComponentStatusProperty.Click)?.Invoke(gameObject as GuiControl, 
                            new GuiComponentClickEvent(eventArg.Time, eventArg.Position, eventArg.Button, eventArg.IsDown));

                    //update the ancestors stack
                    ancestors.Push(new Tuple<GameObject, Matrix4x4, bool>(gameObject, invertTransform, isShow));
                }

                //update the new drag control status and invoke event
                if (newDragControl != null)
                {
                    newDragControl.GetComponent<LogicGuiComponent>().SetStatus(GuiComponentStatusProperty.Drag, true);
                    newDragControl.GetComponent<LogicGuiComponent>().GetEventSolver(GuiComponentStatusProperty.Drag)?.
                        Invoke(newDragControl, new GuiComponentDragEvent(eventArg.Time, true));

                    eventProperty.DragControl = newDragControl;
                }

                //update the new focus control status and invoke event
                //disable the old focus control status and invoke event
                if (newFocusControl != null)
                {
                    //disable old control
                    if (eventProperty.FocusControl != null)
                    {
                        eventProperty.FocusControl.GetComponent<LogicGuiComponent>().SetStatus(GuiComponentStatusProperty.Focus, false);
                        eventProperty.FocusControl.GetComponent<LogicGuiComponent>().GetEventSolver(GuiComponentStatusProperty.Focus)?.
                            Invoke(eventProperty.FocusControl, new GuiComponentFocusEvent(eventArg.Time, false));
                    }

                    //update new control
                    newFocusControl.GetComponent<LogicGuiComponent>().SetStatus(GuiComponentStatusProperty.Focus, true);
                    newFocusControl.GetComponent<LogicGuiComponent>().GetEventSolver(GuiComponentStatusProperty.Focus)?.
                        Invoke(newFocusControl, new GuiComponentFocusEvent(eventArg.Time, true));

                    eventProperty.FocusControl = newFocusControl;
                }
            }

            //solve mouse wheel event, we need process all objects
            //it may trigger the wheel event
            void mouseWheelSolver(List<GameObject> gameObjects, MouseWheelEvent eventArg, ref GuiComponentEventProperty eventProperty)
            {
                //stack to maintain the path of game object's tree from node to root
                //matrix is the invert transform from root to node
                //bool is the visable status sum(operator &) from root to node
                Stack<Tuple<GameObject, Matrix4x4, bool>> ancestors = new Stack<Tuple<GameObject, Matrix4x4, bool>>();

                //add virtual root to stack
                ancestors.Push(new Tuple<GameObject, Matrix4x4, bool>(null, Matrix4x4.Identity, true));

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
                    //get the show status(sum of & from root to node)
                    var isShow = logicComponent.GetStatus(GuiComponentStatusProperty.Show) & ancestors.Peek().Item3;

                    //invoke the wheel event for game object when event is enabled and mouse position is contained
                    if (logicComponent.GetEventStatus(GuiComponentStatusProperty.Wheel) == true && isContained == true && isShow == true)
                        logicComponent.GetEventSolver(GuiComponentStatusProperty.Wheel)?.Invoke(gameObject as GuiControl,
                            new GuiComponentWheelEvent(eventArg.Time, eventArg.Position, eventArg.Offset));

                    //update the ancestors stack
                    ancestors.Push(new Tuple<GameObject, Matrix4x4, bool>(gameObject, invertTransform, isShow));
                }
            }

            //solve mouse move event, we need process all objects
            //it may trigger the drag, move, hover
            void mouseMoveSolver(List<GameObject> gameObjects, MouseMoveEvent eventArg, ref GuiComponentEventProperty eventProperty)
            {
                //solve the drag event, we only need to process the drag control
                if (eventProperty.DragControl != null && eventProperty.MousePosition != null)
                {
                    //event property only record the status of event
                    //if the drag status changed by other reason, we need to update the status
                    if (eventProperty.DragControl.GetComponent<LogicGuiComponent>().GetStatus(GuiComponentStatusProperty.Drag) == true)
                    {
                        //get the offset we need to move the drag control
                        var dragTransformComponent = eventProperty.DragControl.GetComponent<TransformGuiComponent>();
                        var offset = new Position<float>(
                            eventArg.Position.X - eventProperty.MousePosition.X,
                            eventArg.Position.Y - eventProperty.MousePosition.Y);

                        //move it
                        dragTransformComponent.Position = new Position<float>(
                            dragTransformComponent.Position.X + offset.X,
                            dragTransformComponent.Position.Y + offset.Y);
                    }
                    else eventProperty.DragControl = null;
                }

                //stack to maintain the path of game object's tree from node to root
                //matrix is the invert transform from root to node
                //bool is the visable status sum(operator &) from root to node
                Stack<Tuple<GameObject, Matrix4x4, bool>> ancestors = new Stack<Tuple<GameObject, Matrix4x4, bool>>();

                //add virtual root to stack
                ancestors.Push(new Tuple<GameObject, Matrix4x4, bool>(null, Matrix4x4.Identity, true));

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
                    //get the show status(sum of & from root to node)
                    var isShow = logicComponent.GetStatus(GuiComponentStatusProperty.Show) & ancestors.Peek().Item3;

                    //invoke the move event for game object when event is enabled and mouse position is contained
                    if (logicComponent.GetEventStatus(GuiComponentStatusProperty.Move) == true && isContained == true && isShow == true)
                        logicComponent.GetEventSolver(GuiComponentStatusProperty.Move)?.Invoke(gameObject as GuiControl,
                            new GuiComponentMoveEvent(eventArg.Time, eventArg.Position));

                    //solve the hover event, we only sender event when the hover is changed
                    if (logicComponent.GetEventStatus(GuiComponentStatusProperty.Hover) == true && isShow == true)
                    {
                        var hover = logicComponent.GetStatus(GuiComponentStatusProperty.Hover);

                        //hover is not equal the is Contained, we need to sent hover event
                        if (hover ^ isContained == true)
                        {
                            //when is contained is true, the hover must be false, we need to invoke enter event(hover = true)
                            //when is contained is false, the hover must be true, we need to invoke leave event(hover = false)
                            logicComponent.GetEventSolver(GuiComponentStatusProperty.Hover)?.Invoke(gameObject as GuiControl,
                                new GuiComponentHoverEvent(eventArg.Time, isContained));
                        }

                        logicComponent.SetStatus(GuiComponentStatusProperty.Hover, isContained);
                    }

                    //update the ancestors stack
                    ancestors.Push(new Tuple<GameObject, Matrix4x4, bool>(gameObject, invertTransform, isShow));
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

            //solve to render text component
            void textGuiComponentSolver(GuiRender render, bool show, TextGuiComponent textComponent)
            {
                //update the property and create asset(Text)
                textComponent.SetPropertyToAsset();

                var size = (textComponent.Shape as RectangleShape).Size;

                //only render when control is show
                if (show == true)
                {    
                    //compute the position(center)
                    var position = new Position<float>(
                        x: (size.Width - textComponent.mTextAsset.Size.Width) * 0.5f,
                        y: (size.Height - textComponent.mTextAsset.Size.Height) * 0.5f);

                    //not invisable text, we will render it
                    if (textComponent.mTextAsset.Texture.GpuTexture != null)
                        render.DrawText(position, textComponent.mTextAsset, textComponent.Color);
                }

                //enable debug mode, we will render the shape
                if (GuiDebugProperty != null && GuiDebugProperty.ShapeProperty != null)
                {
                    var padding = GuiDebugProperty.ShapeProperty.Padding;

                    //draw debug shape
                    render.DrawRectangle(
                        rectangle: new Rectangle<float>(
                            left: -padding, 
                            top: -padding, 
                            right: size.Width + padding, 
                            bottom: size.Height + padding),
                        color: GuiDebugProperty.ShapeProperty.Color,
                        padding: GuiDebugProperty.ShapeProperty.Padding);
                }
            }

            //solve to render rectangle compoent
            void rectangleComponentSolver(GuiRender render, bool show, RectangleGuiComponent rectangleComponent)
            {
                //get size of rectangle
                var size = (rectangleComponent.Shape as RectangleShape).Size;

                //only render when control is show
                if (show == true)
                {
                    //swtich render mode
                    switch (rectangleComponent.RenderMode)
                    {
                        case GuiRenderMode.WireFrame:
                            render.DrawRectangle(
                                rectangle: new Rectangle<float>(0, 0, size.Width, size.Height),
                                color: rectangleComponent.Color,
                                padding: rectangleComponent.Padding);
                            break;
                        case GuiRenderMode.Solid:
                            render.FillRectangle(
                                rectangle: new Rectangle<float>(0, 0, size.Width, size.Height),
                                color: rectangleComponent.Color);
                            break;
                        default: break;
                    }
                }

                //draw debug shape
                if (GuiDebugProperty != null && GuiDebugProperty.ShapeProperty != null)
                {
                    var padding = GuiDebugProperty.ShapeProperty.Padding;

                    //draw debug shape
                    render.DrawRectangle(
                        rectangle: new Rectangle<float>(
                            left: -padding,
                            top: -padding,
                            right: size.Width + padding,
                            bottom: size.Height + padding),
                        color: GuiDebugProperty.ShapeProperty.Color,
                        padding: GuiDebugProperty.ShapeProperty.Padding);
                }
            }

            //solve to render image component
            void imageComponentSolver(GuiRender render, bool show, ImageGuiComponent imageComponent)
            {
                var size = (imageComponent.Shape as RectangleShape).Size;

                //only render when control is show
                if (show == true)
                {
                    //draw image
                    render.DrawImage(new Rectangle<float>(
                        left: 0, top: 0, right: size.Width, bottom: size.Height),
                        image: imageComponent.Image,
                        opacity: imageComponent.Opacity);
                }

                //draw debug shape
                if (GuiDebugProperty != null && GuiDebugProperty.ShapeProperty != null)
                {
                    var padding = GuiDebugProperty.ShapeProperty.Padding;

                    //draw debug shape
                    render.DrawRectangle(
                        rectangle: new Rectangle<float>(
                            left: -padding,
                            top: -padding,
                            right: size.Width + padding,
                            bottom: size.Height + padding),
                        color: GuiDebugProperty.ShapeProperty.Color,
                        padding: GuiDebugProperty.ShapeProperty.Padding);
                }
            }

            //stack to maintain the path of game object's tree from node to root
            //matrix is the transform from root to node
            //bool is the visable status sum(operator &) from root to node
            Stack<Tuple<GameObject, Matrix4x4, bool>> transformStack = new Stack<Tuple<GameObject, Matrix4x4, bool>>();

            //add virtual root to stack
            transformStack.Push(new Tuple<GameObject, Matrix4x4, bool>(null, Matrix4x4.Identity, true));

            //visual component solver
            mRender.BeginDraw(mCanvas);
            mRender.Clear(mCanvas, new Color<float>(1, 1, 1, 1));

            foreach (var gameObject in passedGameObjectList)
            {
                //maintain the elments in the stack are ancestors from root to node(with deep order, first element is root)
                //because the list of game objects is the dfs order of tree
                while (transformStack.Count != 1 && transformStack.Peek().Item1 != gameObject.Parent) transformStack.Pop();

                //get current transform matrix, it is the result of root to node's transform matrix
                var transformComponent = gameObject.GetComponent<TransformGuiComponent>();
                var transformMatrix = transformComponent.Transform * transformStack.Peek().Item2;
                var isShow = gameObject.GetComponent<LogicGuiComponent>().GetStatus(GuiComponentStatusProperty.Show) & transformStack.Peek().Item3;

                //set transform 
                mRender.SetTransform(transformMatrix);

                switch (gameObject.GetComponent<VisualGuiComponent>())
                {
                    case TextGuiComponent textComponent:
                        textGuiComponentSolver(mRender, isShow, textComponent); break;
                    case RectangleGuiComponent rectangleComponent:
                        rectangleComponentSolver(mRender, isShow, rectangleComponent); break;
                    case ImageGuiComponent imageComponent:
                        imageComponentSolver(mRender, isShow, imageComponent); break;
                    default: break;
                }

                //update stack
                transformStack.Push(new Tuple<GameObject, Matrix4x4, bool>(gameObject, transformMatrix, isShow));
            }

            mRender.EndDraw();
        }
    }
}
