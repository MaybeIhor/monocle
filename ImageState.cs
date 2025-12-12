using System.Collections.Generic;
using System.Drawing;

namespace Image_View
{
    public class ImageState
    {
        public Rectangle? Crop { get; set; }
        public int Rotation { get; set; }
        public bool IsMirrored { get; set; }

        public ImageState()
        {
            Crop = null;
            Rotation = 0;
            IsMirrored = false;
        }

        public ImageState(Rectangle? crop, int rotation, bool isMirrored)
        {
            Crop = crop;
            Rotation = rotation;
            IsMirrored = isMirrored;
        }

        public ImageState Clone()
        {
            return new ImageState(Crop, Rotation, IsMirrored);
        }

        public bool Equals(ImageState other)
        {
            if (other == null) return false;
            return Crop == other.Crop && Rotation == other.Rotation && IsMirrored == other.IsMirrored;
        }
    }
    public class StateStack
    {
        private Stack<ImageState> undoStack;
        private Stack<ImageState> redoStack;
        private ImageState currentState;

        public StateStack()
        {
            undoStack = new Stack<ImageState>();
            redoStack = new Stack<ImageState>();
            currentState = new ImageState();
        }

        public void PushState(ImageState state)
        {
            if (!state.Equals(currentState))
            {
                undoStack.Push(currentState.Clone());
                currentState = state.Clone();
                redoStack.Clear();
            }
        }

        public ImageState Undo()
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(currentState.Clone());
                currentState = undoStack.Pop();
            }
            return currentState.Clone();
        }

        public ImageState Redo()
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(currentState.Clone());
                currentState = redoStack.Pop();
            }
            return currentState.Clone();
        }

        public bool CanUndo() => undoStack.Count > 0;
        public bool CanRedo() => redoStack.Count > 0;

        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
            currentState = new ImageState();
        }

        public ImageState GetCurrentState() => currentState.Clone();
    }
}