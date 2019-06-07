using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    class EventForwardInputEmitter : InputEmitter
    {
        private EngineWindow mWindow;

        public EventForwardInputEmitter(EngineWindow window)
        {
            mWindow = window;
        }

        public void Forward(MouseMoveEvent eventArg)
        {
            var halfSize = new Sizef(
                width: mWindow.Size.Width * 0.5f,
                height: mWindow.Size.Height * 0.5f);

            float xOffset = (eventArg.Position.X - halfSize.Width) / halfSize.Width;
            float yOffset = (eventArg.Position.Y - halfSize.Height) / halfSize.Height;

            Forward(new AxisInputAction(InputProperty.MouseX, xOffset));
            Forward(new AxisInputAction(InputProperty.MouseY, yOffset));
        }

        public void Forward(MouseClickEvent eventArg)
        {
            var buttonName = "";

            switch (eventArg.Button)
            {
                case MouseButton.Left: buttonName = InputProperty.LeftButton; break;
                case MouseButton.Middle: buttonName = InputProperty.MiddleButton; break;
                case MouseButton.Right: buttonName = InputProperty.RightButton; break;
                default: throw new Exception("Invalid Mouse Button.");
            }

            Forward(new ButtonInputAction(buttonName, eventArg.IsDown));
        }

        public void Forward(MouseWheelEvent eventArg)
        {
            //forward mouse wheel event
            //notice: the range of offset is from -inf to inf
            Forward(new AxisInputAction(InputProperty.MouseWheel, eventArg.Offset / 120.0f));
        }

        public void Forward(KeyBoardEvent eventArg)
        {
            int keyCode = (int)eventArg.KeyCode;
            
            //forward keyboard event, for current version, we only forward A-Z keycode
            if (keyCode >= 'A' && keyCode <= 'Z')
            {
                Forward(new ButtonInputAction(((char)keyCode).ToString(), eventArg.IsDown));
            }
        }

        public void Forward(BaseEvent eventArg)
        {
            switch (eventArg)
            {
                case MouseClickEvent mouseClick: Forward(mouseClick); break;
                case MouseWheelEvent mouseWheel: Forward(mouseWheel); break;
                case MouseMoveEvent mouseMove: Forward(mouseMove); break;
                case KeyBoardEvent keyBoard: Forward(keyBoard); break;
                default: break;
            }
        }
    }
}
