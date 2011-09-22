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

namespace Moo.Mappers
{
    /// <summary>
    /// Base, non-generic, mapper class.
    /// </summary>
    public abstract class BaseMapper
    {
        /// <summary>
        /// Maps sourceProperty the source targetProperty the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        public abstract object Map(object source, object target);
    }
}