/*-----------------------------------------------------------------------------
Copyright 2010 Diogo Lucas

This file is part of Moo.

Foobar is free software: you can redistribute it and/or modify it under the
terms of the GNU General Public License as published by the Free Software
Foundation, either version 3 of the License, or (at your option) any later
version.

Moo is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY
; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A
PARTICULAR PURPOSE. See the GNU General Public License for more details.

You should have received a copy of the GNU General Public License along with
Moo. If not, see http://www.gnu.org/licenses/.
---------------------------------------------------------------------------- */

namespace Moo
{
    /// <summary>
    /// Event args for cancellation-enabled events.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class MappingCancellationEventArgs<TSource, TTarget> : MappingEventArgs<TSource, TTarget>
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MappingCancellationEventArgs&lt;TSource, TTarget&gt;"/> should be canceled.
        /// </summary>
        /// <sourceValue>
        ///   <c>true</c> if should cancel; otherwise, <c>false</c>.
        /// </sourceValue>
        public bool Cancel { get; set; }
    }
}