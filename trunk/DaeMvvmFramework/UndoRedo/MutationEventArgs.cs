using System;

namespace DaeMvvmFramework
{
    public sealed class MutationEventArgs : EventArgs
    {
        public readonly Mutation Mutation;
        public readonly MutationEventKind Kind;

        public MutationEventArgs(Mutation mutation, MutationEventKind kind)
        {
            Mutation = mutation;
            Kind = kind;
        }
    }
}