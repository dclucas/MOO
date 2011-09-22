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
    /// Represents a property mapping method.
    /// </summary>
    /// <typeparam name="TSource">
    /// Source type.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// Destination type.
    /// </typeparam>
    /// <param name="source">
    /// Source object.
    /// </param>
    /// <param name="target">
    /// Destination object.
    /// </param>
    public delegate void MappingAction<TSource, TTarget>(TSource source, TTarget target);

    /// <summary>
    /// Interface for extensible mappers.
    /// </summary>
    /// <typeparam name="TSource">
    /// Origin type for the mapping.
    /// </typeparam>
    /// <typeparam name="TTarget">
    /// Destination type for tha mapping.
    /// </typeparam>
    public interface IExtensibleMapper<TSource, TTarget> : IMapper<TSource, TTarget>
    {
        /// <summary>
        /// Adds a mapping rule for the specified members.
        /// </summary>
        /// <param name="sourceMemberName">
        /// Source member.
        /// </param>
        /// <param name="targetMemberName">
        /// Destination member.
        /// </param>
        /// <param name="mappingAction">
        /// The delegate that will perform the conversion.
        /// </param>
        void AddMappingAction(string sourceMemberName, string targetMemberName, MappingAction<TSource, TTarget> mappingAction);
    }
}