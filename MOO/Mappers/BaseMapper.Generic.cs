// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Diogo Lucas">
//
// Copyright (C) 2010 Diogo Lucas
//
// This file is part of Moo.
//
// Moo is free software: you can redistribute it and/or modify
// it under the +terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along Moo.  If not, see http://www.gnu.org/licenses/.
// </copyright>
// <summary>
// Moo is a object-to-object multi-mapper.
// Email: diogo.lucas@gmail.com
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Moo.Mappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Moo.Core;
    using System.Collections;

    /// <summary>
    /// Base generic mapper class.
    /// </summary>
    /// <typeparam name="TSource">The type of the mapping source.</typeparam>
    /// <typeparam name="TTarget">The type of the mapping target.</typeparam>
    /// <remarks>
    /// This class exists targetProperty guarantee basic functioning and behavior on all mappers. All of them
    /// should inherit sourceProperty it.
    /// </remarks>
    public abstract class BaseMapper<TSource, TTarget> : BaseMapper, IMapper<TSource, TTarget>
    {
        /// <summary>Used for thread synchronization only.</summary>
        private object syncRoot = new object();

        /// <summary>The property explorer to use.</summary>
        private IPropertyExplorer propertyExplorer;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMapper&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        protected BaseMapper()
        {
            this.TypeMapping = new TypeMappingInfo<TSource, TTarget>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMapper{TSource,TTarget}"/> class.
        /// </summary>
        /// <param name="constructionInfo">
        /// Contains additional mapper construction information.
        /// </param>
        protected BaseMapper(MapperConstructionInfo constructionInfo)
            : this()
        {
            Guard.CheckArgumentNotNull(constructionInfo, "constructionInfo");
            this.ParentRepository = constructionInfo.ParentRepo;
        }

        #endregion Constructors

        #region Properties

        /// <summary>Gets or sets the type mapping information.</summary>
        /// <value>The type mapping.</value>
        public TypeMappingInfo<TSource, TTarget> TypeMapping { get; protected set; }

        /// <summary>
        /// Gets the parent mapping repository.
        /// </summary>
        protected IMappingRepository ParentRepository { get; private set; }

        /// <summary>Gets the current mapper status.</summary>
        /// <value>The current mapper status.</value>
        protected internal MapperStatus CurrentStatus { get; private set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Maps from the source to a new target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <returns>A filled target object.</returns>
        public object Map(object source)
        {
            return this.Map((TSource)source);
        }

        /// <summary>
        /// Maps the specified source to a target object.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>A filled target object</returns>
        /// <remarks>
        /// This method relies on the <see cref="System.Activator.CreateInstance&lt;T&gt;"/>
        /// method to create target objects. This means that both there are
        /// more efficient methods for that and that this limits the use of
        /// this overload to target classes that are passible of contruction
        /// through this framework method.
        /// </remarks>
        public virtual TTarget Map(TSource source)
        {
            var target = Activator.CreateInstance<TTarget>();
            this.Map(source, target);
            return target;
        }

        /// <summary>
        /// Maps from the source to the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>The target object, with its properties filled.</returns>
        public override object Map(object source, object target)
        {
            return this.Map((TSource)source, (TTarget)target);
        }

        /// <summary>
        /// Maps from the specified source to the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="createTarget">A function to create target objects.</param>
        /// <returns>
        /// A filled target object.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "1", Justification = "The call to Guard already does that.")]
        public object Map(object source, Func<object> createTarget)
        {
            Guard.CheckArgumentNotNull(source, "source");
            Guard.CheckArgumentNotNull(createTarget, "createTarget");
            var target = (TTarget)createTarget();
            return this.Map((TSource)source, target);
        }

        /// <summary>
        /// Maps from the source to the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        /// <returns>The target object, with its properties filled.</returns>
        public virtual TTarget Map(TSource source, TTarget target)
        {
            try
            {
                if (this.CurrentStatus == MapperStatus.New)
                {
                    this.InitializeMapping();
                }
            }
            catch (Exception exc)
            {
                throw new MappingException(
                    string.Format(
                            System.Globalization.CultureInfo.InvariantCulture,
                            "Error getting mappings from type {0} to {1}. Please check inner exception for details.",
                            typeof(TSource),
                            typeof(TTarget)),
                        typeof(TSource),
                        typeof(TTarget),
                        null,
                        null,
                        exc);
            }

            foreach (var mapping in this.TypeMapping.GetMappings())
            {
                try
                {
                    mapping.Map(source, target);
                }
                catch (Exception exc)
                {
                    throw new MappingException(typeof(TSource), typeof(TTarget), mapping.SourceMemberName, mapping.TargetMemberName, exc);
                }
            }

            return target;
        }

        /// <summary>Initializes the internal mapping.</summary>
        private void InitializeMapping()
        {
            if (this.TypeMapping == null)
            {
                lock (this.syncRoot)
                {
                    if (this.TypeMapping == null)
                    {
                        this.TypeMapping = new TypeMappingInfo<TSource, TTarget>();
                    }
                }
            }

            var mappings = this.GetMappings();
            if (mappings != null)
            {
                this.TypeMapping.AddRange(this.GetMappings());
            }

            this.TypeMapping.Compile();

            this.CurrentStatus = MapperStatus.Active;
        }

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="createTarget">A function to create target objects.</param>
        /// <returns>
        /// A filled target object.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "1",
            Justification = "The Guard call does that.")]
        public virtual TTarget Map(TSource source, Func<TTarget> createTarget)
        {
            Guard.CheckArgumentNotNull(createTarget, "createTarget");
            TTarget target = createTarget();
            this.Map(source, target);
            return target;
        }

        /// <summary>
        /// Maps multiple source objects into multiple target objects.
        /// </summary>
        /// <param name="sourceList">The source list.</param>
        /// <returns>
        /// A list of target objects.
        /// </returns>
        /// <remarks>
        /// This method relies on the <c>TTarget Map(TSource source)</c> item
        /// mapping overload. So the dependency to <see cref="System.Activator.CreateInstance&lt;T&gt;"/>
        /// and its limitarions also occurs here
        /// </remarks>
        public virtual IEnumerable<TTarget> MapMultiple(IEnumerable<TSource> sourceList)
        {
            return sourceList.Select(s => this.Map(s));
        }

        public System.Collections.IEnumerable MapMultiple(System.Collections.IEnumerable sourceList)
        {
            foreach (var o in sourceList)
            {
                yield return this.Map(o);
            }
        }

        /// <summary>
        /// Maps multiple source objects into multiple target objects.
        /// </summary>
        /// <param name="sourceList">The source list.</param>
        /// <param name="createTarget">A factory function to create target objects.</param>
        /// <returns>
        /// A list of target objects.
        /// </returns>
        public virtual IEnumerable<TTarget> MapMultiple(IEnumerable<TSource> sourceList, Func<TTarget> createTarget)
        {
            return sourceList.Select(s => this.Map(s, createTarget));
        }

        /// <summary>Adds an inner mapper, to map from the source to the target members.</summary>
        /// <typeparam name="TInnerSource">Type of the inner source.</typeparam>
        /// <typeparam name="TInnerTarget">Type of the inner target.</typeparam>
        /// <param name="sourceMember">Name of the source member.</param>
        /// <param name="targetMember">Name of the target member.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "No can do.")]
        public virtual void AddInnerMapper<TInnerSource, TInnerTarget>(PropertyInfo sourceMember, PropertyInfo targetMember)
        {
            var innerMapper = this.GetInnerMapper<TInnerSource, TInnerTarget>();
            this.AddMappingInfo(new MapperMappingInfo<TSource, TTarget>(innerMapper, sourceMember, targetMember));
        }

        /// <summary>Gets an inner mapper from the repository.</summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the mapper does not have a parent repository.
        /// </exception>
        /// <typeparam name="TInnerSource">Type of the source property.</typeparam>
        /// <typeparam name="TInnerTarget">Type of the target property.</typeparam>
        /// <returns>
        /// A mapper capable of translating <typeparamref name="TInnerSource"/> objects into
        /// <typeparamref name="TInnerTarget"/> ones.
        /// </returns>
        protected IMapper GetInnerMapper<TInnerSource, TInnerTarget>()
        {
            if (this.ParentRepository == null)
            {
                // TODO: review approach here -- this branch could lead to mapper "cloning"
                throw new InvalidOperationException("Mapper must be contained in a repo in order to allow inner mappers.");
            }

            var enumerableType = typeof(IEnumerable);
            var innerSourceType = typeof(TInnerSource);
            var innerTargetType = typeof(TInnerTarget);

            if (enumerableType.IsAssignableFrom(typeof(TInnerSource))
                && enumerableType.IsAssignableFrom(typeof(TInnerTarget)))
            {
                var genericEnumerableType = typeof(IEnumerable<>);
                var ts = innerSourceType.GetGenericTypeDefinition();
                var tt = innerTargetType.GetGenericTypeDefinition();

                if ((tt != null)
                    && (ts != null)
                    && genericEnumerableType.IsAssignableFrom(tt)
                    && genericEnumerableType.IsAssignableFrom(ts))
                {
                    var innerSource = innerSourceType.GetGenericArguments()[0];
                    var innerTarget = innerTargetType.GetGenericArguments()[0];
                    var realMapper = this.ParentRepository.ResolveMapper(innerSource, innerTarget);
                    return new MultyMappingAdapter(realMapper, innerSourceType);
                }
            }

            return this.ParentRepository.ResolveMapper<TInnerSource, TInnerTarget>();
        }

        /// <summary>Returns all internal mappings from the mapper.</summary>
        /// <returns>
        /// An enumerator that allows foreach to be used to get mappings in this collection.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1006:DoNotNestGenericTypesInMemberSignatures",
            Justification = "Using a method communicates the possibly non-atomic nature of this operation.")]
        protected internal abstract IEnumerable<MemberMappingInfo<TSource, TTarget>> GetMappings();

        /// <summary>
        /// Adds the specified mapping info to the internal mappings table.
        /// </summary>
        /// <param name="mappingInfo">The mapping info targetProperty be added.</param>
        protected void AddMappingInfo(MemberMappingInfo<TSource, TTarget> mappingInfo)
        {
            if (this.CurrentStatus == MapperStatus.Active)
            {
                throw new InvalidOperationException("Cannot add mappings to an already active mapper");
            }
            else if (this.CurrentStatus == MapperStatus.New)
            {
                this.CurrentStatus = MapperStatus.Initialized;
            }

            this.TypeMapping.Add(mappingInfo);
        }

        /// <summary>Gets a property explorer instance.</summary>
        /// <value>The property explorer.</value>
        /// <remarks>This property is lazy loaded.</remarks>
        protected internal virtual IPropertyExplorer PropertyExplorer
        {
            get
            {
                if (this.propertyExplorer == null)
                {
                    this.propertyExplorer = new PropertyExplorer();
                }

                return this.propertyExplorer;
            }

            private set
            {
                this.propertyExplorer = value;
            }
        }

        /// <summary>
        /// Gets the default property converter.
        /// </summary>
        /// <returns>
        /// An instance of the default property converter.
        /// </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "This is a virtual getter, for chrissake's")]
        protected internal virtual PropertyConverter GetPropertyConverter()
        {
            return new PropertyConverter();
        }

        #endregion Methods

        #region Internal classes

        private class MultyMappingAdapter : IMapper
        {
            public MultyMappingAdapter(IMapper realMapper, Type sourceType)
            {
                var methodInfo = realMapper.GetType().GetMethod("MapMultiple", new[] { sourceType });
                this.MapMethod = (s) => methodInfo.Invoke(realMapper, new[] { s });
            }

            private Func<object, object> MapMethod { get; set; }

            public object Map(object source, object target)
            {
                throw new NotImplementedException();
            }

            public object Map(object source)
            {
                return MapMethod(source);
            }

            public object Map(object source, Func<object> createTarget)
            {
                throw new NotImplementedException();
            }

            public IEnumerable MapMultiple(IEnumerable sourceList)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}