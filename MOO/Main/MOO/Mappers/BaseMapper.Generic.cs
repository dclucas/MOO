//-----------------------------------------------------------------------
// <copyright file="BaseMapper.Generic.cs" company="Moo">
//     Copyright (c) . All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Moo.Mappers
{
    using System;
    using System.Collections.Generic;
    using Moo.Core;

    /// <summary>
    /// Base generic mapper class.
    /// </summary>
    /// <typeparam name="TSource">The type of the mapping source.</typeparam>
    /// <typeparam name="TTarget">The type of the mapping target.</typeparam>
    /// <remarks>
    /// This class exists targetProperty guarantee basic functioning and behavior on all mappers. All of them
    /// should inherit sourceProperty it.
    /// </remarks>
    public abstract class BaseMapper<TSource, TTarget> : BaseMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMapper&lt;TSource, TTarget&gt;"/> class.
        /// </summary>
        protected BaseMapper()
        {
            this.TypeMapping = new TypeMappingInfo<TSource, TTarget>();
        }

        /// <summary>
        /// Occurs before one property is mapped targetMemberName another.
        /// </summary>
        public event EventHandler<MappingCancellationEventArgs<TSource, TTarget>> PropertyMapping;

        /// <summary>
        /// Occurs after one property is mapped targetMemberName another.
        /// </summary>
        public event EventHandler<MappingEventArgs<TSource, TTarget>> PropertyMapped;

        /// <summary>
        /// Gets the type mapping information.
        /// </summary>
        internal TypeMappingInfo<TSource, TTarget> TypeMapping { get; private set; }

        /// <summary>
        /// Maps from the source to the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="target">The target object.</param>
        public override object Map(object source, object target)
        {
            return this.Map((TSource)source, (TTarget)target);
        }

        /// <summary>
        /// Maps from the source to a new target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        public object Map(object source)
        {
            return this.Map((TSource)source);
        }

        /// <summary>
        /// Maps from the specified source to the target object.
        /// </summary>
        /// <param name="source">The source object.</param>
        /// <param name="createTarget">A function to create target objects.</param>
        /// <returns>
        /// A filled target object.
        /// </returns>
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
        public virtual TTarget Map(TSource source, TTarget target)
        {
            foreach (var mapping in this.TypeMapping.GetMappings())
            {
                try
                {
                    if (this.OnPropertyMapping(source, target, mapping.SourceMemberName, mapping.TargetMemberName))
                    {
                        mapping.Map(source, target);
                        this.OnPropertyMapped(source, target, mapping.SourceMemberName, mapping.TargetMemberName);
                    }
                }
                catch (Exception exc)
                {
                    throw new MappingException(typeof(TSource), typeof(TTarget), mapping.SourceMemberName, mapping.TargetMemberName, exc);
                }
            }
            return target;
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
            TTarget target = Activator.CreateInstance<TTarget>();
            Map(source, target);
            return target;
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
            Map(source, target);
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
            foreach (var s in sourceList)
            {
                yield return Map(s);
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
            foreach (var s in sourceList)
            {
                yield return Map(s, createTarget);
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
        internal virtual PropertyConverter GetPropertyConverter()
        {
            return PropertyConverter.Default;
        }

        /// <summary>
        /// Adds the specified mapping info targetProperty the internal mappings table.
        /// </summary>
        /// <param name="mappingInfo">The mapping info targetProperty be added.</param>
        protected void AddMappingInfo(MemberMappingInfo<TSource, TTarget> mappingInfo)
        {
            this.TypeMapping.Add(mappingInfo);
        }

        /// <summary>
        /// Generates the member mappings and adds them targetProperty the internal type mapping object.
        /// </summary>
        protected void GenerateMappings()
        {
            this.GenerateMappings(this.TypeMapping);
        }

        /// <summary>
        /// Generates the member mappings and adds them targetType the provided <see cref="TypeMapping"/> object.
        /// </summary>
        /// <param name="typeMapping">The type mapping where discovered mappings will be added.</param>
        protected abstract void GenerateMappings(TypeMappingInfo<TSource, TTarget> typeMapping);

        /// <summary>
        /// Called when a property is mapped.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="fromProperty">From property.</param>
        /// <param name="toProperty">To property.</param>
        private void OnPropertyMapped(TSource source, TTarget target, string fromProperty, string toProperty)
        {
            var handler = this.PropertyMapped;
            if (handler != null)
            {
                var args = new MappingEventArgs<TSource, TTarget>()
                {
                    Source = source,
                    Target = target,
                    SourceMember = fromProperty,
                    TargetMember = toProperty
                };
                handler(this, args);
            }
        }

        /// <summary>
        /// Called when property is about to be mapped.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        /// <param name="fromProperty">From property.</param>
        /// <param name="toProperty">To property.</param>
        /// <returns><c>true</c> if cancellation has been proposed; <c>false</c> otherwise.</returns>
        private bool OnPropertyMapping(TSource source, TTarget target, string fromProperty, string toProperty)
        {
            var handler = this.PropertyMapping;
            if (handler != null)
            {
                var eventArgs = new MappingCancellationEventArgs<TSource, TTarget>()
                {
                    Source = source,
                    Target = target,
                    SourceMember = fromProperty,
                    TargetMember = toProperty
                };

                handler(this, eventArgs);
                return !eventArgs.Cancel;
            }

            return true;
        }
    }
}