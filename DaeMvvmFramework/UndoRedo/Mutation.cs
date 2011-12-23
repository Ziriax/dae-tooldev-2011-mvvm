namespace DaeMvvmFramework
{
    /// <summary>
    /// A Mutation encapsulates Undo and Redo methods.
    /// It also provides a Do method which is called
    /// the first time the Mutation is applied. 
    /// Typically Do and Redo share the same code.
    /// </summary>
    /// <remarks>
    /// Only the History can call Do, Redo and Undo;
    /// hence these methods are marked internal protected.
    /// </remarks>
    public abstract class Mutation
    {
        internal protected virtual void Do()
        {
            Redo();
        }

        internal protected abstract void Redo();
        internal protected abstract void Undo();
    }
}