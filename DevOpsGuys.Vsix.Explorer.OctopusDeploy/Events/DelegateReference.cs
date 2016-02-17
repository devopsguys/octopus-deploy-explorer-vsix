// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DelegateReference.cs" company="DevOpsGuys Ltd">
//   Copyright (c) DevOpsGuys Ltd. All rights reserved.
// </copyright>
// <summary>
//   Represents a reference to a <see cref="Delegate" /> that may contain a
//   <see cref="WeakReference" /> to the target. This class is used
//   internally by the Composite Application Library.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DevOpsGuys.Vsix.Explorer.OctopusDeploy.Events
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Represents a reference to a <see cref="Delegate"/> that may contain a
    /// <see cref="WeakReference"/> to the target. This class is used
    /// internally by the Composite Application Library.
    /// </summary>
    internal class DelegateReference : IDelegateReference
    {
        /// <summary>
        /// The delegate method.
        /// </summary>
        private readonly Delegate delegateMethod;

        /// <summary>
        /// The weak reference.
        /// </summary>
        private readonly WeakReference weakReference;

        /// <summary>
        /// The method.
        /// </summary>
        private readonly MethodInfo method;

        /// <summary>
        /// The delegate type.
        /// </summary>
        private readonly Type delegateType;

        /// <summary>
        /// Initialises a new instance of the <see cref="DelegateReference"/> class.
        /// </summary>
        /// <param name="delegate">The original <see cref="Delegate" /> to create a reference for.</param>
        /// <param name="keepReferenceAlive">If <see langword="false" /> the class will create a weak reference to the delegate, allowing it to be garbage collected. Otherwise it will keep a strong reference to the target.</param>
        /// <exception cref="ArgumentNullException">If the passed <paramref name="delegate" /> is not assignable to <see cref="Delegate" />.</exception>
        public DelegateReference(Delegate @delegate, bool keepReferenceAlive)
        {
            if (@delegate == null)
            {
                throw new ArgumentNullException("delegate");
            }

            if (keepReferenceAlive)
            {
                this.delegateMethod = @delegate;
            }
            else
            {
                this.weakReference = new WeakReference(@delegate.Target);
                this.method = @delegate.Method;
                this.delegateType = @delegate.GetType();
            }
        }

        /// <summary>
        /// Gets the <see cref="Delegate" /> (the target) referenced by the current <see cref="DelegateReference"/> object.
        /// </summary>
        /// <value><see langword="null"/> if the object referenced by the current <see cref="DelegateReference"/> object has been garbage collected; otherwise, a reference to the <see cref="Delegate"/> referenced by the current <see cref="DelegateReference"/> object.</value>
        public Delegate Target
        {
            get
            {
                if (this.delegateMethod != null)
                {
                    return this.delegateMethod;
                }
                else
                {
                    return this.TryGetDelegate();
                }
            }
        }

        /// <summary>
        /// Tries the get delegate.
        /// </summary>
        /// <returns>A <see cref="Delegate"/>.</returns>
        private Delegate TryGetDelegate()
        {
            if (this.method.IsStatic)
            {
                return Delegate.CreateDelegate(this.delegateType, null, this.method);
            }

            object target = this.weakReference.Target;
            if (target != null)
            {
                return Delegate.CreateDelegate(this.delegateType, target, this.method);
            }
            
            return null;
        }
    }
}