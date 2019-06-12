﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiGroup : GuiElement
    {
        public List<GuiElement> Elements { get; }

        public InputMapped InputMapped { get; }

        protected internal override void Draw(GuiRender render) 
            => Elements.ForEach((element) =>
            {
                render.SetTransform(element.Transform.GetMatrix());

                element.Draw(render);
            });

        protected internal override void Update(float delta)
            => Elements.ForEach((element) => element.Update(delta));

        protected internal override void Input(InputAction action)
        {
            var alias = InputMapped.IsMapped(action.Name) ?
                InputMapped.MappedInput(action.Name) : Gui.InputMapped.MappedInput(action.Name);

            if (!GuiProperty.IsGuiInput(alias)) return;

            void resolveInputMoveX(AxisInputAction input)
            {
                float offset = input.Offset * Gui.Canvas.Size.Width;

                Gui.GlobalElementStatus.Position.X += offset;

                if (Gui.GlobalElementStatus.DragElement != null)
                {
                    Gui.GlobalElementStatus.DragElement.Transform.Position =
                        new Point2f(
                            Gui.GlobalElementStatus.DragElement.Transform.Position.X + offset,
                            Gui.GlobalElementStatus.DragElement.Transform.Position.Y);
                }
            }

            void resolveInputMoveY(AxisInputAction input)
            {
                float offset = input.Offset * Gui.Canvas.Size.Height;

                Gui.GlobalElementStatus.Position.Y += offset;

                if (Gui.GlobalElementStatus.DragElement != null)
                {
                    Gui.GlobalElementStatus.DragElement.Transform.Position =
                        new Point2f(
                            Gui.GlobalElementStatus.DragElement.Transform.Position.X,
                            Gui.GlobalElementStatus.DragElement.Transform.Position.Y + offset);
                }
            }

            void resolveInputWheel(AxisInputAction input)
            {
                if (Gui.GlobalElementStatus.FocusElement != null)
                {
                    Gui.GlobalElementStatus.FocusElement.Input(
                        new AxisInputAction(GuiProperty.InputWheel, input.Offset));
                }
            }

            void resolveInputClick(ButtonInputAction input)
            {
                bool anyElementContain = false;

                GuiElement newElement = null;

                Elements.ForEach((element) =>
                {
                    //input click down, we need to get drag element, focus element ...
                    if (input.Status == true)
                    {
                        bool isContain = element.Contain(Gui.GlobalElementStatus.Position);

                        anyElementContain = anyElementContain | isContain;

                        //if we are draging gui element, we do not need to update the drag or focus element.
                        if (isContain == true && Gui.GlobalElementStatus.DragElement == null)
                        {
                            newElement = element;
                        }
                    }
                });

                if (newElement != null)
                {
                    Gui.GlobalElementStatus.DragElement = newElement;
                    Gui.GlobalElementStatus.FocusElement = newElement;

                    newElement.Input(new ButtonInputAction(GuiProperty.InputClick, input.Status));
                }

                ///update the drag and focus element.
                ///if the input click is down and there are no elements contain the cursor.
                ///we need to release the focus gui element.
                ///if the input click is up, we need to release the drag gui element.
                if (input.Status == true)
                {
                    if (anyElementContain == false)
                        Gui.GlobalElementStatus.FocusElement = null;
                }
                else Gui.GlobalElementStatus.DragElement = null;
            }

            void resolveInputText(ButtonInputAction input)
            {
                if (Gui.GlobalElementStatus.FocusElement != null)
                {
                    Gui.GlobalElementStatus.FocusElement.Input(input);
                }
            }

            InputSolver solver = new InputSolver();

            solver.AxisInputAction.Add(GuiProperty.InputMoveX, new AxisInputActionSolvers());
            solver.AxisInputAction.Add(GuiProperty.InputMoveY, new AxisInputActionSolvers());
            solver.AxisInputAction.Add(GuiProperty.InputWheel, new AxisInputActionSolvers());

            solver.AxisInputAction[GuiProperty.InputMoveX].Solvers.Add(resolveInputMoveX);
            solver.AxisInputAction[GuiProperty.InputMoveY].Solvers.Add(resolveInputMoveY);
            solver.AxisInputAction[GuiProperty.InputWheel].Solvers.Add(resolveInputWheel);

            solver.ButtonInputAction.Add(GuiProperty.InputClick, new ButtonInputActionSolvers());
            solver.ButtonInputAction.Add(GuiProperty.InputText, new ButtonInputActionSolvers());

            solver.ButtonInputAction[GuiProperty.InputClick].Solvers.Add(resolveInputClick);
            solver.ButtonInputAction[GuiProperty.InputText].Solvers.Add(resolveInputText);

            solver.InputMappeds.Add(InputMapped);
            solver.InputMappeds.Add(Gui.InputMapped);

            solver.Execute(action);
        }

        public override bool Contain(Point2f point)
        {
            foreach (var element in Elements)
                if (element.Contain(point)) return true;
            return false;
        }

        public string Name { get; }

        public GuiGroup(string name)
        {
            Name = name;
            Elements = new List<GuiElement>();
            InputMapped = new InputMapped();
        }
    }
}
