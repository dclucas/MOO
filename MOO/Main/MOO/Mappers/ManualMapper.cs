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
    using Moo.Core;

    /// <summary>
    /// Manual mappers allow the addition of custom mapping rules, through code.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <typeparam name="TTarget">The type of the target.</typeparam>
    public class ManualMapper<TSource, TTarget> : BaseMapper<TSource, TTarget>, IExtensibleMapper<TSource, TTarget>
    {
        #region Methods (2)

        // Public Methods (1) 

        /// <summary>
        /// Adds the mapping action.
        /// </summary>
        /// <param name="sourceMemberName">Name of the source member.</param>
        /// <param name="targetMemberName">The target member member.</param>
        /// <param name="mappingAction">The mapping action.</param>
        public void AddMappingAction(
            string sourceMemberName,
            string targetMemberName,
            MappingAction<TSource, TTarget> mappingAction)
        {
            var info = new DelegateMappingInfo<TSource, TTarget>(sourceMemberName, targetMemberName, mappingAction);
            this.AddMappingInfo(info);
        }

        // Protected Methods (1) 

        /// <summary>
        /// Generates the member mappings and adds them targetType the provided <see cref="TypeMappingInfo{TSource, TTarget}"/> object.
        /// </summary>
        /// <param name="typeMapping">The type mapping where discovered mappings will be added.</param>
        /// <remarks>
        /// As this mapper should generate absolutely no mappings (all of its mappings are to
        /// be added manually), this method is intentionally left blank.
        /// </remarks>
        protected override void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping)
        {
        }

        #endregion Methods
    }
}